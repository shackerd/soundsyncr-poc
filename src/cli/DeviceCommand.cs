using Microsoft.Extensions.Logging;
using Midicontrol.Midi;
using Midicontrol.PulseAudio;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Midicontrol.CLI
{
    internal class DeviceCommand : AsyncCommand<DeviceCommandSettings>
    {        
        private readonly IMidiDeviceListenerFactory _listenerFactory;
        private readonly ILogger<DeviceCommand> _logger;
        private readonly IMidiListenerStore _store;

        public DeviceCommand(IMidiDeviceListenerFactory listenerFactory, ILogger<DeviceCommand> logger, IMidiListenerStore store)
        {            
            _listenerFactory = listenerFactory;
            _logger = logger;
            _store = store;
        }
        
        // xmidicontrol start        
        // xmidicontrol list sinks
        // xmidicontrol list listener --map
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

                var device = PortMidi.MidiDeviceManager.AllDevices.Last();                

                IMidiDeviceListener listener = _listenerFactory.Create(device);
                                
                var listenerTask = listener.StartAsync().ConfigureAwait(false);                

                // xmidicontrol list sinks
                var sinks = _store.GetListener(device.Name).Sinks.Select(s => s.Name);

                var root = new Tree($"Sinks for {device.Name}");


                foreach (var sink in sinks)
                {
                    root.AddNode(sink);                                        
                }

                AnsiConsole.Write(root);

                Console.ReadLine();
            }
            // Omitted
            return 0;
        }        
    }  
}