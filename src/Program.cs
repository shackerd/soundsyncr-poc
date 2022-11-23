using System.ComponentModel;
using System.Text;
using Spectre.Console;
using Spectre.Console.Cli;
using Midicontrol.Midi;
using Midicontrol.PulseAudio;
using Microsoft.Extensions.DependencyInjection;
using Midicontrol.Infrastructure;

namespace Midicontrol
{
    public class Program {
        public static void Main(string[] args)
        {                        
            ITypeRegistrar registrar = Setup();

            CommandApp app = new CommandApp(registrar);                    

            app.Configure(config => {
                config                
                    .AddCommand<DeviceCommand>("device")
                    .WithAlias("dev");
            });            
            
            // app.Run(args);
            app.Run(new string[2]{"device", "--list"});            
        }        

        private static ITypeRegistrar Setup(){

            IServiceCollection services = new ServiceCollection();
            
            services.AddSingleton<SynchronizationContext>();
            services.AddSingleton<PulseAudioClient>();            
            services.AddTransient<IMidiMessageSink, PulseAudioMidiSink>();
            

            return new TypeRegistrar(services);
        }
    }    

    internal class DeviceCommand : AsyncCommand<DeviceCommandSettings>
    {
        private readonly SynchronizationContext _synchronizationContext;
        private readonly IEnumerable<IMidiMessageSink> _sinks;
        private readonly PulseAudioClient _client;

        public DeviceCommand(SynchronizationContext synchronizationContext, IEnumerable<IMidiMessageSink> sinks, PulseAudioClient client)
        {
            _synchronizationContext = synchronizationContext;
            _sinks = sinks;
            _client = client;
        }
        // xmidictrl -v
        // xmidictrl -d -l
        // xmidictrl --device --list
        // xmidictrl --device --list
        // xmidictrl -c -l
        // xmidictrl --channel --list
        public override async Task<int> ExecuteAsync(CommandContext context, DeviceCommandSettings settings)
        {                        
            if(settings.List){
                Table table = new Table();
                table.AddColumns("Id", "Interface", "Name", "Input", "Output", "Opened");
                table.Border(TableBorder.Rounded);

                foreach (var item in PortMidi.MidiDeviceManager.AllDevices)
                {
                    table.AddRow(
                        item.ID.ToString(), 
                        item.Interface, 
                        item.Name, 
                        item.IsInput.ToString(), 
                        item.IsOutput.ToString(), 
                        item.IsOpened.ToString()
                    );
                }

                AnsiConsole.Write(table);                            
            }
            // if(settings.SetDefault){
            if(true) {

                await _client.ConnectAsync();
                
                var device = PortMidi.MidiDeviceManager.AllDevices.Last();                

                MidiDeviceListener listener = new MidiDeviceListener(device, _synchronizationContext, _sinks);                
                
                Console.WriteLine($"Listening to device: {device.Name}");
                var listenerTask = listener.StartAsync().ConfigureAwait(false);                
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
}

