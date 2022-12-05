using System.Runtime.CompilerServices;
using Tmds.DBus;

[assembly: InternalsVisibleTo(Tmds.DBus.Connection.DynamicAssemblyName)]

namespace Midicontrol.Midi.NativeSinks.PulseAudio.DBus
{
    [DBusInterface("org.PulseAudio.Core1")]
    public interface ICoreProxy : IDBusObject
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
        Task<IDisposable> WatchNewCardAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchCardRemovedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchNewSinkAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchSinkRemovedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchFallbackSinkUpdatedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchFallbackSinkUnsetAsync(Action handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchNewSourceAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchSourceRemovedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchFallbackSourceUpdatedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchFallbackSourceUnsetAsync(Action handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchNewPlaybackStreamAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchPlaybackStreamRemovedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchNewRecordStreamAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchRecordStreamRemovedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchNewSampleAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchSampleRemovedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchNewModuleAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchModuleRemovedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchNewClientAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchClientRemovedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchNewExtensionAsync(Action<string> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchExtensionRemovedAsync(Action<string> handler, Action<Exception>? onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<CoreProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }
}