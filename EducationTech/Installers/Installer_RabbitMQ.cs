using EducationTech.Configurations;
using EducationTech.MessageQueue.Common.Abstracts;
using EducationTech.MessageQueue.Common.Interfaces;
using EducationTech.MessageQueue.VideoConvert;
using RabbitMQ.Client;

namespace EducationTech.Installers
{
    public class Installer_RabbitMQ : IInstaller
    {
        public IServiceCollection InstallServicesToServiceCollection(IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMQConfiguration = new RabbitMQConfiguration();
            configuration.GetSection("RabbitMQ").Bind(rabbitMQConfiguration);
            
            var factory = new ConnectionFactory
            {
                HostName = rabbitMQConfiguration.HostName,
                Port = rabbitMQConfiguration.Port,
                UserName = rabbitMQConfiguration.Username,
                Password = rabbitMQConfiguration.Password,
                VirtualHost = rabbitMQConfiguration.VirtualHost
            };

            services.AddSingleton(factory);

            var connection = factory.CreateConnection();
            services.AddSingleton(connection);

            var channel = connection.CreateModel();
            services.AddSingleton(channel);

            
            var exchanges = typeof(IExchange).Assembly.GetExportedTypes()
                .Where(x => typeof(IExchange).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x =>
                {
                    services.AddSingleton(x);
                    return (IExchange)Activator.CreateInstance(x);
                })
                .ToArray();

            foreach (var exchange in exchanges)
            {
                channel.ExchangeDeclare(exchange.Name, exchange.Type, exchange.Durable, exchange.AutoDelete, exchange.Arguments);
                foreach(var queue in exchange.MessageQueues)
                {
                    channel.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete, queue.Arguments);
                    channel.QueueBind(queue.Name, exchange.Name, queue.Key);
                }
            }


            var publisherTypes = typeof(Publisher<>).Assembly.GetExportedTypes()
                .Where(t => t.BaseType != null 
                            && t.BaseType.IsGenericType 
                            && t.BaseType.GetGenericTypeDefinition() == typeof(Publisher<>))
                .ToArray();
            foreach (var publisher in publisherTypes)
            {
                services.AddSingleton(publisher);
            }

            var consumerTypes = typeof(Consumer<>).Assembly.GetExportedTypes()
                .Where(t => t.BaseType != null
                            && t.BaseType.IsGenericType
                            && t.BaseType.GetGenericTypeDefinition() == typeof(Consumer<>))
                .ToArray();
            foreach (var consumer in consumerTypes)
            {
                services.AddSingleton(consumer);
            }


            return services;
        }

        public WebApplicationBuilder InstallServicesToWebApplicationBuilder(WebApplicationBuilder builder, IConfiguration configuration)
        {
            return builder;
        }
    }
}
