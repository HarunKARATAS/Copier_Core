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

            IFileCopier copier = new FileCopier();
            ILogger logger = new ConsoleLoggger();
            IFileWatcher fileWatcher = new FileWatcher(copier,logger);
            fileWatcher.Watch(options);
        }

    }
}
