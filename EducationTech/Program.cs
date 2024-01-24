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

namespace EducationTech
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureLogging();

            builder.Configuration.AddUserSecrets<Program>();
            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Keys
                                            .SelectMany(key => context.ModelState[key].Errors.Select(x => $"{key}: {x.ErrorMessage}"))
                                            .ToArray();
                    throw new HttpException(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(errors));
                    
                };
            });
            

            builder.Services.AddDbContext<MainDatabaseContext>();
            builder.Services
                .InjectServices()
                .InjectRepositpories()
                .InjectUtilities()
                .InjectSeeders();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "EducationTech", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddScoped<ISeederExecutor, SeederExecutor>();
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(options =>
                {
                    var scope = builder.Services.BuildServiceProvider().CreateScope();

                    var authUltils = scope.ServiceProvider.GetRequiredService<IAuthUtils>();
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireSignedTokens = true,
                        IssuerSigningKeyResolver = authUltils.KeysResolver,

                        ClockSkew = TimeSpan.Zero
                    };

                    
                });

            builder.Services.ApplyPolicies();




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

            builder.Services.AddCors();


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