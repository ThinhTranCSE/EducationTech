using EducationTech.Controllers.Abstract;
using Newtonsoft.Json;
using System.Text;

namespace EducationTech.Middlewares
{
    public class ResponseRestructureMiddleware
    {
        private readonly RequestDelegate next;
        public ResponseRestructureMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Stream originalBody = context.Response.Body;
            try
            {
                string responseBody = null;
                using (var memStream = new MemoryStream())
                {
                    //Replace stream for upstream calls.
                    context.Response.Body = memStream;
                    //continue up the pipeline
                    await next(context);
                    //back from upstream call.
                    //memory stream now hold the response data
                    //reset position to read data stored in response stream
                    memStream.Position = 0;
                    responseBody = new StreamReader(memStream).ReadToEnd();
                }//dispose of previous memory stream. 
                 //lets convert responseBody to something we can use
                var data = JsonConvert.DeserializeObject(responseBody);
                //create your wrapper response and convert to JSON
                var responseStructure = new ResponseMessage()
                {
                    Status = 200,
                    Message = "Success",
                    Data = data
                };
                //convert json to a stream
                var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseStructure));
                context.Response.ContentLength = buffer.Length;
                using (var output = new MemoryStream(buffer))
                {
                    await output.CopyToAsync(originalBody);
                }//dispose of output stream
            }
            finally
            {
                //and finally, reset the stream for downstream calls
                context.Response.Body = originalBody;
            }
        }
    }
}
