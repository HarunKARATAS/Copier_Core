using System;
using System.IO;
using CommandLine;
using Copier;

namespace Client
{
    class Program
    {
        public static event Action ApplicationStarted = delegate  {};
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

            ApplicationStarted();
            Console.WriteLine("Watching has started");

             options.SourceDirectoryPath = string.IsNullOrWhiteSpace(options.SourceDirectoryPath)
            ? Directory.GetCurrentDirectory()
            : options.SourceDirectoryPath;

            ILogger logger = new ConsoleLoggger();
            IFileCopier copier = new FileCopier(logger);
            IFileWatcher fileWatcher = new FileWatcher(copier,logger);
            fileWatcher.Watch(options);
        }

    }
}
