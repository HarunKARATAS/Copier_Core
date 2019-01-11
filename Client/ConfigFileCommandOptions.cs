using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Copier
{
    [Verb("watchWithConfigFile", HelpText = "Start watching from config file")]
    public class ConfigFileCommandOptions
    {
        [Option('c',"configFilePath",Default = "config.txt",HelpText = "Path of the configuration file which can be used instead of passing all the options to the command line.")]
        public string ConfigFilePath { get; set; }

        [Usage]
        public static IEnumerable<Example> Examples => new List<Example>()
        {
            new Example( "Starts the copier from configuration file",new UnParserSettings{PreferShortName = true},
            new ConfigFileCommandOptions
            {
                 ConfigFilePath= "/Users/harun/Desktop/config.txt"

            })
        };
    }
}
