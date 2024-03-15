using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.IO;
using Serilog;
using Serilog.Context;
using System.Text;

namespace EducationTech.Middlewares
{
    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //context.Request.EnableBuffering();
            //var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            //context.Request.Body.Position = 0;



            using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier)) 
            using (LogContext.PushProperty("Query", context.Request.Query)) 
            //using (LogContext.PushProperty("RequestBody", requestBody, true)) 
            await _next(context);
            
        }
    }
}
