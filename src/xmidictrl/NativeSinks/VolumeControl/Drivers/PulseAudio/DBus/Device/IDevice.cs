using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio.DBus
{
    [DBusInterface("org.PulseAudio.Core1.Device")]
    interface IDevice : IDBusObject
    {
        Task SuspendAsync(bool Suspend);
        Task<ObjectPath> GetPortByNameAsync(string Name);
        Task<IDisposable> WatchVolumeUpdatedAsync(Action<uint[]> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchMuteUpdatedAsync(Action<bool> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchStateUpdatedAsync(Action<uint> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchActivePortUpdatedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchPropertyListUpdatedAsync(Action<IDictionary<string, byte[]>> handler, Action<Exception>? onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<DeviceProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }
}