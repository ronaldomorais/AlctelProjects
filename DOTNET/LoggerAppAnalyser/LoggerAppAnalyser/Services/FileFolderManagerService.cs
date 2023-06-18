using LoggerAppAnalyser.Interfaces;
using LoggerAppAnalyser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerAppAnalyser.Services
{
    public class FileFolderManagerService : IFileFolderManagerService
    {
        public FileFolderManagerService()
        {

        }

        public List<string> GetDirs(string rootDir)
        {
            List<string> directoriesList = new List<string>();
            try
            {
                DirectoryInfo directory = new DirectoryInfo(rootDir);
                List<string> dirs = Directory.GetDirectories(rootDir).ToList();

                if (dirs.Count > 0)
                {
                    foreach (var dir in Directory.GetDirectories(rootDir))
                    {
                        directoriesList.AddRange(GetDirs(dir));
                    }
                }
                else
                {
                    directoriesList.Add(rootDir);
                }
            }
            catch (Exception ex)
            { }

            return directoriesList;
        }

        public FileDataList GetFiles(string rootDir)
        {
            FileDataList fileDatas = new FileDataList();
            try
            {
                DirectoryInfo directory = new DirectoryInfo(rootDir);

                foreach (var file in directory.GetFiles())
                {                    
                    fileDatas.Add(new FileData
                    {
                        FileSize = file.Length,
                        FileName = file.Name,
                        FullFileName = file.FullName,
                        DirName = file.FullName
                    });
                }
            }
            catch (Exception ex)
            { }

            return fileDatas;
        }
    }
}
