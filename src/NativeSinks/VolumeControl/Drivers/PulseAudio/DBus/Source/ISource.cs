using Tmds.DBus;

namespace Midicontrol.PulseAudio.DBus
{
    [DBusInterface("org.PulseAudio.Core1.Source")]
    internal interface ISource : IDBusObject
    {
        Task<T> GetAsync<T>(string prop);
        Task<SourceProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }
}