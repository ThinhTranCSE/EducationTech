using Newtonsoft.Json;
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
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var body = Encoding.UTF8.GetString(buffer);

            
            using(LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
            using(LogContext.PushProperty("RequestBody", body))
            using(LogContext.PushProperty("Query", context.Request.Query))
            await _next(context);
            
        }
    }
}
