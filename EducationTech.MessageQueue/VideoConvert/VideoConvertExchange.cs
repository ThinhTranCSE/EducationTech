using EducationTech.MessageQueue.Common.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.MessageQueue.VideoConvert
{
    public class VideoConvertExchange : IExchange
    {
        public string Name => nameof(VideoConvertExchange);

        public string Type => ExchangeType.Direct;

        public bool Durable => true;

        public bool AutoDelete => false;

        public IDictionary<string, object> Arguments => null;

        public IEnumerable<IMessageQueue> MessageQueues { get; init;  } = new IMessageQueue[]
        {
            new VideoConvertQueue()
        };
    }
}
