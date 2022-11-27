using Midicontrol.Midi;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Midicontrol.CLI
{
    internal class ListCommand : AsyncCommand
    {        
        public override async Task<int> ExecuteAsync(CommandContext context)
        {                        
            Table table = new Table();
                table.AddColumns("Interface", "Name", "InUse");
                table.Border(TableBorder.Rounded);

                foreach (var item in PortMidi.MidiDeviceManager.AllDevices.Where(d => d.IsInput))
                {
                    table.AddRow(
                        item.Interface, 
                        item.Name, 
                        item.IsOpened.ToString()
                    );
                }

                AnsiConsole.Write(table);

            return 0;
        }
    }
}