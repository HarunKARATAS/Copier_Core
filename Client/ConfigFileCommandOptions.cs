using System;
using CommandLine;

namespace Copier
{
    public class ConfigFileCommandOptions
    {
        [Option('c',"configFilePath",Default = "config.txt",HelpText = "Path of the configuration file which can be used instead of passing all the options to the command line.")]
        public string ConfigFilePath { get; set; }
    }
}
