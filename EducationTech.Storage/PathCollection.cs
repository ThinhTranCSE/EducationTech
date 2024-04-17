using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Storage
{
    public class PathCollection
    {
        private Dictionary<string, string> _pathDictionary { get; set; }
        private string _uncategorizedFilesPath { get; set; }
        public PathCollection((string, string[])[] extensionAndTheirPaths, string uncategorizedFilesPath)
        {
            _pathDictionary = new Dictionary<string, string>();
            foreach (var (path, extensions) in extensionAndTheirPaths)
            {
                foreach (var extension in extensions)
                {
                    _pathDictionary.Add(extension, path);
                }
            }
            _uncategorizedFilesPath = uncategorizedFilesPath;
        }
        public string this[string extension]
        {
            get
            {
                if (!_pathDictionary.ContainsKey(extension))
                {
                    return _uncategorizedFilesPath;
                }
                return _pathDictionary[extension];
            }
        }
    }
}
