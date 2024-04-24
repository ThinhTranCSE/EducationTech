using EducationTech.Storage.Enums;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace EducationTech.Storage
{
    public class GlobalUsings
    {
#region Paths
        private string _currentDirctoryPath { get; set; }   
        public string StorageRootPath => _currentDirctoryPath;
        public string ContentRootPath => GetContentRootPath();
        public string StaticFilesPath => Path.Combine(StorageRootPath,"Static");
        public string TempFilesPath => GetCategoryDirectoryPath("Temps");
        public string ImageFilesPath => GetCategoryDirectoryPath("Images");
        public string StreamFilesPath => GetCategoryDirectoryPath("Streams");
        public string UncategorizedFilesPath => GetCategoryDirectoryPath("Uncategorized");
#endregion

#region Upload File Configs
        public long UploadSessionTimeOut => 1 * 60;
        public int UploadChunkSize => 1024 * 1024 * 2;
        #endregion

#region Collections
        public PathCollection PathCollection { get; set; }
        public FileTypeCollection FileTypeCollection { get; set; }
#endregion
        public GlobalUsings()
        {
            string currentFilePath = GetThisFilePath();
            _currentDirctoryPath = Path.GetDirectoryName(currentFilePath)!;


            var imageExtensions = new string[] { "ai", "bmp", "cur", "gif", "ico", "icns", "jpg", "jpeg", "png", "ps", "psd", "svg", "tif", "tiff" };
            var videoExtensions = new string[] { "3g2", "3gp", "avi", "flv", "h264", "m4v", "mkv", "mov", "mp4", "mpg", "mpeg", "rm", "swf", "vob", "wmv" };
            (FileType, string[])[] fileTypeAndTheirExtensions = new (FileType, string[])[]
            {
                (FileType.Image, imageExtensions),
                (FileType.Video, videoExtensions)
            };
            FileTypeCollection = new FileTypeCollection(fileTypeAndTheirExtensions);
            (string, string[])[] extensionAndTheirPaths = new (string, string[])[]
            {
                (ImageFilesPath, imageExtensions),
                (StreamFilesPath, videoExtensions)
            };
            PathCollection = new PathCollection(extensionAndTheirPaths, UncategorizedFilesPath);
        }



        private string GetCategoryDirectoryPath(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentNullException(nameof(category));
            }
            string categoryPath= Path.Combine(StaticFilesPath, category);
            if(!Directory.Exists(categoryPath))
            {
                Directory.CreateDirectory(categoryPath);
            }
            return categoryPath;
        }
        private string GetThisFilePath([CallerFilePath] string path = null)
        {
            return path;
        }
        private string GetContentRootPath()
        {
            string rootPath = Directory.GetParent(StorageRootPath).FullName;
            return Path.Combine(rootPath, "EducationTech");
        }
    }
} 
