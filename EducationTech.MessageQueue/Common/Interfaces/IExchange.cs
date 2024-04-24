using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.MessageQueue.Common.Interfaces
{
    public interface IExchange
    {
        string Name { get; }
        string Type { get; }
        bool Durable { get; }
        bool AutoDelete { get; }
        IDictionary<string, object> Arguments { get; }
        IEnumerable<IMessageQueue> MessageQueues { get; }
    }
}
