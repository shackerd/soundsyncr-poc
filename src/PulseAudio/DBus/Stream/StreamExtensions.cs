using Tmds.DBus;

namespace Midicontrol.PulseAudio.DBus
{       
    static class StreamExtensions
    {
        public static Task<uint> GetIndexAsync(this IStreamProxy o) => o.GetAsync<uint>("Index");
        public static Task<string> GetDriverAsync(this IStreamProxy o) => o.GetAsync<string>("Driver");
        public static Task<ObjectPath> GetOwnerModuleAsync(this IStreamProxy o) => o.GetAsync<ObjectPath>("OwnerModule");
        public static Task<ObjectPath> GetClientAsync(this IStreamProxy o) => o.GetAsync<ObjectPath>("Client");
        public static Task<ObjectPath> GetDeviceAsync(this IStreamProxy o) => o.GetAsync<ObjectPath>("Device");
        public static Task<uint> GetSampleFormatAsync(this IStreamProxy o) => o.GetAsync<uint>("SampleFormat");
        public static Task<uint> GetSampleRateAsync(this IStreamProxy o) => o.GetAsync<uint>("SampleRate");
        public static Task<uint[]> GetChannelsAsync(this IStreamProxy o) => o.GetAsync<uint[]>("Channels");
        public static Task<uint[]> GetVolumeAsync(this IStreamProxy o) => o.GetAsync<uint[]>("Volume");
        public static Task<bool> GetMuteAsync(this IStreamProxy o) => o.GetAsync<bool>("Mute");
        public static Task<ulong> GetBufferLatencyAsync(this IStreamProxy o) => o.GetAsync<ulong>("BufferLatency");
        public static Task<ulong> GetDeviceLatencyAsync(this IStreamProxy o) => o.GetAsync<ulong>("DeviceLatency");
        public static Task<string> GetResampleMethodAsync(this IStreamProxy o) => o.GetAsync<string>("ResampleMethod");
        public static Task<IDictionary<string, byte[]>> GetPropertyListAsync(this IStreamProxy o) => o.GetAsync<IDictionary<string, byte[]>>("PropertyList");
        public static Task SetVolumeAsync(this IStreamProxy o, uint[] val) => o.SetAsync("Volume", val);
        public static Task SetMuteAsync(this IStreamProxy o, bool val) => o.SetAsync("Mute", val);
    }
}