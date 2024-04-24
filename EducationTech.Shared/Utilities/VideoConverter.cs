using EducationTech.Shared.Utilities.Interfaces;
using FFMpegCore;
using FFMpegCore.Enums;

namespace EducationTech.Shared.Utilities
{
    public class VideoConverter : IVideoConverter
    {
        private string? _inputFilePath { get; set; } = null;
        private string? _outputFolderPath { get; set; } = null;
        private Codec? _videoCodec { get; set; } = null;
        private Codec? _audioCodec { get; set; } = null;
        private int? _bitrate { get; set; } = null;
        private int? _framerate { get; set; } = null;
        //private int? _width { get; set; } = null;        
        //private int? _height { get; set; } = null;
        private int? _crf { get; set; } = null;
        private VideoSize? _resolution { get; set; } = null;
        private Speed? _speed { get; set; } = null;

        public IVideoConverter From(string inputFilePath)
        {
            _inputFilePath = inputFilePath;
            return this;
        }
        public IVideoConverter To(string outputFolderPath)
        {
            _outputFolderPath = outputFolderPath;
            return this;
        }

        public IVideoConverter WithAudioCodec(Codec codec)
        {
            _audioCodec = codec;
            return this;
        }

        public IVideoConverter WithCodec(Codec codec)
        {
            _videoCodec = codec;
            return this;
        }

        public IVideoConverter WithConstantRateFactor(int crf)
        {
            _crf = crf;
            return this;
        }

        public IVideoConverter WithFramerate(int framerate)
        {
            _framerate = framerate;
            return this;
        }

        public IVideoConverter WithResolution(VideoSize size)
        {
            _resolution = size;
            return this;
        }

        //public IVideoConverter WithSize(int width, int height)
        //{
        //    _width = width;
        //    _height = height;
        //    return this;
        //}

        public IVideoConverter WithSpeedPreset(Speed speed)
        {
            _speed = speed;
            return this;
        }

        public IVideoConverter WithVariableBitrate(int bitrate)
        {
            _bitrate = bitrate;
            return this;
        }
        public Task<bool> ProcessAsync()
        {
            if (_inputFilePath == null || _outputFolderPath == null)
            {
                throw new ArgumentNullException("Input and output path must be set");
            }
            var converterArgs = FFMpegArguments.FromFileInput(_inputFilePath);
            if (!Directory.Exists(_outputFolderPath))
            {
                Directory.CreateDirectory(_outputFolderPath);
            }
            var processor = converterArgs.OutputToFile(Path.Combine(_outputFolderPath, "input.m3u8"), true, options =>
            {

                if (_videoCodec != null)
                {
                    options.WithVideoCodec(_videoCodec);
                }
                else
                {
                    options.WithCustomArgument("-c:v copy");
                }
                if (_audioCodec != null)
                {
                    options.WithAudioCodec(_audioCodec);
                }
                else
                {
                    options.WithCustomArgument("-c:a copy");
                }
                if (_bitrate != null)
                {
                    options.WithVariableBitrate(_bitrate.Value);
                }
                if (_crf != null)
                {
                    options.WithConstantRateFactor(_crf.Value);
                }
                if (_framerate != null)
                {
                    options.WithFramerate(_framerate.Value);
                }
                if (_resolution != null)
                {
                    options.WithVideoFilters(filterOptions => filterOptions.Scale(_resolution.Value));
                }
                if (_speed != null)
                {
                    options.WithSpeedPreset(_speed.Value);
                }
                options
                    //convert to hls stream
                    .WithCustomArgument($"-start_number 0 -hls_time 10 -hls_list_size 0 -hls_segment_filename {Path.Combine(_outputFolderPath, "segment_%03d.ts")}")
                    .ForceFormat("hls");
            });
            return processor.ProcessAsynchronously();
        }
    }
}
