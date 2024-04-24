using EducationTech.MessageQueue.Common.Abstracts;

namespace EducationTech.MessageQueue.VideoConvert
{
    [Serializable]
    public class VideoConvertMessage
    {
        public string OriginalVideoPath { get; set; }
        public string ConvertedVideoDirectory { get; set; }
    }
}