using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio.DBus
{
    [DBusInterface("org.PulseAudio.Ext.StreamRestore1")]
    interface IStreamRestoreProxy : IDBusObject
    {
        Task<ObjectPath> AddEntryAsync(string Name, string Device, (uint, uint)[] Volume, bool Mute, bool ApplyImmediately);
        Task<ObjectPath> GetEntryByNameAsync(string Name);
        Task<IDisposable> WatchNewEntryAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchEntryRemovedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<StreamRestoreProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }
}