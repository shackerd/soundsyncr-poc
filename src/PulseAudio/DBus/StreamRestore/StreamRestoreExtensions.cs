using Tmds.DBus;

namespace Midicontrol.PulseAudio.DBus
{            
    static class StreamRestoreExtensions
    {
        public static Task<uint> GetInterfaceRevisionAsync(this IStreamRestoreProxy o) => o.GetAsync<uint>("InterfaceRevision");
        public static Task<ObjectPath[]> GetEntriesAsync(this IStreamRestoreProxy o) => o.GetAsync<ObjectPath[]>("Entries");
    }
}