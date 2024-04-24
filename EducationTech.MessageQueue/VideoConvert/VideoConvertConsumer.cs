using EducationTech.MessageQueue.Common.Abstracts;
using EducationTech.Shared.Utilities;
using EducationTech.Shared.Utilities.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.MessageQueue.VideoConvert
{
    public class VideoConvertConsumer : Consumer<VideoConvertMessage>
    {
        private readonly IVideoConverter _videoConverter = new VideoConverter();
      
        public VideoConvertConsumer(IModel channel, VideoConvertExchange exchange) : base(channel, exchange)
        {

        }

        public override void Consume(VideoConvertMessage message)
        {
            //try
            //{
            //    _videoConverter
            //        .From(message.OriginalVideoPath)
            //        .To(message.ConvertedVideoDirectory)
            //        .ProcessAsync()
            //        .GetAwaiter()
            //        .GetResult();
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            _videoConverter
                    .From(message.OriginalVideoPath)
                    .To(message.ConvertedVideoDirectory)
                    .ProcessAsync()
                    .GetAwaiter()
                    .GetResult();
        }
    }
}
