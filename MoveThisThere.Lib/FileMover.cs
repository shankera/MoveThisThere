using System;
using System.IO;

namespace MoveThisThere.Lib
{
    public class FileMover
    {
        private static int _copied;
        private static int _overwritten;
        private static int _total;
        private FileMover() {}

        public static int[] Move(string sourcePath, string destPath)
        {
            var destDir = new DirectoryInfo(destPath);
            if (!destDir.Exists) Directory.CreateDirectory(destPath);

            var sourceDir = new DirectoryInfo(sourcePath);
            _copied = 0;
            _overwritten = 0;
            _total = 0;
            MoveAllInFolder(sourceDir, sourcePath, destPath);
            return new[]{ _copied,_overwritten, _total };
        }

        private static void MoveAllInFolder(DirectoryInfo sourceDir, string sourcePath, string destPath)
        {
            foreach (var dir in sourceDir.GetDirectories())
            {
                MoveAllInFolder(dir, sourcePath, destPath);
            }

            foreach (var file in sourceDir.GetFiles())
            {
                _total++;
                var destpathandfile = RemovePathorFileName(sourcePath, file.FullName);
                var filelessPath = RemovePathorFileName(file.Name, destpathandfile);

                if (!File.Exists(destPath + destpathandfile))
                {
                    Directory.CreateDirectory(destPath + filelessPath);
                    File.Copy(file.FullName, destPath + destpathandfile);
                    _copied++;
                }
                else
                {
                    if (File.GetLastWriteTime(file.FullName).Equals(File.GetLastWriteTime(destPath + destpathandfile))) continue;
                    File.SetAttributes(file.FullName, FileAttributes.Normal);
                    Directory.CreateDirectory(destPath + filelessPath);
                    File.Copy(file.FullName, destPath + destpathandfile, true);
                    File.SetAttributes(destPath + filelessPath, FileAttributes.Normal);
                    _overwritten++;
                }
            }
        }

        private static string RemovePathorFileName(string remove, string keep)
        {
            return keep.Remove(keep.IndexOf(remove, StringComparison.Ordinal), remove.Length);
        }

    }
}
