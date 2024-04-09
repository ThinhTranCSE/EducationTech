using System.Runtime.CompilerServices;

namespace EducationTech.Storage
{
    public class GlobalUsings
    {
        private string _currentDirctoryPath { get; set; }   
        public string StorageRootPath => _currentDirctoryPath;
        public string StaticFilesPath => Path.Combine(StorageRootPath, "Static");

        public string ContentRootPath => GetContentRootPath();

        public GlobalUsings()
        {
            string currentFilePath = GetThisFilePath();
            _currentDirctoryPath = Path.GetDirectoryName(currentFilePath);
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
