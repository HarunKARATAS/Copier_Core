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
                    _logger.LogInfo($"{args.Name} File has changed");
                }
                _fileCopier.CopyFile(args.Name);

            };
            watcher.Renamed += (sender, args) =>
            {
                if (options.Verbose)
                {
                    _logger.LogInfo($"{args.OldName} File has been renamed to {args.Name}");
                }
                _fileCopier.CopyFile(args.Name);
            };

            //Start watching the file
            watcher.EnableRaisingEvents = true;

        }


    }
}
