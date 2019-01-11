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

            Parser.Default.ParseArguments<CommandOptions, ConfigFileCommandOptions> (args)
              .WithParsed<CommandOptions>(StartWatching)
              .WithParsed<ConfigFileCommandOptions>(StartWatchingWithConfigurationFile)
              .WithNotParsed(a =>
              {
                  Environment.Exit(1);
              });

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static void StartWatchingWithConfigurationFile(ConfigFileCommandOptions options)
        {

            ILogger logger = new ConsoleLoggger();
            if (File.Exists(options.ConfigFilePath))
            {
                var configContent = File.ReadAllLines(options.ConfigFilePath);
                var commandOptions = string.Join(" ", configContent);
                Parser.Default.ParseArguments<CommandOptions>(configContent)
                    .WithParsed(StartWatching)
                    .WithNotParsed(a =>
                    {
                        logger.LogError("Configuration file does not parsed correctly.");
                        Environment.Exit(1);
                    });
            }
            else
            {
                logger.LogError($"Configuration file does not exist in {options.ConfigFilePath} path");
            }
        }

        private static void StartWatching(CommandOptions options)
        {

            ILogger logger = new ConsoleLoggger();
            logger.LogInfo("Watching has started");

             options.SourceDirectoryPath = string.IsNullOrWhiteSpace(options.SourceDirectoryPath)
            ? Directory.GetCurrentDirectory()
            : options.SourceDirectoryPath;

            IPluginLoader loader = new PluginLoader(logger,options.Debug);
            IFileCopier copier = new FileCopier(logger,options);
            if (options.Delay > 0)
            {
                copier = new QueuedFileCopier(copier, logger,options);
            }

        
            IFileWatcher fileWatcher = new FileWatcher(copier,logger);

            loader.Subscribe((IPreCopyEventBroadcaster) copier, (IPostCopyEventBroadcaster) copier);

            fileWatcher.Watch(options);
        }

    }
}
