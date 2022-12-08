using Microsoft.Extensions.DependencyInjection;
using Midicontrol.Midi.NativeSinks.MicrosoftAudio;
using Midicontrol.Midi.NativeSinks.PulseAudio;
using Midicontrol.Midi.NativeSinks.VolumeControl;

namespace Midicontrol.Midi.NativeSinks
{
    internal static class NativeSinksExtensions
    {
        public static IServiceCollection AddVolumeControlSink(this IServiceCollection services)
        {
            if(OperatingSystem.IsLinux()) {

                services.AddSingleton<Midi.NativeSinks.PulseAudio.PulseAudioStreamStore>();
                services.AddSingleton<Midi.NativeSinks.PulseAudio.PulseAudioWatchdogHandleStore>();
                services.AddSingleton<IPulseAudioConnection, PulseAudioConnection>();
                services.AddSingleton<IPulseAudioWatchdog, PulseAudioWatchdog>();
                services.AddSingleton<IPulseAudioStreamLoader, PulseAudioStreamLoader>();
                services.AddSingleton<IAudioDriver, PulseAudioDriver>();
            }

            if(OperatingSystem.IsWindows()) {
                services.AddSingleton<IAudioDriver, MicrosoftAudioDriver>();
            }

            services.AddSingleton<IMidiMessageSink, VolumeControlSink>();

            return services;
        }
    }
}