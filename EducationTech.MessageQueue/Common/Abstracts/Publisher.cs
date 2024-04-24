using EducationTech.MessageQueue.Common.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.MessageQueue.Common.Abstracts
{
    public abstract class Publisher<TMessage> : IPublisher<TMessage>
        where TMessage : class
    {
        protected readonly BinaryFormatter _binaryFormatter = new();
        protected readonly IModel _channel;
        protected readonly IExchange _exchange;
        protected virtual string _defaultRoutingKey => _exchange.MessageQueues.First().Key;
        protected virtual bool _persistent => true;
        protected virtual string _expiration => default;

        protected Publisher(IModel channel, IExchange exchange)
        {
            _channel = channel;
            _exchange = exchange;
        }
        protected void Publish<T>(T message, string routingKey) where T : class
        {
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = _persistent;
            properties.Expiration = _expiration;

            var body = Serialize(message);
            _channel.BasicPublish(_exchange.Name, routingKey, properties, body);
        }

        private byte[] Serialize<T>(T message) where T : class
        {
            using (var stream = new MemoryStream())
            {
                _binaryFormatter.Serialize(stream, message);
                return stream.ToArray();
            }
        }

        public virtual void Publish(TMessage message)
        {
            Publish(message, _defaultRoutingKey);
        }
    }
}
