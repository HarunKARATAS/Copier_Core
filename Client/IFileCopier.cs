using System;

namespace Copier
{
    public interface IFileCopier
    {
        void CopyFile(string sourceDirectoryPath, string fileName, string targetDirectoryPath, bool overwriteTargetFile);
    }
}
