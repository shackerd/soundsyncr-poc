using Midicontrol.Midi;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Midicontrol.CLI
{
    internal class DebugCommand : AsyncCommand
    {
        private readonly IMidiDeviceListenerFactory _listenerFactory;

        public DebugCommand(IMidiDeviceListenerFactory listenerFactory) 
        {
            _listenerFactory = listenerFactory;
        }

        public override async Task<int> ExecuteAsync(CommandContext context)
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