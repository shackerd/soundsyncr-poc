using Tmds.DBus;

namespace Midicontrol.PulseAudio.DBus
{
    [Dictionary]
    internal class DeviceProperties
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

        private ObjectPath _Card = default(ObjectPath);
        public ObjectPath Card
        {
            get
            {
                return _Card;
            }

            set
            {
                _Card = (value);
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

        private bool _HasFlatVolume = default(bool);
        public bool HasFlatVolume
        {
            get
            {
                return _HasFlatVolume;
            }

            set
            {
                _HasFlatVolume = (value);
            }
        }

        private bool _HasConvertibleToDecibelVolume = default(bool);
        public bool HasConvertibleToDecibelVolume
        {
            get
            {
                return _HasConvertibleToDecibelVolume;
            }

            set
            {
                _HasConvertibleToDecibelVolume = (value);
            }
        }

        private uint _BaseVolume = default(uint);
        public uint BaseVolume
        {
            get
            {
                return _BaseVolume;
            }

            set
            {
                _BaseVolume = (value);
            }
        }

        private uint _VolumeSteps = default(uint);
        public uint VolumeSteps
        {
            get
            {
                return _VolumeSteps;
            }

            set
            {
                _VolumeSteps = (value);
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

        private bool _HasHardwareVolume = default(bool);
        public bool HasHardwareVolume
        {
            get
            {
                return _HasHardwareVolume;
            }

            set
            {
                _HasHardwareVolume = (value);
            }
        }

        private bool _HasHardwareMute = default(bool);
        public bool HasHardwareMute
        {
            get
            {
                return _HasHardwareMute;
            }

            set
            {
                _HasHardwareMute = (value);
            }
        }

        private ulong _ConfiguredLatency = default(ulong);
        public ulong ConfiguredLatency
        {
            get
            {
                return _ConfiguredLatency;
            }

            set
            {
                _ConfiguredLatency = (value);
            }
        }

        private bool _HasDynamicLatency = default(bool);
        public bool HasDynamicLatency
        {
            get
            {
                return _HasDynamicLatency;
            }

            set
            {
                _HasDynamicLatency = (value);
            }
        }

        private ulong _Latency = default(ulong);
        public ulong Latency
        {
            get
            {
                return _Latency;
            }

            set
            {
                _Latency = (value);
            }
        }

        private bool _IsHardwareDevice = default(bool);
        public bool IsHardwareDevice
        {
            get
            {
                return _IsHardwareDevice;
            }

            set
            {
                _IsHardwareDevice = (value);
            }
        }

        private bool _IsNetworkDevice = default(bool);
        public bool IsNetworkDevice
        {
            get
            {
                return _IsNetworkDevice;
            }

            set
            {
                _IsNetworkDevice = (value);
            }
        }

        private uint _State = default(uint);
        public uint State
        {
            get
            {
                return _State;
            }

            set
            {
                _State = (value);
            }
        }

        private ObjectPath[] _Ports = default(ObjectPath[]);
        public ObjectPath[] Ports
        {
            get
            {
                return _Ports;
            }

            set
            {
                _Ports = (value);
            }
        }

        private ObjectPath _ActivePort = default(ObjectPath);
        public ObjectPath ActivePort
        {
            get
            {
                return _ActivePort;
            }

            set
            {
                _ActivePort = (value);
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