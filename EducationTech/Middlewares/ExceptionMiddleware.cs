using EducationTech.Controllers.Abstract;
using EducationTech.Exceptions.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO.Pipelines;
using System.Net;
using System.Text;

namespace EducationTech.Middlewares
{
    internal class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception exception)
            {

                var responseFeature = context.Features.Get<IHttpResponseFeature>();
                responseFeature.ReasonPhrase = exception.Message;

                var response = context.Response;

                var responseStructure = new ResponseMessage
                {
                    Status = 500,
                    Message = exception.Message,
                    Errors = exception.StackTrace
                };
                var json = JsonConvert.SerializeObject(responseStructure);
                var buffer = Encoding.UTF8.GetBytes(json);

                response.StatusCode = 500;
                response.ContentType = "application/json";


                await response.WriteAsync(json);
            }
        }
    }
}
