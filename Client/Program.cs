using System;
using System.IO;
using CommandLine;
using Copier;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            Parser.Default.ParseArguments<CommandOptions> (args)
              .WithParsed(StartWatching)
              .WithNotParsed(a =>
              {
                  Environment.Exit(1);
              });

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static void StartWatching(CommandOptions options)
        {
            Console.WriteLine("Watching has started");

             options.SourceDirectoryPath = string.IsNullOrWhiteSpace(options.SourceDirectoryPath)
            ? Directory.GetCurrentDirectory()
            : options.SourceDirectoryPath;

            WatchFile(options);
        }

        private static void WatchFile(CommandOptions options)
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
                    Console.WriteLine($"{args.Name} File has changed {args.ChangeType}");
                }
                    CopyFile(options.SourceDirectoryPath, args.Name, options.DestinationDirectoryPath, options.OverwriteTargetFile);
                
            };
            watcher.Renamed += (sender, args) =>
            {
                if (options.Verbose)
                {
                    Console.WriteLine($"{args.OldName} File has been renamed to {args.Name}");
                }
                CopyFile(options.SourceDirectoryPath, args.Name, options.DestinationDirectoryPath, options.OverwriteTargetFile);
            };

            //Start watching the file
            watcher.EnableRaisingEvents = true;
        }


        private static void CopyFile(string sourceDirectoryPath,string fileName,string targetDirectoryPath, bool overwriteTargetFile)
        {
            var absoluteSourceFilePath = Path.Combine(sourceDirectoryPath, fileName);
            var absoluteTargetFilePath = Path.Combine(targetDirectoryPath, fileName);

            if (File.Exists(absoluteSourceFilePath) && (overwriteTargetFile == false)) return;
            File.Copy(absoluteSourceFilePath, targetDirectoryPath, overwriteTargetFile);
        }

    }
}
