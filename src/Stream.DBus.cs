using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tmds.DBus;

[assembly: InternalsVisibleTo(Tmds.DBus.Connection.DynamicAssemblyName)]
namespace Core1.DBus
{
    [DBusInterface("org.PulseAudio.Core1.Stream")]
    public interface IStream : IDBusObject
    {
        Task MoveAsync(ObjectPath Device);
        Task KillAsync();
        Task<IDisposable> WatchDeviceUpdatedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchSampleRateUpdatedAsync(Action<uint> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchVolumeUpdatedAsync(Action<uint[]> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchMuteUpdatedAsync(Action<bool> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchPropertyListUpdatedAsync(Action<IDictionary<string, byte[]>> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchStreamEventAsync(Action<(string name, IDictionary<string, byte[]> propertyList)> handler, Action<Exception> onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<StreamProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

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

    static class StreamExtensions
    {
        public static Task<uint> GetIndexAsync(this IStream o) => o.GetAsync<uint>("Index");
        public static Task<string> GetDriverAsync(this IStream o) => o.GetAsync<string>("Driver");
        public static Task<ObjectPath> GetOwnerModuleAsync(this IStream o) => o.GetAsync<ObjectPath>("OwnerModule");
        public static Task<ObjectPath> GetClientAsync(this IStream o) => o.GetAsync<ObjectPath>("Client");
        public static Task<ObjectPath> GetDeviceAsync(this IStream o) => o.GetAsync<ObjectPath>("Device");
        public static Task<uint> GetSampleFormatAsync(this IStream o) => o.GetAsync<uint>("SampleFormat");
        public static Task<uint> GetSampleRateAsync(this IStream o) => o.GetAsync<uint>("SampleRate");
        public static Task<uint[]> GetChannelsAsync(this IStream o) => o.GetAsync<uint[]>("Channels");
        public static Task<uint[]> GetVolumeAsync(this IStream o) => o.GetAsync<uint[]>("Volume");
        public static Task<bool> GetMuteAsync(this IStream o) => o.GetAsync<bool>("Mute");
        public static Task<ulong> GetBufferLatencyAsync(this IStream o) => o.GetAsync<ulong>("BufferLatency");
        public static Task<ulong> GetDeviceLatencyAsync(this IStream o) => o.GetAsync<ulong>("DeviceLatency");
        public static Task<string> GetResampleMethodAsync(this IStream o) => o.GetAsync<string>("ResampleMethod");
        public static Task<IDictionary<string, byte[]>> GetPropertyListAsync(this IStream o) => o.GetAsync<IDictionary<string, byte[]>>("PropertyList");
        public static Task SetVolumeAsync(this IStream o, uint[] val) => o.SetAsync("Volume", val);
        public static Task SetMuteAsync(this IStream o, bool val) => o.SetAsync("Mute", val);
    }
}