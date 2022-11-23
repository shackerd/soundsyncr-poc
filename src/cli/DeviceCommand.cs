using Midicontrol.Midi;
using Midicontrol.PulseAudio;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Midicontrol.CLI
{
    internal class DeviceCommand : AsyncCommand<DeviceCommandSettings>
    {        
        private readonly PulseAudioClient _client;
        private readonly IMidiDeviceListenerFactory _listenerFactory;

        public DeviceCommand(PulseAudioClient client, IMidiDeviceListenerFactory listenerFactory)
        {
            
            _client = client;
            _listenerFactory = listenerFactory;
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

                IMidiDeviceListener listener = _listenerFactory.Create(device);
                                
                var listenerTask = listener.StartAsync().ConfigureAwait(false);                
                Console.ReadLine();
            }
            // Omitted
            return 0;
        }        
    }  
}