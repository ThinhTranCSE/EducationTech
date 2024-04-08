using System.Runtime.CompilerServices;

namespace EducationTech.Storage
{
    public class GlobalUsings
    {
        private string _currentDirctoryPath { get; set; }   
        public string ContentRootPath => _currentDirctoryPath;
        public string StaticFilesPath => Path.Combine(ContentRootPath, "Static");
        
        public GlobalUsings()
        {
            string currentFilePath = GetThisFilePath();
            _currentDirctoryPath = Path.GetDirectoryName(currentFilePath);
        }

        private string GetThisFilePath([CallerFilePath] string path = null)
        {
            return path;
        }
    }
}
