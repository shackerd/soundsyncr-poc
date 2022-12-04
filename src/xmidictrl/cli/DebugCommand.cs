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

        public override Task<int> ExecuteAsync(CommandContext context)
        {
            PortMidi.MidiDeviceInfo device = AnsiConsole.Prompt(
                new SelectionPrompt<PortMidi.MidiDeviceInfo>()
                    .Title("Select [green]device[/]")
                    .UseConverter((d) => d.Name)
                    .AddChoices(
                        PortMidi
                            .MidiDeviceManager
                            .AllDevices.Where(d => d.IsInput)
                    ));

            IMidiDeviceListener listener = _listenerFactory.CreateDebug(device);
            Task listenerTask = listener.StartAsync();

            Console.ReadLine();

            return Task.FromResult(0);
        }
    }
}