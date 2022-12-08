using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Midicontrol.Infrastructure.Bindings;
using Midicontrol.Midi;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Midicontrol.CLI
{
    internal class StartCommand : AsyncCommand<StartCommandSettings>
    {
        private readonly IMidiDeviceListenerFactory _listenerFactory;
        private readonly ILogger<StartCommand> _logger;
        private readonly IMidiListenerStore _store;
        private readonly ConfigMap _config;

        public StartCommand(IMidiDeviceListenerFactory listenerFactory, ILogger<StartCommand> logger, IMidiListenerStore store, ConfigMap config)
        {
            _listenerFactory = listenerFactory;
            _logger = logger;
            _store = store;
            _config = config;
        }

        // xmidicontrol start
        // xmidicontrol list sinks
        // xmidicontrol list listener --map
        public override Task<int> ExecuteAsync(CommandContext context, StartCommandSettings settings)
        {
            List<Task> listenerTasks = new();

            if (_config.DevicesMap == null)
            {
                _logger.LogInformation($"No device map found, exiting...");
                return Task.FromResult(0);
            }

            foreach (var item in _config.DevicesMap)
            {
                PortMidi.MidiDeviceInfo device =
                    PortMidi
                        .MidiDeviceManager
                        .AllDevices
                        .FirstOrDefault(d => d.Name == item.DeviceName && !d.IsOutput);

                if(!device.IsInput){
                    // struct type, so we double check this property to know if device has been found
                    continue;
                }

                IMidiDeviceListener listener = _listenerFactory.Create(device);

                listenerTasks.Add(listener.StartAsync());

                // xmidicontrol list sinks
                IEnumerable<string> sinks = _store.GetListener(device.Name).Dispatcher.Sinks.Select(s => s.Name);


                foreach (var sink in sinks)
                {
                    _logger.LogInformation($"Enabling {sink} for {device.Name}");
                }
            }

            Console.ReadLine();

            // Omitted
            return Task.FromResult(0);
        }
    }
}