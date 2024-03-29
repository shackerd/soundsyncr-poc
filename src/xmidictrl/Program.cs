﻿using Spectre.Console.Cli;
using Midicontrol.Midi;
using Microsoft.Extensions.DependencyInjection;
using Midicontrol.Infrastructure;
using Midicontrol.CLI;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SpectreConsole;
using Spectre.Console;
using Midicontrol.Infrastructure.Bindings;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Midicontrol.Midi.NativeSinks.PulseAudio;
using System.Runtime.CompilerServices;
using MediatR.Pipeline;
using MediatR;
using System.Reflection;
using Midicontrol.Midi.NativeSinks;
using Midicontrol.Midi.NativeSinks.VolumeControl;
using Midicontrol.Midi.NativeSinks.MicrosoftAudio;

[assembly: InternalsVisibleTo("xmidictrl.tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Midicontrol
{
    public class Program {
        public static void Main(string[] args)
        {
            // var device = PortMidi.MidiDeviceManager.AllDevices.First(d => d.Name == "nanoKONTROL2 nanoKONTROL2 _ CTR" && d.IsOutput);
            // var output = PortMidi.MidiDeviceManager.OpenOutput(device.ID);
            // output.Write(new PortMidi.MidiEvent() { Message = new PortMidi.MidiMessage(0, 127, 176) });
            SayHello();

            if(!TrySetup(out ITypeRegistrar? registrar )) return;

            CommandApp app = new CommandApp(registrar);

            app.Configure(config => {
                config
                    .AddCommand<DebugCommand>("debug");
                config
                    .AddCommand<ListCommand>("list")
                    .WithAlias("ls");
                config
                    .AddCommand<StartCommand>("start");

                config.Settings.ApplicationName = "xmidicontrol";
            });


            app.SetDefaultCommand<StartCommand>();
            // app.Run(new string[] { "list", "sinks"});
            app.Run(args);
        }

        private static bool TrySetup(out ITypeRegistrar? registrar){

            IServiceCollection services = new ServiceCollection();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.SpectreConsole("{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}", minLevel: LogEventLevel.Debug)
                .Enrich.FromLogContext()
#if DEBUG
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                .MinimumLevel.Debug()
#endif
                .CreateLogger();

            if (!TryLoadConfiguration(out ConfigMap? configMap))
            {
                registrar = null;
                return false;
            }

            services.AddSingleton<ConfigMap>((_) => configMap!);

            services.AddSingleton<SynchronizationContext>();

            services.AddVolumeControlSink();

            services.AddTransient<IMidiMessageSink, DebugMidiMessageSink>();
            services.AddSingleton<IMidiListenerStore, MidiListenerStore>();
            services.AddSingleton<IMidiDeviceListenerFactory, MidiDeviceListenerFactory>();
            services.AddLogging(builder => builder.AddSerilog(dispose: false));

            services.AddMediatR(Assembly.GetExecutingAssembly());

            registrar = new TypeRegistrar(services);

            return true;
        }

        private static bool TryLoadConfiguration(out ConfigMap? configMap) {

            IDeserializer deserializer =
                new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

            try
            {
                configMap = deserializer.Deserialize<ConfigMap>(File.ReadAllText("mapping.yml"));
                Log.Logger.Information("Loaded configuration");
                return true;
            }
            catch (System.Exception)
            {
                configMap = null;
                Log.Logger.Warning("Cannot load configuration file");
                return false;
            }
        }

        private static void SayHello()
        {
            using HttpClient client = new HttpClient();
            using HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri("http://www.figlet.org/fonts/nancyj.flf");

            FigletFont font = FigletFont.Load(client.Send(request).Content.ReadAsStream());
            AnsiConsole.Write(new FigletText(font, "xmCtrl").Color(Color.DodgerBlue1));
            AnsiConsole.Write(new Markup("[bold yellow]Crafted by[/] [blue]Shackerd:[/] https://github.com/shackerd"));
            AnsiConsole.WriteLine();
        }
    }
}
