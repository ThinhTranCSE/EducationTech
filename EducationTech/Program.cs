using EducationTech.Databases;
using EducationTech.Extensions;
using EducationTech.Seeders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

namespace EducationTech
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            builder.Services.AddDbContext<MainDatabaseContext>();
            builder.Services.InjectServices();
            builder.Services.InjectRepositpories();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ISeederExecutor, SeederExecutor>();

            var app = builder.Build();

            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
            {

                CancellationToken cancellationToken = cancellationTokenSource.Token;
                using(var scope = app.Services.CreateScope())
                {
                    ISeederExecutor seederExecutor = scope.ServiceProvider.GetRequiredService<ISeederExecutor>();
                    seederExecutor.Execute(cancellationTokenSource, args);
                }
                if(cancellationToken.IsCancellationRequested)
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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}