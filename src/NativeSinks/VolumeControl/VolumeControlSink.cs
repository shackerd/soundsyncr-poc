using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;

namespace Midicontrol.Midi.NativeSinks
{
    internal class VolumeControlSink : IMidiMessageSink
    {
        private readonly ILogger _logger;

        public string Name => "VolumeControl";

        public VolumeControlSink(ILogger<VolumeControlSink> logger)
        {
            _logger = logger;
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public Task ProcessMessageAsync(IEnumerable<IMidiMessageSinkArgs> args)
        {
            throw new NotImplementedException();
        }
    }

    interface IMidiDevice {
        MidiDeviceCapability Capability { get; } // if both : TwoWay binding, else OneWay
    }

    internal enum MidiDeviceCapability
    {
        Write,
        Read
    }

    // MS / PA same contract interface
    interface IAudioDriver
    {
        Task InitializeAsync();
        Task<IEnumerable<IAudioStream>> GetStreams(string destination);
    }

    interface IAudioStream
    {
        Task<uint> GetVolumeAsync();
        Task SetVolumeAsync(uint value);
        Task ToggleMuteAsync(bool value);
        Task ToggleSoloAsync(bool value);
    }

    public class VolumeSinkAction
    {
        private readonly Uri _uri;
        private readonly ActionType _type;
        private readonly ActionEntityType _entityType;
        private readonly ActionStreamType _streamType;
        private readonly IReadOnlyDictionary<string, string> _additionalParams;
        public ActionType Type => _type;
        public ActionEntityType Entity => _entityType;
        public ActionStreamType Stream => _streamType;
        public IReadOnlyDictionary<string, string> AdditionalParams => _additionalParams;


        internal VolumeSinkAction(string action)
        {
            _uri = new Uri(action);
            _type = (ActionType)Enum.Parse(typeof(ActionType), _uri.Scheme, true);
            _entityType = (ActionEntityType)Enum.Parse(typeof(ActionEntityType), _uri.Host, true);
            _streamType = (ActionStreamType)Enum.Parse(typeof(ActionStreamType), _uri.AbsolutePath.TrimStart('/'), true);

            IEnumerable<KeyValuePair<string, string>> @params =
                _uri
                    .Query
                    .TrimStart('?')
                    .Split('&')
                    .Select(
                        p => new KeyValuePair<string, string>(
                            p.Split('=').First(),
                            p.Split('=').Last()
                        )
                    );

            _additionalParams =
                new ReadOnlyDictionary<string, string>(
                    new Dictionary<string, string>(@params)
                );
        }

        public static implicit operator VolumeSinkAction(string value)
        {
            return new VolumeSinkAction(value);
        }
    }

    public enum ActionType
    {
        Bind,
        Mute,
        Solo
    }
    public enum ActionEntityType
    {
        Device,
        Channel
    }
    public enum ActionStreamType
    {
        Playback,
        Record
    }
}