using System;

namespace Copier
{
    public interface IFileCopier
    {
        void CopyFile(CommandOptions options, string fileName);
    }
}
