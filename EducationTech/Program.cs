using EducationTech.Business.Controllers.Abstract;
using EducationTech.Databases;
using EducationTech.Extensions;
using EducationTech.Middlewares;
using EducationTech.Seeders;
using EducationTech.Utilities;
using EducationTech.Utilities.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;
using System;
using System.Diagnostics;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using EducationTech.Exceptions.Http;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using EducationTech.Installers;

namespace EducationTech
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddUserSecrets<Program>();

            builder.InstallServices(builder.Configuration);


            using (var cancellationTokenSource = new CancellationTokenSource())
            {

                CancellationToken cancellationToken = cancellationTokenSource.Token;
                var providers = builder.Services.BuildServiceProvider();
                using (var scope = providers.CreateScope())
                {
                    ISeederExecutor seederExecutor = scope.ServiceProvider.GetRequiredService<ISeederExecutor>();
                    seederExecutor.RegisterSeeders(scope);
                    seederExecutor.Execute(cancellationTokenSource, args);
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();



            app.UseCors(buidler =>
            {
                buidler.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseMiddleware<RequestLogContextMiddleware>();
            app.UseSerilogRequestLogging();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<HttpExceptionMiddleware>();
            app.UseMiddleware<ResponseRestructureMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}