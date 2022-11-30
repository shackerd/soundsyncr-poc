using System.ComponentModel;
using Spectre.Console.Cli;

namespace Midicontrol.CLI
{
    internal class StartCommandSettings : CommandSettings
    {
        [Description("Verbose")]
        [CommandOption("-v|--verbose")]
        public bool Vebose { get; set; }        
    }  
}