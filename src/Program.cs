using Spectre.Console;


namespace Midicontrol
{
    public class Program {
        public static void Main(string[] args)
        {
            AnsiConsole.Markup("[underline red]xmidicontrol[/], available devices:\n");
            Table table = new Table();
            table.AddColumns("Id", "Interface", "Name", "Input", "Output", "InUse");
            table.Border(TableBorder.Rounded);

            // public int ID { readonly get; set; }
            // public string Interface { get; }
            // public string Name { get; }
            // public bool IsInput { get; }
            // public bool IsOutput { get; }
            // public bool IsOpened { get; }

            foreach (var device in PortMidi.MidiDeviceManager.AllDevices)
            {
                table.AddRow(device.ID.ToString(), device.Interface, device.Name, device.IsInput.ToString(), device.IsOutput.ToString(), device.IsOpened.ToString());
            }

            AnsiConsole.Write(table);
        }
    }
}

