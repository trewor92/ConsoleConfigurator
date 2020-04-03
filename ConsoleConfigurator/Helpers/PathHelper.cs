using ConsoleConfigurator.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleConfigurator.Helpers
{
    static class PathHelper
    {
        public static string FindChildFilePath(string parentFolderPath, string fileName)
        {
            return RecoursiveFilePathFinder(parentFolderPath, fileName);
        }

        private static string RecoursiveFilePathFinder(string parentFolderPath, string fileName)
        {
            foreach (string directory in Directory.GetDirectories(parentFolderPath))
            {
                foreach (string file in Directory.GetFiles(directory, fileName))
                {
                    return file;
                }
                return RecoursiveFilePathFinder(directory, fileName);
            }
            return null;
        }

        public static IEnumerable<string> GetChildDirectories(string parentFolderPath)
        {
            return Directory.GetDirectories(parentFolderPath).Select(x=>x.StripPrefix( parentFolderPath+ '\\'));
        }
    }
}
