using System.ComponentModel;
using Spectre.Console.Cli;

namespace Midicontrol.CLI
{
    internal class DeviceCommandSettings : CommandSettings
    {
        [Description("Enumerate available MIDI devices")]
        [CommandOption("-l|--list")]
        public bool List { get; set; }

        [CommandOption("-s|--set-default <ID>")]
        public bool SetDefault { get; set; }
    }  
}