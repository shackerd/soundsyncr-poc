using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio.DBus
{
    [DBusInterface("org.PulseAudio.Core1.Stream")]
    public interface IStreamProxy : IDBusObject
    {
        Task MoveAsync(ObjectPath Device);
        Task KillAsync();
        Task<IDisposable> WatchDeviceUpdatedAsync(Action<ObjectPath> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchSampleRateUpdatedAsync(Action<uint> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchVolumeUpdatedAsync(Action<uint[]> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchMuteUpdatedAsync(Action<bool> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchPropertyListUpdatedAsync(Action<IDictionary<string, byte[]>> handler, Action<Exception>? onError = null);
        Task<IDisposable> WatchStreamEventAsync(Action<(string name, IDictionary<string, byte[]> propertyList)> handler, Action<Exception>? onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<StreamProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }
}