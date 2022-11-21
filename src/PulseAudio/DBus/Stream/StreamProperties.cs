using Tmds.DBus;

namespace Midicontrol.PulseAudio.DBus
{
    [Dictionary]
    public class StreamProperties
    {
        private uint _Index = default(uint);
        public uint Index
        {
            get
            {
                return _Index;
            }

            set
            {
                _Index = (value);
            }
        }

        private string _Driver = default(string);
        public string Driver
        {
            get
            {
                return _Driver;
            }

            set
            {
                _Driver = (value);
            }
        }

        private ObjectPath _OwnerModule = default(ObjectPath);
        public ObjectPath OwnerModule
        {
            get
            {
                return _OwnerModule;
            }

            set
            {
                _OwnerModule = (value);
            }
        }

        private ObjectPath _Client = default(ObjectPath);
        public ObjectPath Client
        {
            get
            {
                return _Client;
            }

            set
            {
                _Client = (value);
            }
        }

        private ObjectPath _Device = default(ObjectPath);
        public ObjectPath Device
        {
            get
            {
                return _Device;
            }

            set
            {
                _Device = (value);
            }
        }

        private uint _SampleFormat = default(uint);
        public uint SampleFormat
        {
            get
            {
                return _SampleFormat;
            }

            set
            {
                _SampleFormat = (value);
            }
        }

        private uint _SampleRate = default(uint);
        public uint SampleRate
        {
            get
            {
                return _SampleRate;
            }

            set
            {
                _SampleRate = (value);
            }
        }

        private uint[] _Channels = default(uint[]);
        public uint[] Channels
        {
            get
            {
                return _Channels;
            }

            set
            {
                _Channels = (value);
            }
        }

        private uint[] _Volume = default(uint[]);
        public uint[] Volume
        {
            get
            {
                return _Volume;
            }

            set
            {
                _Volume = (value);
            }
        }

        private bool _Mute = default(bool);
        public bool Mute
        {
            get
            {
                return _Mute;
            }

            set
            {
                _Mute = (value);
            }
        }

        private ulong _BufferLatency = default(ulong);
        public ulong BufferLatency
        {
            get
            {
                return _BufferLatency;
            }

            set
            {
                _BufferLatency = (value);
            }
        }

        private ulong _DeviceLatency = default(ulong);
        public ulong DeviceLatency
        {
            get
            {
                return _DeviceLatency;
            }

            set
            {
                _DeviceLatency = (value);
            }
        }

        private string _ResampleMethod = default(string);
        public string ResampleMethod
        {
            get
            {
                return _ResampleMethod;
            }

            set
            {
                _ResampleMethod = (value);
            }
        }

        private IDictionary<string, byte[]> _PropertyList = default(IDictionary<string, byte[]>);
        public IDictionary<string, byte[]> PropertyList
        {
            get
            {
                return _PropertyList;
            }

            set
            {
                _PropertyList = (value);
            }
        }
    }
}