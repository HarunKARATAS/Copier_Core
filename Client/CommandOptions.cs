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

        [Option('o', "overwriteTargetFile", Default = false, HelpText = "If passedd true, Copier will overwrite existing files at the target location")]
        public bool OverwriteTargetFile { get; set; }

        [Option('v', "verbose", Default = false,Required =false, HelpText = "If passedd true, more information will outputted to the console.")]
        public bool Verbose { get; set; }

        [Option('e', "debug", Default = false, Required = false, HelpText = "Show debug information.")]
        public bool Debug { get; set; }


        [Usage]
        public static IEnumerable<Example> Examples => new List<Example>()
        {
            new Example( "Starts the copier",new UnParserSettings{PreferShortName = true},
            new CommandOptions
            {
                SourceDirectoryPath = "/Users/harun/Desktop",
                FileGlobPattern = "*.jpg",
                DestinationDirectoryPath = "/Users/harun/Desktop/Cambridge_English_Vocabulary_ in_Use/Tools"

            }),
            new Example( "Starts the copier and the overrides target files.",new UnParserSettings{PreferShortName = true},
            new CommandOptions
            {
                SourceDirectoryPath = "/Users/harun/Desktop",
                FileGlobPattern = "*.jpg",
                DestinationDirectoryPath = "/Users/harun/Desktop/Cambridge_English_Vocabulary_ in_Use/Tools",
                OverwriteTargetFile = true

            }),
            new Example( "Starts the copier and the overrides target files and output verbose messages.",new UnParserSettings{PreferShortName = true},
            new CommandOptions
            {
                SourceDirectoryPath = "/Users/harun/Desktop",
                FileGlobPattern = "*.jpg",
                DestinationDirectoryPath = "/Users/harun/Desktop/Cambridge_English_Vocabulary_ in_Use/Tools",
                OverwriteTargetFile = true,
                Verbose = true

            })
        };
    }
}
