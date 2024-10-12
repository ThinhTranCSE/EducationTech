using EducationTech.Storage;

namespace EducationTech.Middlewares
{
    public class GetSchemeAndHostBeforeRequestExecutedMiddleware
    {
        private readonly RequestDelegate _next;

        public GetSchemeAndHostBeforeRequestExecutedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            GlobalReference.Instance.HostScheme = context.Request.Scheme;
            GlobalReference.Instance.HostName = context.Request.Host.Value;
            await _next(context);
        }
    }
}
