using Microsoft.Extensions.Logging;
using Midicontrol.Infrastructure.Bindings;
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
        private readonly ConfigMap _config;

        public DeviceCommand(IMidiDeviceListenerFactory listenerFactory, ILogger<DeviceCommand> logger, IMidiListenerStore store, ConfigMap config)
        {            
            _listenerFactory = listenerFactory;
            _logger = logger;
            _store = store;
            _config = config;
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

                foreach (var item in _config.DevicesMap)
                {
                    var device = 
                        PortMidi
                            .MidiDeviceManager
                            .AllDevices
                            .FirstOrDefault(d => d.Name == item.DeviceName && !d.IsOutput);

                    if(!device.IsInput){
                        // struct type, so we double check this property to know if device has been found
                        continue;
                    }

                    IMidiDeviceListener listener = _listenerFactory.Create(device);
                                    
                    var listenerTask = listener.StartAsync().ConfigureAwait(false);                

                    // xmidicontrol list sinks
                    var sinks = _store.GetListener(device.Name).Dispatcher.Sinks.Select(s => s.Name);

                    // var root = new Tree($"Sinks for {device.Name}");


                    foreach (var sink in sinks)
                    {
                        _logger.LogInformation($"Enabling {sink} for {device.Name}");
                        // root.AddNode(sink);                                        
                    }
                    
                    // AnsiConsole.Write(root);
                }                

                Console.ReadLine();
            }
            // Omitted
            return 0;
        }        
    }

    internal class DebugCommand : AsyncCommand<DeviceCommandSettings>
    {
        private readonly IMidiDeviceListenerFactory _listenerFactory;

        public DebugCommand(IMidiDeviceListenerFactory listenerFactory) 
        {
            _listenerFactory = listenerFactory;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, DeviceCommandSettings settings)
        {
                        
            string deviceName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select [green]device[/]")                                        
                    .AddChoices(
                        PortMidi
                            .MidiDeviceManager
                            .AllDevices.Where(d => d.IsInput).Select(d => d.Name)
                    ));

            var device = PortMidi.MidiDeviceManager.AllDevices.FirstOrDefault(d => d.Name.Equals(deviceName) && d.IsInput);
            IMidiDeviceListener listener = _listenerFactory.CreateDebug(device);
            var listenerTask = listener.StartAsync().ConfigureAwait(false);                

            Console.ReadLine();

            return 0;
        }
    }
}