using EducationTech.Business.Shared.Exceptions.Http;
using EducationTech.Controllers.Abstract;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using System.Text;

namespace EducationTech.Middlewares;

public class HttpExceptionMiddleware
{
    private readonly RequestDelegate next;

    public HttpExceptionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (HttpException httpException)
        {
            //read the request body
            //var request = context.Request;
            //var requestBody = string.Empty;
            //request.EnableBuffering();
            //request.Body.Position = 0;
            //using (var reader = new StreamReader(request.Body, Encoding.UTF8))
            //{
            //    requestBody = await reader.ReadToEndAsync();
            //}
            //request.Body.Position = 0;


            var responseFeature = context.Features.Get<IHttpResponseFeature>();
            responseFeature.ReasonPhrase = httpException.Message;

            var response = context.Response;


            var responseStructure = new ResponseMessage
            {
                Status = httpException.StatusCode,
                Message = httpException.Message,
                Errors = httpException.StackTrace
            };
            var json = JsonConvert.SerializeObject(responseStructure);
            var buffer = Encoding.UTF8.GetBytes(json);

            response.StatusCode = httpException.StatusCode;
            response.ContentType = "application/json";
            response.ContentLength = buffer.Length;

            await response.WriteAsync(json);
        }
    }
}