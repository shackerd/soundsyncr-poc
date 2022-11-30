using System.Collections.ObjectModel;

namespace Midicontrol.Midi.NativeSinks
{
    public class VolumeControlAction
    {
        private readonly Uri _uri;
        private readonly ActionType _type;
        private readonly Scope _scope;
        private readonly StreamType _streamType;
        private readonly IReadOnlyDictionary<string, string> _additionalParams;
        public ActionType ActionType => _type;
        public Scope Scope => _scope;
        public StreamType StreamType => _streamType;
        public IReadOnlyDictionary<string, string> AdditionalParams => _additionalParams;


        internal VolumeControlAction(string action)
        {
            _uri = new Uri(action);
            _type = (ActionType)Enum.Parse(typeof(ActionType), _uri.Scheme, true);
            _scope = (Scope)Enum.Parse(typeof(Scope), _uri.Host, true);
            _streamType = (StreamType)Enum.Parse(typeof(StreamType), _uri.AbsolutePath.TrimStart('/'), true);

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

        public static implicit operator VolumeControlAction(string value)
        {
            return new VolumeControlAction(value);
        }
    }
}