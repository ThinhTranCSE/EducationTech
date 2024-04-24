using EducationTech.MessageQueue.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.MessageQueue.VideoConvert
{
    public class VideoConvertQueue : IMessageQueue
    {
        public string Name => nameof(VideoConvertQueue);
        public string Key => "video_convert_key";

        public bool Durable => true;

        public bool Exclusive => false;

        public bool AutoDelete => false;

        public IDictionary<string, object> Arguments => null;
    }
}
