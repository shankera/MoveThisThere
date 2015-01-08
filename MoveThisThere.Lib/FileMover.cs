using System;
using System.Collections.Generic;
using System.IO;

namespace MoveThisThere.Lib
{
    public class FileMover{
        private static FileMover _instance;

        private FileMover() {}

        public static FileMover Instance
        {
            get { return _instance ?? (_instance = new FileMover()); }
        }

        public bool Move(string sourcePath, string destPath)
        {
            var dirInfo = new DirectoryInfo(destPath);
            if (!dirInfo.Exists) Directory.CreateDirectory(destPath);
            var files = Directory.GetFiles(sourcePath);
            foreach (var file in files)
            {
                var fileLocation = file.Remove(file.IndexOf(sourcePath, System.StringComparison.Ordinal), sourcePath.Length);
                Console.Out.WriteLine(sourcePath);
                Console.Out.WriteLine(destPath);
                Console.Out.WriteLine(file);
                Console.Out.WriteLine(fileLocation);
            }
            return false;
        }
    }
}
