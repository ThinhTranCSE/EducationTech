using EducationTech.Storage.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.Storage
{
    public class FileTypeCollection
    {
        private Dictionary<string, FileType> _fileTypeDictionary { get; set; }
        public FileTypeCollection((FileType, string[])[] fileTypeAndTheirExtensions)
        {
            _fileTypeDictionary = new Dictionary<string, FileType>();
            foreach (var (fileType, extensions) in fileTypeAndTheirExtensions)
            {
                foreach (var extension in extensions)
                {
                    _fileTypeDictionary.Add(extension, fileType);
                }
            }
        }
        public FileType? this[string extension]
        {
            get
            {
                if (!_fileTypeDictionary.ContainsKey(extension))
                {
                    return null;
                }
                return _fileTypeDictionary[extension];
            }
        }
    }
}
