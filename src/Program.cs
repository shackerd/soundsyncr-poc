using Spectre.Console.Cli;
using Midicontrol.Midi;
using Midicontrol.PulseAudio;
using Microsoft.Extensions.DependencyInjection;
using Midicontrol.Infrastructure;
using Midicontrol.CLI;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Sinks.SpectreConsole;

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

            Log.Logger = new LoggerConfiguration()                       
                // .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
                .WriteTo.SpectreConsole("{Timestamp:HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}", minLevel: LogEventLevel.Debug)
                .Enrich.FromLogContext()                
#if DEBUG
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                .MinimumLevel.Debug()
#endif                    
                .CreateLogger();
            
            services.AddSingleton<SynchronizationContext>();
            services.AddSingleton<PulseAudioClient>();            
            services.AddTransient<IMidiMessageSink, PulseAudioMidiSink>();
            services.AddSingleton<IMidiDeviceListenerFactory, MidiDeviceListenerFactory>();            
            services.AddLogging(builder => builder.AddSerilog(dispose: false));
            

            return new TypeRegistrar(services);
        }
    }                   
}

