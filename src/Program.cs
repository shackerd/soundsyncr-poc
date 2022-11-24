using Spectre.Console.Cli;
using Midicontrol.Midi;
using Midicontrol.PulseAudio;
using Microsoft.Extensions.DependencyInjection;
using Midicontrol.Infrastructure;
using Midicontrol.CLI;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SpectreConsole;
using Spectre.Console;
using Midicontrol.Infrastructure.Bindings;

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
            
            SayHello();
            
            // app.Run(args);
            app.Run(new string[2]{"device", "--list"});            
        }

        private static ITypeRegistrar Setup(){

            IServiceCollection services = new ServiceCollection();

            Log.Logger = new LoggerConfiguration()                       
                .WriteTo.SpectreConsole("{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}", minLevel: LogEventLevel.Debug)
                .Enrich.FromLogContext()                
#if DEBUG
                .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                .MinimumLevel.Debug()
#endif                    
                .CreateLogger();
            
            services.AddSingleton<SynchronizationContext>();

            services.AddSingleton<PulseAudioClient>();         
            services.AddSingleton<MidiBindingMap>((_) => CreateBindingMap());

            services.AddTransient<IMidiMessageSink, PulseAudioMidiSink>();
            services.AddTransient<IMidiMessageSink, DebugMidiMessageSink>();
            services.AddSingleton<IMidiListenerStore, MidiListenerStore>();
            services.AddSingleton<IMidiDeviceListenerFactory, MidiDeviceListenerFactory>();
            services.AddLogging(builder => builder.AddSerilog(dispose: false));
                            
            return new TypeRegistrar(services);
        }

        private static MidiBindingMap CreateBindingMap() 
        {
            var map = new MidiBindingMap() { DeviceMaps = new List<MidiDeviceMap>() };
            map.DeviceMaps.Add(
                new MidiDeviceMap() {
                    DeviceName = "nanoKONTROL2 nanoKONTROL2 _ CTR",
                    Sinks = new List<MidiSinkMap>() {
                        new MidiSinkMap() {
                            Bindings = new List<MidiBinding>() {
                                new MidiBinding() {
                                    Controller = 1,
                                    Params = new List<MidiSinkParam>() {
                                        new MidiSinkParam() {
                                            Action = "volume",
                                            Destination = "teams"
                                        }
                                    }
                                },
                                new MidiBinding() {
                                    Controller = 2,
                                    Params = new List<MidiSinkParam>() {
                                        new MidiSinkParam() {
                                            Action = "volume",
                                            Destination = "chrome"
                                        }
                                    }
                                }
                            },
                            Sink = "Pulse Audio"
                        }
                    }

                }
            );
            return map;
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

