using Tmds.DBus;

namespace Midicontrol.PulseAudio
{
    public class PulseAudioStreamChangeEventArgs : EventArgs
    {
        private readonly string _objectPath;
        private readonly PulseAudioStreamType _streamType;
        private readonly PulseAudioStreamChangeType  _changeType;
        public string ObjectPath => _objectPath;
        public PulseAudioStreamType StreamType => _streamType;
        public PulseAudioStreamChangeType ChangeType => _changeType;

        public PulseAudioStreamChangeEventArgs(string objectPath, PulseAudioStreamType streamType, PulseAudioStreamChangeType changeType)
        {
            _objectPath = objectPath;
            _streamType = streamType;
            _changeType = changeType;
        }
    }   
}