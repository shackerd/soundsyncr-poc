using Spectre.Console;
using Spectre.Console.Cli;

namespace Midicontrol
{
    public class Program {
        public static void Main(string[] args)
        {                        
            var app = new CommandApp();
            app.Configure(config => {
                config
                    .AddCommand<DeviceCommand>("device")
                    .WithAlias("d");
            });
            app.Run(args);
        }
    }

    internal class DeviceCommand : Command<DeviceCommandSettings>
    {
        // xmidictrl -v
        // xmidictrl -d -l
        // xmidictrl --device --list
        // xmidictrl --device --list
        // xmidictrl -c -l
        // xmidictrl --channel --list
        public override int Execute(CommandContext context, DeviceCommandSettings settings)
        {
            if(settings.List){
                AnsiConsole.Markup("[underline red]xmidicontrol[/], available devices:\n");
                Table table = new Table();
                table.AddColumns("Id", "Interface", "Name", "Input", "Output", "InUse");
                table.Border(TableBorder.Rounded);

                foreach (var device in PortMidi.MidiDeviceManager.AllDevices)
                {
                    table.AddRow(device.ID.ToString(), device.Interface, device.Name, device.IsInput.ToString(), device.IsOutput.ToString(), device.IsOpened.ToString());
                }

                AnsiConsole.Write(table);
            }
            // Omitted
            return 0;
        }

    }

    internal class DeviceCommandSettings : CommandSettings
    {
        [CommandOption("-l|--list")]
        public bool List { get; set; }
    }
}

