using EducationTech.Business.Business.Interfaces;
using EducationTech.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text;

namespace EducationTech.Annotations
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;

        public CacheAttribute(int timeToLiveSeconds = 1000 )
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var redisConfigurations = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();
            if (!redisConfigurations.Enabled)
            {
                await next();
                return;
            }
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            string cacheKey = GenerateKeyFromRequest(context.HttpContext.Request);

            var cachedValue = await cacheService.GetAsync(cacheKey);
            if(cachedValue != null)
            {
                var deserializedValue = JsonConvert.DeserializeObject(cachedValue);
                await WriteToResponse(context.HttpContext.Response, deserializedValue);
                return;
            }

            var executedContext = await next();
            if(executedContext.Result is ObjectResult result )
            {
                
                await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
        }

        private string GenerateKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path);

            foreach(var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }

        private async Task WriteToResponse(HttpResponse response, object value)
        {
            Stream originalBody = response.Body;
            try
            {
                //convert json to a stream
                var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
                response.ContentLength = buffer.Length;
                using (var output = new MemoryStream(buffer))
                {
                    await output.CopyToAsync(originalBody);
                }//dispose of output stream
                response.ContentType = "application/json";
                response.StatusCode = 200;
            }
            finally
            {
                //and finally, reset the stream for downstream calls
                response.Body = originalBody;
            }
        }
    }
}
