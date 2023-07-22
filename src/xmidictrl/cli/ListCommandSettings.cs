using System.ComponentModel;
using Spectre.Console.Cli;

namespace Midicontrol.CLI
{
    internal class ListCommandSettings : CommandSettings
    {
        [Description("Shows entity like: devices, sinks")]        
        [CommandArgument(0, "[Type]")]        
        public ListCommandEntityType Type { get; set; }
    }
}