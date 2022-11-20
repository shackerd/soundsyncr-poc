using System.ComponentModel;
using System.Text;
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
                    .WithAlias("dev");
            });
            // app.Run(args);
            app.Run(new string[2]{"device", "--list"});
        }
    }

    internal class DeviceCommand : AsyncCommand<DeviceCommandSettings>
    {
        // xmidictrl -v
        // xmidictrl -d -l
        // xmidictrl --device --list
        // xmidictrl --device --list
        // xmidictrl -c -l
        // xmidictrl --channel --list
        public override async Task<int> ExecuteAsync(CommandContext context, DeviceCommandSettings settings)
        {
            PulseAudioClient client = new PulseAudioClient();
            await client.ConnectAsync();
            
            
            if(settings.List){
                Table table = new Table();
                table.AddColumns("Id", "Interface", "Name", "Input", "Output", "Opened");
                table.Border(TableBorder.Rounded);

                foreach (var item in PortMidi.MidiDeviceManager.AllDevices)
                {
                    table.AddRow(item.ID.ToString(), item.Interface, item.Name, item.IsInput.ToString(), item.IsOutput.ToString(), item.IsOpened.ToString());
                }

                AnsiConsole.Write(table);                            
            }
            // if(settings.SetDefault){
            if(true) {
                var device = PortMidi.MidiDeviceManager.AllDevices.Last();

                MidiDeviceListener listener = new MidiDeviceListener(device);
                listener.OnMidiMessage += async (msg) => {
                    uint value = (uint)(msg.Value * (float)10);
                    await client.PlaybackStreams.First().Proxy.SetAsync("Volume", new uint[2] { value, value });
                    
                };
                
                Console.WriteLine($"Listening to device: [underline green]{device.Name}[/]");
                Task listenerTask = listener.StartAsync();

                if((listenerTask.Status & TaskStatus.Faulted) == TaskStatus.Faulted)
                {
                    AnsiConsole.WriteException(listenerTask.Exception);
                }
                else {                    
                    Task.Run(() => AnsiConsole.Markup($"Listening to device: [underline green]{device.Name}[/]"));
                }
                
            
                Console.ReadLine();
            }
            // Omitted
            return 0;
        }        
    }    

    internal class DeviceCommandSettings : CommandSettings
    {
        [Description("Enumerate available MIDI devices")]
        [CommandOption("-l|--list")]
        public bool List { get; set; }

        [CommandOption("-s|--set-default <ID>")]
        public bool SetDefault { get; set; }
    }

    internal class SetDefaultDeviceCommandSettings: CommandSettings 
    {

    }   
}

