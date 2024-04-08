using FFMpegCore.Enums;

namespace EducationTech.Shared.Utilities.Interfaces
{
    public interface IVideoConverter
    {
        public IVideoConverter From(string inputFilePath);
        public IVideoConverter To(string outputFolderPath);

        public IVideoConverter WithFramerate(int framerate);

        public IVideoConverter WithConstantRateFactor(int crf);
        //public IVideoConverter WithSize(int width, int height);

        public IVideoConverter WithCodec(Codec codec);

        public IVideoConverter WithAudioCodec(Codec codec);

        public IVideoConverter WithVariableBitrate(int bitrate);

        public IVideoConverter WithResolution(VideoSize size);

        public IVideoConverter WithSpeedPreset(Speed speed);

        public Task<bool> ProcessAsync();
    }
}
