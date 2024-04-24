using EducationTech.MessageQueue.Common.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.MessageQueue.Common.Abstracts
{
    public abstract class Consumer<TMessage> : IConsumer<TMessage> 
        where TMessage : class
    {
        protected readonly BinaryFormatter _binaryFormatter = new();
        protected readonly IModel _channel;
        protected readonly IExchange _exchange;
        protected readonly EventingBasicConsumer _consumer;

        protected Consumer(IModel channel, IExchange exchange)
        {
            _channel = channel;
            _exchange = exchange;
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += Consumer_Received;
            string queueName = _exchange.MessageQueues.First().Name;
            _channel.BasicConsume(queueName, true, _consumer);
        }
        private void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            var message = Deserialize(e.Body);
            Consume(message);
        }

        private TMessage Deserialize(ReadOnlyMemory<byte> body)
        {
            using(var stream = new MemoryStream(body.ToArray()))
            {
                return _binaryFormatter.Deserialize(stream) as TMessage;
            }
        }

        public abstract void Consume(TMessage message);

    }
}
