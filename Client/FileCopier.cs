using System;
using System.IO;

namespace Copier
{
    class FileCopier : IFileCopier,IPreCopyEventBroadcaster, IPostCopyEventBroadcaster
    {

        public event Action<string> PreCopyEvent = delegate { };
        public event Action<string> PostCopyEvent = delegate { };

        private readonly ILogger _logger;

        public FileCopier(ILogger logger)
        {
            _logger = logger;
        }

        public void CopyFile(CommandOptions options, string fileName)
        {
            var absoluteSourceFilePath = Path.Combine(options.SourceDirectoryPath, fileName);
            var absoluteTargetFilePath = Path.Combine(options.DestinationDirectoryPath, fileName);

            if (File.Exists(absoluteSourceFilePath) && (options.OverwriteTargetFile == false)) 
            {

                _logger.Write($"{fileName} exist. Skipped because OverwriteTargetFile is set false.");
                return; 
            }

            PreCopyEvent(absoluteSourceFilePath);
            File.Copy(absoluteSourceFilePath, absoluteTargetFilePath, options.OverwriteTargetFile);
            PostCopyEvent(absoluteSourceFilePath);
        }

    }

    public interface IPostCopyEventBroadcaster
    {
        event Action<string> PostCopyEvent;
    }

    public interface IPreCopyEventBroadcaster
    {
        event Action<string> PreCopyEvent;
    }
}
