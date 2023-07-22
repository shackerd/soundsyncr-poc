using Midicontrol.Midi.NativeSinks.VolumeControl;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal interface IPulseAudioStream : IAudioStream
    {
        ObjectPath ObjectPath { get; }
    }
}