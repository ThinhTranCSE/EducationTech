using EducationTech.MessageQueue.Common.Abstracts;
using EducationTech.MessageQueue.Common.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.MessageQueue.VideoConvert
{
    public class VideoConvertPublisher : Publisher<VideoConvertMessage>
    {
        public VideoConvertPublisher(IModel channel, VideoConvertExchange exchange) : base(channel, exchange)
        {
        }

    }
}
