using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio.DBus
{
    [DBusInterface("org.PulseAudio.ServerLookup1")]
    interface IServerLookupProxy : IDBusObject
    {
        Task<T> GetAsync<T>(string prop);
        Task<ServerLookupProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }
}