using EducationTech.Business.Controllers.Abstract;
using EducationTech.Exceptions.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using System;
using System.IO.Pipelines;
using System.Net;
using System.Text;

namespace EducationTech.Middlewares
{
    internal class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;
        public ExceptionMiddleware(RequestDelegate next, Serilog.ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
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
