using System;
using System.IO;

namespace Copier
{
    public interface IFileWatcher
    {
        void Watch(CommandOptions options);
    }

    class FileWatcher : IFileWatcher
    {
        private readonly IFileCopier _fileCopier;

        private readonly ILogger _logger;

        public FileWatcher(IFileCopier fileCopier, ILogger logger)
        {
            _fileCopier = fileCopier;
            _logger = logger;
        }


        public void Watch(CommandOptions options)
        {


            var watcher = new FileSystemWatcher
            {
                Path = options.SourceDirectoryPath,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                Filter = options.FileGlobPattern
            };

            watcher.Changed += (sender, args) =>
            {
                if (args.ChangeType != WatcherChangeTypes.Changed) return;

                if (options.Verbose)
                {
                    _logger.Write($"{args.Name} File has changed {args.ChangeType}");
                }
                _fileCopier.CopyFile(options.SourceDirectoryPath, args.Name, options.DestinationDirectoryPath, options.OverwriteTargetFile);

            };
            watcher.Renamed += (sender, args) =>
            {
                if (options.Verbose)
                {
                    _logger.Write($"{args.OldName} File has been renamed to {args.Name}");
                }
                _fileCopier.CopyFile(options.SourceDirectoryPath, args.Name, options.DestinationDirectoryPath, options.OverwriteTargetFile);
            };

            //Start watching the file
            watcher.EnableRaisingEvents = true;

        }


    }
}
