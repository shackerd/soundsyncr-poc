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
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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
            DeserializerBuilder builder = new DeserializerBuilder();
            builder
                .WithNamingConvention(CamelCaseNamingConvention.Instance);

            var deserializer = builder.Build();
            var configMap = deserializer.Deserialize<ConfigMap>(File.ReadAllText("mapping.yml"));
            
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
            services.AddSingleton<ConfigMap>((_) => configMap);

            services.AddTransient<IMidiMessageSink, PulseAudioMidiSink>();
            services.AddTransient<IMidiMessageSink, DebugMidiMessageSink>();
            services.AddSingleton<IMidiListenerStore, MidiListenerStore>();
            services.AddSingleton<IMidiDeviceListenerFactory, MidiDeviceListenerFactory>();
            services.AddLogging(builder => builder.AddSerilog(dispose: false));
                            
            return new TypeRegistrar(services);
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

