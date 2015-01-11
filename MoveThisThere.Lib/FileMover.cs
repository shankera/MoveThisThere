using System;
using System.IO;

namespace MoveThisThere.Lib
{
    public class FileMover{

        private FileMover() {}

        public static bool Move(string sourcePath, string destPath)
        {
            var destDir = new DirectoryInfo(destPath);
            if (!destDir.Exists) Directory.CreateDirectory(destPath);

            var sourceDir = new DirectoryInfo(sourcePath);

            MoveAllInFolder(sourceDir, sourcePath, destPath);
            return true;
        }

        private static void MoveAllInFolder(DirectoryInfo sourceDir, string sourcePath, string destPath)
        {
            foreach (var dir in sourceDir.GetDirectories())
            {
                MoveAllInFolder(dir, sourcePath, destPath);
            }

            foreach (var file in sourceDir.GetFiles())
            {
                var destpathandfile = RemovePathorFileName(sourcePath, file.FullName);
                var filelessPath = RemovePathorFileName(file.Name, destpathandfile);
                if (!new DirectoryInfo(destPath + destpathandfile).Exists) Directory.CreateDirectory(destPath + filelessPath);
                File.Copy(file.FullName, destPath + destpathandfile);
            }
        }

        private static string RemovePathorFileName(string remove, string keep)
        {
            return keep.Remove(keep.IndexOf(remove, StringComparison.Ordinal), remove.Length);
        }

    }
}
