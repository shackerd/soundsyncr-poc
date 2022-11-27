using Midicontrol.Midi;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Midicontrol.CLI
{
    internal class ListCommand : AsyncCommand<ListCommandSettings>
    {        
        private readonly IEnumerable<IMidiMessageSink> _sinks;

        public ListCommand(IEnumerable<IMidiMessageSink> sinks)
        {
            _sinks = sinks;    
        }
        public override async Task<int> ExecuteAsync(CommandContext context, ListCommandSettings settings)
        {                  
            switch (settings.Type)
            {
                case ListCommandEntityType.Devices:
                    ShowDevices();
                    break;
                case ListCommandEntityType.Sinks:
                    ShowSinks();
                    break;
            }      
            return 0;
        }

        private void ShowDevices() {
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
        }

        private void ShowSinks() {
            Table table = new Table();
            table.AddColumns("Loaded sinks");
            table.Border(TableBorder.Rounded);

            foreach (var sink in _sinks)
            {                
                table.AddRow(
                    sink.Name
                );
            }

            AnsiConsole.Write(table);
        }
    }
}