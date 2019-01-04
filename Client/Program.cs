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
            WatchFile(options.FileGlobPattern, options.SourceDirectoryPath);
        }

        private static void WatchFile(String filePattern, String sourceDirectoryPath)
        {
            var watcher = new FileSystemWatcher
            {
                Path = sourceDirectoryPath,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                Filter = filePattern
            };

            watcher.Changed += (sender, args) =>
            {
                if (args.ChangeType == WatcherChangeTypes.Changed)
                {
                    Console.WriteLine($"{args.Name} File has changed{args.ChangeType}");
                }
            };
            watcher.Renamed += (sender, args) => Console.WriteLine($"{args.OldName} File has been renamed to {args.Name}");

            //Start watching the file
            watcher.EnableRaisingEvents = true;
        }


     
    }
}
