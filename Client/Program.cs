using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine;
using Copier;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var result =
            Parser.Default.ParseArguments<CommandOptions, ConfigFileCommandOptions>(args)
            /* .MapResult(
                (CommandOptions o) => StartWatchingAndReturnExitCode(o),
                (ConfigFileCommandOptions co) => StartWatchingWithConfigurationFile(co),
                err => 1
                );*/
             .WithParsed<CommandOptions>(StartWatching)
              .WithParsed<ConfigFileCommandOptions>(StartWatchingWithConfigurationFile)
              .WithNotParsed(a =>
              {
                  Environment.Exit(1);
              });

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }

        private static void  StartWatchingWithConfigurationFile(ConfigFileCommandOptions configFileOptions)
        {

            ILogger logger = new ConsoleLoggger();
            if (File.Exists(configFileOptions.ConfigFilePath))
            {
                var options = GetCommandOptionsFromConfigFile(configFileOptions);

                Parser.Default.ParseArguments<CommandOptions>(options)
                    .WithParsed(StartWatching)
                    .WithNotParsed(a =>
                    {
                        logger.LogError("Configuration file does not parsed correctly.");
                        Environment.Exit(1);
                    });
            }
            else
            {
                logger.LogError($"Configuration file does not exist in {configFileOptions.ConfigFilePath} path");
            }
        }

        private static List<string> GetCommandOptionsFromConfigFile(ConfigFileCommandOptions options)
        {
            var configContent = File.ReadAllLines(options.ConfigFilePath);
            //var commandOptions = string.Join(" ", configContent);

            var trimmedConfig = configContent.SelectMany(a =>
            {
                var result = Regex.Match(a, "\"(.*?)\"");
                if (result.Success)
                {
                    var option = a.Replace(result.Value, "");
                    return new[] { option.Trim(), result.Value.Trim().Replace("\"", "") };
                }
                return new[] { a.Trim() };
            }).ToList();

            return trimmedConfig;
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
