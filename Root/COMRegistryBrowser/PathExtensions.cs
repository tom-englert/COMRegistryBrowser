using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ComBrowser
{
    internal static class PathExtensions
    {
        private static string[] pathFolders = Environment.GetEnvironmentVariable("PATH").Split(';');

        public static string GetFullFilePath(this string filePath)
        {
            const char DoubleQuote = '"';

            filePath = Environment.ExpandEnvironmentVariables(filePath);

            if (filePath.FirstOrDefault() == DoubleQuote)
            {
                filePath = new string(filePath.Skip(1).TakeWhile(c => c != DoubleQuote).ToArray());
            }

            if (!Path.IsPathRooted(filePath))
            {
                filePath = FindPath(filePath);
            }

            if (filePath.Contains('~'))
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    filePath = fileInfo.FullName;
                }
            }

            return filePath;
        }

        private static string FindPath(string filePath)
        {
            foreach (var folder in pathFolders)
            {
                var fullPath = Path.Combine(folder, filePath);
                if (File.Exists(fullPath))
                    return fullPath;
            }

            return filePath;
        }
    }
}
