using EducationTech.Business.Controllers.Abstract;
using EducationTech.Exceptions.Http;
using Microsoft.AspNetCore.Http.Features;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Text.Json;

namespace EducationTech.Middlewares
{
    internal class HttpExceptionMiddleware
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


                await response.WriteAsync(json);
            }
        }
    }
}