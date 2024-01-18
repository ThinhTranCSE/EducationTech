using EducationTech.Exceptions.Http;

namespace EducationTech.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpExceptions(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<HttpExceptionMiddleware>();
            return builder;
        }
    }
}
