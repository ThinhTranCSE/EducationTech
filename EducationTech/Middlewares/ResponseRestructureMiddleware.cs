using EducationTech.Annotations;
using EducationTech.Business.Controllers.Abstract;
using EducationTech.Exceptions.Http;
using Newtonsoft.Json;
using System.Net;
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
            // Check if the action method has the SkipMiddlewareAttribute
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var skipAttribute = endpoint.Metadata.GetMetadata<SkipRestructurePhaseAttribute>();
                if (skipAttribute != null)
                {
                    await next(context);
                    return; // Skip the middleware
                }
            }



            Stream originalBody = context.Response.Body;
            try
            {
                string responseBody = null;
                using (var memStream = new MemoryStream())
                {
                    //Replace stream f or upstream calls.
                    context.Response.Body = memStream;
                    //continue up the pipeline
                    await next(context);

                    int code = context.Response.StatusCode;
                    if(code >= 300)
                    {
                        if(code == 401)
                        {
                            throw new HttpException(HttpStatusCode.Unauthorized, "Unauthorized");
                        }
                        else if(code == 403)
                        {
                            throw new HttpException(HttpStatusCode.Forbidden, "Forbidden");
                        }
                        throw new HttpException(code, "Something went wrong");
                    }


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
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                //and finally, reset the stream for downstream calls
                context.Response.Body = originalBody;
            }
        }
    }
}
