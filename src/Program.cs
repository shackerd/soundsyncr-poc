﻿using System.ComponentModel;
using System.Text;
using Spectre.Console;
using Spectre.Console.Cli;
using Midicontrol.Midi;
using Midicontrol.PulseAudio;

namespace Midicontrol
{
    public class Program {
        public static void Main(string[] args)
        {                        
            var app = new CommandApp();
            app.Configure(config => {
                config
                    .AddCommand<DeviceCommand>("device")
                    .WithAlias("dev");
            });
            // app.Run(args);
            app.Run(new string[2]{"device", "--list"});
        }
    }

    internal class DeviceCommand : AsyncCommand<DeviceCommandSettings>
    {
        // xmidictrl -v
        // xmidictrl -d -l
        // xmidictrl --device --list
        // xmidictrl --device --list
        // xmidictrl -c -l
        // xmidictrl --channel --list
        public override async Task<int> ExecuteAsync(CommandContext context, DeviceCommandSettings settings)
        {
            PulseAudioClient client = new PulseAudioClient();
            await client.ConnectAsync();

            Dictionary<int, string> ctrl = 
                new Dictionary<int, string>();

            ctrl.Add(2, "chrome");
            ctrl.Add(1, "teams");
            
            
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

                MidiDeviceListener listener = new MidiDeviceListener(device);

                listener.OnMidiMessage += async (msg) => {
                    
                    uint value = PuseAudioMidiValueConverter.FromCCValueToVolume((uint)msg.Value);

                    Console.WriteLine($"0x{value.ToString("X2")}");

                    if(!client.PlaybackStreams.Any())
                    {
                        return;
                    }

                    if (ctrl.ContainsKey((int)msg.Controller))
                    {
                        string binary = ctrl[(int)msg.Controller];

                        var stream = 
                            client
                                .PlaybackStreams
                                .FirstOrDefault(_ => _.Binary.StartsWith(binary));
                        
                        if (stream != null)
                        {
                            await stream
                                .Proxy
                                .SetAsync("Volume", new uint[2] { value, value });    
                        }
                        
                    }                    
                };
                
                Console.WriteLine($"Listening to device: [underline green]{device.Name}[/]");
                Task listenerTask = listener.StartAsync();

                if((listenerTask.Status & TaskStatus.Faulted) == TaskStatus.Faulted)
                {
                    AnsiConsole.WriteException(listenerTask.Exception);
                }
                else {                    
                    Task.Run(() => AnsiConsole.Markup($"Listening to device: [underline green]{device.Name}[/]"));
                }
                
            
                Console.ReadLine();
            }
            // Omitted
            return 0;
        }        
    }    

    public static class PuseAudioMidiValueConverter
    {
        public static uint FromCCValueToVolume(uint value)
        {
            // MIDI MSB (Most Significant Byte) cc (ControlChange) max value is encoded on 7-bit -> byte.MaxValue >> 1 (0x7F) 

            if(value > 0x7F){
                throw new ArgumentOutOfRangeException("Value exceed 0x7F (127)");
            }

            uint paValue = (uint)(value * 0x205); // 0x204 * 0x7F  will not reach 0xFFFF
            
            paValue = Math.Clamp(paValue, 0x0, 0xFFFF); // 0xFFFF is volume max value, beyond it is amplified

            return paValue;
        }
    }

    internal class DeviceCommandSettings : CommandSettings
    {
        [Description("Enumerate available MIDI devices")]
        [CommandOption("-l|--list")]
        public bool List { get; set; }

        [CommandOption("-s|--set-default <ID>")]
        public bool SetDefault { get; set; }
    }

    internal class SetDefaultDeviceCommandSettings: CommandSettings 
    {

    }   
}

