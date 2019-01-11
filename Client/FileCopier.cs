using System;
using System.IO;

namespace Copier
{
    class FileCopier : IFileCopier,IPreCopyEventBroadcaster, IPostCopyEventBroadcaster
    {

        public event Action<string> PreCopyEvent = delegate { };
        public event Action<string> PostCopyEvent = delegate { };

        private ILogger _logger;
        private CommandOptions _options;

        public FileCopier(ILogger logger,CommandOptions options)
        {
            _logger = logger;
            _options = options;
        }

        public void CopyFile(string fileName)
        {
            var absoluteSourceFilePath = Path.Combine(_options.SourceDirectoryPath, fileName);
            var absoluteTargetFilePath = Path.Combine(_options.DestinationDirectoryPath, fileName);

            if (File.Exists(absoluteSourceFilePath) && (_options.OverwriteTargetFile == false)) 
            {

                _logger.LogInfo($"{fileName} exist. Skipping the copy operation because OverwriteTargetFile is set false.");
                return; 
            }

            PreCopyEvent(absoluteSourceFilePath);
            File.Copy(absoluteSourceFilePath, absoluteTargetFilePath, _options.OverwriteTargetFile);
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
