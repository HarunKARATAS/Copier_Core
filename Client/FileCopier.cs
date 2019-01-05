using System.IO;

namespace Copier
{
    class FileCopier : IFileCopier
    {
        public void CopyFile(string sourceDirectoryPath, string fileName, string targetDirectoryPath, bool overwriteTargetFile)
        {
            var absoluteSourceFilePath = Path.Combine(sourceDirectoryPath, fileName);
            var absoluteTargetFilePath = Path.Combine(targetDirectoryPath, fileName);

            if (File.Exists(absoluteSourceFilePath) && (overwriteTargetFile == false)) return;
            File.Copy(absoluteSourceFilePath, targetDirectoryPath, overwriteTargetFile);
        }

    }
}
