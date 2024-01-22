using EducationTech.Databases;
using EducationTech.Extensions;
using EducationTech.Seeders;
using EducationTech.Utilities;
using EducationTech.Utilities.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

namespace EducationTech
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddUserSecrets<Program>();
            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            builder.Services.AddDbContext<MainDatabaseContext>();
            builder.Services.InjectServices();
            builder.Services.InjectRepositpories();
            builder.Services.InjectUtilities();
            builder.Services.InjectSeeders();

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
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            


            var app = builder.Build();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {

                CancellationToken cancellationToken = cancellationTokenSource.Token;
                using (var scope = app.Services.CreateScope())
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


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}