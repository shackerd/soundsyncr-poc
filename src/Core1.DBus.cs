using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tmds.DBus;

[assembly: InternalsVisibleTo(Tmds.DBus.Connection.DynamicAssemblyName)]
namespace Core1.DBus
{
    [DBusInterface("org.PulseAudio.Core1")]
    interface ICore1 : IDBusObject
    {
        Task<ObjectPath> GetCardByNameAsync(string Name);
        Task<ObjectPath> GetSinkByNameAsync(string Name);
        Task<ObjectPath> GetSourceByNameAsync(string Name);
        Task<ObjectPath> GetSampleByNameAsync(string Name);
        Task<ObjectPath> UploadSampleAsync(string Name, uint SampleFormat, uint SampleRate, uint[] Channels, uint[] DefaultVolume, IDictionary<string, byte[]> PropertyList, byte[] Data);
        Task<ObjectPath> LoadModuleAsync(string Name, IDictionary<string, string> Arguments);
        Task ExitAsync();
        Task ListenForSignalAsync(string Signal, ObjectPath[] Objects);
        Task StopListeningForSignalAsync(string Signal);
        Task<IDisposable> WatchNewCardAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchCardRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchNewSinkAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchSinkRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchFallbackSinkUpdatedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchFallbackSinkUnsetAsync(Action handler, Action<Exception> onError = null);
        Task<IDisposable> WatchNewSourceAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchSourceRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchFallbackSourceUpdatedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchFallbackSourceUnsetAsync(Action handler, Action<Exception> onError = null);
        Task<IDisposable> WatchNewPlaybackStreamAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchPlaybackStreamRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchNewRecordStreamAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchRecordStreamRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchNewSampleAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchSampleRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchNewModuleAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchModuleRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchNewClientAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchClientRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchNewExtensionAsync(Action<string> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchExtensionRemovedAsync(Action<string> handler, Action<Exception> onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<Core1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class Core1Properties
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

    static class Core1Extensions
    {
        public static Task<uint> GetInterfaceRevisionAsync(this ICore1 o) => o.GetAsync<uint>("InterfaceRevision");
        public static Task<string> GetNameAsync(this ICore1 o) => o.GetAsync<string>("Name");
        public static Task<string> GetVersionAsync(this ICore1 o) => o.GetAsync<string>("Version");
        public static Task<bool> GetIsLocalAsync(this ICore1 o) => o.GetAsync<bool>("IsLocal");
        public static Task<string> GetUsernameAsync(this ICore1 o) => o.GetAsync<string>("Username");
        public static Task<string> GetHostnameAsync(this ICore1 o) => o.GetAsync<string>("Hostname");
        public static Task<uint[]> GetDefaultChannelsAsync(this ICore1 o) => o.GetAsync<uint[]>("DefaultChannels");
        public static Task<uint> GetDefaultSampleFormatAsync(this ICore1 o) => o.GetAsync<uint>("DefaultSampleFormat");
        public static Task<uint> GetDefaultSampleRateAsync(this ICore1 o) => o.GetAsync<uint>("DefaultSampleRate");
        public static Task<uint> GetAlternateSampleRateAsync(this ICore1 o) => o.GetAsync<uint>("AlternateSampleRate");
        public static Task<ObjectPath[]> GetCardsAsync(this ICore1 o) => o.GetAsync<ObjectPath[]>("Cards");
        public static Task<ObjectPath[]> GetSinksAsync(this ICore1 o) => o.GetAsync<ObjectPath[]>("Sinks");
        public static Task<ObjectPath> GetFallbackSinkAsync(this ICore1 o) => o.GetAsync<ObjectPath>("FallbackSink");
        public static Task<ObjectPath[]> GetSourcesAsync(this ICore1 o) => o.GetAsync<ObjectPath[]>("Sources");
        public static Task<ObjectPath> GetFallbackSourceAsync(this ICore1 o) => o.GetAsync<ObjectPath>("FallbackSource");
        public static Task<ObjectPath[]> GetPlaybackStreamsAsync(this ICore1 o) => o.GetAsync<ObjectPath[]>("PlaybackStreams");
        public static Task<ObjectPath[]> GetRecordStreamsAsync(this ICore1 o) => o.GetAsync<ObjectPath[]>("RecordStreams");
        public static Task<ObjectPath[]> GetSamplesAsync(this ICore1 o) => o.GetAsync<ObjectPath[]>("Samples");
        public static Task<ObjectPath[]> GetModulesAsync(this ICore1 o) => o.GetAsync<ObjectPath[]>("Modules");
        public static Task<ObjectPath[]> GetClientsAsync(this ICore1 o) => o.GetAsync<ObjectPath[]>("Clients");
        public static Task<ObjectPath> GetMyClientAsync(this ICore1 o) => o.GetAsync<ObjectPath>("MyClient");
        public static Task<string[]> GetExtensionsAsync(this ICore1 o) => o.GetAsync<string[]>("Extensions");
        public static Task SetDefaultChannelsAsync(this ICore1 o, uint[] val) => o.SetAsync("DefaultChannels", val);
        public static Task SetDefaultSampleFormatAsync(this ICore1 o, uint val) => o.SetAsync("DefaultSampleFormat", val);
        public static Task SetDefaultSampleRateAsync(this ICore1 o, uint val) => o.SetAsync("DefaultSampleRate", val);
        public static Task SetAlternateSampleRateAsync(this ICore1 o, uint val) => o.SetAsync("AlternateSampleRate", val);
        public static Task SetFallbackSinkAsync(this ICore1 o, ObjectPath val) => o.SetAsync("FallbackSink", val);
        public static Task SetFallbackSourceAsync(this ICore1 o, ObjectPath val) => o.SetAsync("FallbackSource", val);
    }

    [DBusInterface("org.PulseAudio.Ext.StreamRestore1")]
    interface IStreamRestore1 : IDBusObject
    {
        Task<ObjectPath> AddEntryAsync(string Name, string Device, (uint, uint)[] Volume, bool Mute, bool ApplyImmediately);
        Task<ObjectPath> GetEntryByNameAsync(string Name);
        Task<IDisposable> WatchNewEntryAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchEntryRemovedAsync(Action<ObjectPath> handler, Action<Exception> onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<StreamRestore1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class StreamRestore1Properties
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

        private ObjectPath[] _Entries = default(ObjectPath[]);
        public ObjectPath[] Entries
        {
            get
            {
                return _Entries;
            }

            set
            {
                _Entries = (value);
            }
        }
    }

    static class StreamRestore1Extensions
    {
        public static Task<uint> GetInterfaceRevisionAsync(this IStreamRestore1 o) => o.GetAsync<uint>("InterfaceRevision");
        public static Task<ObjectPath[]> GetEntriesAsync(this IStreamRestore1 o) => o.GetAsync<ObjectPath[]>("Entries");
    }
}