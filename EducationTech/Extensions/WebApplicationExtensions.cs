using EducationTech.MessageQueue.Common.Abstracts;
using EducationTech.Storage;

namespace EducationTech.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureStaticFiles(this WebApplication app)
        {

            //create directory for static files
            var globalUsing = GlobalReference.Instance;
            string staticFilesPath = Path.Combine(globalUsing.StorageRootPath, "Static");
            if (!Directory.Exists(staticFilesPath))
            {
                Directory.CreateDirectory(staticFilesPath);
            }

            return app;
        }

        public static WebApplication ResolveAllConsumers(this WebApplication app)
        {
            var consumerTypes = typeof(Consumer<>).Assembly.GetExportedTypes()
                .Where(t => t.BaseType != null
                            && t.BaseType.IsGenericType
                            && t.BaseType.GetGenericTypeDefinition() == typeof(Consumer<>))
                .ToArray();
            foreach (var consumer in consumerTypes)
            {
                app.Services.GetRequiredService(consumer);
            }


            return app;
        }
    }
}
