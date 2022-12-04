using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal interface IPulseAudioStream : IAudioStream
    {
        ObjectPath ObjectPath { get; }
    }
}