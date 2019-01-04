using System;
using System.Collections;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Copier
{
    public class CommandOptions
    {
        [Option('s', "sourceDirectoryPath", HelpText = "Parent directory where the files will be. If skipped current directory will be searched.")]
        public string SourceDirectoryPath { get; set; }

        [Option('f', "fileGlobPattern", Required = true, HelpText = "Files to be searched. Accepts glob pattern for pattern matching.")]
        public string FileGlobPattern { get; set; }

        [Option('d', "destinationDirectoryPath", Required = true, HelpText = "Destination directory path")]
        public string DestinationDirectoryPath { get; set; }

        [Usage]
        public static IEnumerable<Example> Examples => new List<Example>()
        {
            new Example( "Starts the copier",new UnParserSettings{PreferShortName = true},
            new CommandOptions
            {
                SourceDirectoryPath = "/Users/harun/Desktop",
                FileGlobPattern = "*.jpg",
                DestinationDirectoryPath = "/Users/harun/Desktop/Cambridge_English_Vocabulary_ in_Use/Tools"

            })
        };
    }
}
