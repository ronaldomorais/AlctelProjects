using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerAppAnalyser.Models
{
    public class FileData
    {
        public long FileSize { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string DirName { get; set; } = string.Empty;
        public string FullFileName { get; set; } = string.Empty;
    }

    public class FileDataList : List<FileData> { }
}
