using Tmds.DBus;

namespace Midicontrol.PulseAudio.DBus
{
    [Dictionary]
    public class CoreProperties
    {
        private uint _InterfaceRevision = default(uint);
        public uint InterfaceRevision
        {
            get
            {
                return _InterfaceRevision;
            }

            set
            {
                _InterfaceRevision = (value);
            }
        }

        private string _Name = default(string);
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = (value);
            }
        }

        private string _Version = default(string);
        public string Version
        {
            get
            {
                return _Version;
            }

            set
            {
                _Version = (value);
            }
        }

        private bool _IsLocal = default(bool);
        public bool IsLocal
        {
            get
            {
                return _IsLocal;
            }

            set
            {
                _IsLocal = (value);
            }
        }

        private string _Username = default(string);
        public string Username
        {
            get
            {
                return _Username;
            }

            set
            {
                _Username = (value);
            }
        }

        private string _Hostname = default(string);
        public string Hostname
        {
            get
            {
                return _Hostname;
            }

            set
            {
                _Hostname = (value);
            }
        }

        private uint[] _DefaultChannels = default(uint[]);
        public uint[] DefaultChannels
        {
            get
            {
                return _DefaultChannels;
            }

            set
            {
                _DefaultChannels = (value);
            }
        }

        private uint _DefaultSampleFormat = default(uint);
        public uint DefaultSampleFormat
        {
            get
            {
                return _DefaultSampleFormat;
            }

            set
            {
                _DefaultSampleFormat = (value);
            }
        }

        private uint _DefaultSampleRate = default(uint);
        public uint DefaultSampleRate
        {
            get
            {
                return _DefaultSampleRate;
            }

            set
            {
                _DefaultSampleRate = (value);
            }
        }

        private uint _AlternateSampleRate = default(uint);
        public uint AlternateSampleRate
        {
            get
            {
                return _AlternateSampleRate;
            }

            set
            {
                _AlternateSampleRate = (value);
            }
        }

        private ObjectPath[] _Cards = default(ObjectPath[]);
        public ObjectPath[] Cards
        {
            get
            {
                return _Cards;
            }

            set
            {
                _Cards = (value);
            }
        }

        private ObjectPath[] _Sinks = default(ObjectPath[]);
        public ObjectPath[] Sinks
        {
            get
            {
                return _Sinks;
            }

            set
            {
                _Sinks = (value);
            }
        }

        private ObjectPath _FallbackSink = default(ObjectPath);
        public ObjectPath FallbackSink
        {
            get
            {
                return _FallbackSink;
            }

            set
            {
                _FallbackSink = (value);
            }
        }

        private ObjectPath[] _Sources = default(ObjectPath[]);
        public ObjectPath[] Sources
        {
            get
            {
                return _Sources;
            }

            set
            {
                _Sources = (value);
            }
        }

        private ObjectPath _FallbackSource = default(ObjectPath);
        public ObjectPath FallbackSource
        {
            get
            {
                return _FallbackSource;
            }

            set
            {
                _FallbackSource = (value);
            }
        }

        private ObjectPath[] _PlaybackStreams = default(ObjectPath[]);
        public ObjectPath[] PlaybackStreams
        {
            get
            {
                return _PlaybackStreams;
            }

            set
            {
                _PlaybackStreams = (value);
            }
        }

        private ObjectPath[] _RecordStreams = default(ObjectPath[]);
        public ObjectPath[] RecordStreams
        {
            get
            {
                return _RecordStreams;
            }

            set
            {
                _RecordStreams = (value);
            }
        }

        private ObjectPath[] _Samples = default(ObjectPath[]);
        public ObjectPath[] Samples
        {
            get
            {
                return _Samples;
            }

            set
            {
                _Samples = (value);
            }
        }

        private ObjectPath[] _Modules = default(ObjectPath[]);
        public ObjectPath[] Modules
        {
            get
            {
                return _Modules;
            }

            set
            {
                _Modules = (value);
            }
        }

        private ObjectPath[] _Clients = default(ObjectPath[]);
        public ObjectPath[] Clients
        {
            get
            {
                return _Clients;
            }

            set
            {
                _Clients = (value);
            }
        }

        private ObjectPath _MyClient = default(ObjectPath);
        public ObjectPath MyClient
        {
            get
            {
                return _MyClient;
            }

            set
            {
                _MyClient = (value);
            }
        }

        private string[] _Extensions = default(string[]);
        public string[] Extensions
        {
            get
            {
                return _Extensions;
            }

            set
            {
                _Extensions = (value);
            }
        }
    }

}