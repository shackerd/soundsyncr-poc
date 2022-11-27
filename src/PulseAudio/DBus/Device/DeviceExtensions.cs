using Tmds.DBus;

namespace Midicontrol.PulseAudio.DBus
{
    static class DeviceExtensions
    {
        public static Task<uint> GetIndexAsync(this IDevice o) => o.GetAsync<uint>("Index");
        public static Task<string> GetNameAsync(this IDevice o) => o.GetAsync<string>("Name");
        public static Task<string> GetDriverAsync(this IDevice o) => o.GetAsync<string>("Driver");
        public static Task<ObjectPath> GetOwnerModuleAsync(this IDevice o) => o.GetAsync<ObjectPath>("OwnerModule");
        public static Task<ObjectPath> GetCardAsync(this IDevice o) => o.GetAsync<ObjectPath>("Card");
        public static Task<uint> GetSampleFormatAsync(this IDevice o) => o.GetAsync<uint>("SampleFormat");
        public static Task<uint> GetSampleRateAsync(this IDevice o) => o.GetAsync<uint>("SampleRate");
        public static Task<uint[]> GetChannelsAsync(this IDevice o) => o.GetAsync<uint[]>("Channels");
        public static Task<uint[]> GetVolumeAsync(this IDevice o) => o.GetAsync<uint[]>("Volume");
        public static Task<bool> GetHasFlatVolumeAsync(this IDevice o) => o.GetAsync<bool>("HasFlatVolume");
        public static Task<bool> GetHasConvertibleToDecibelVolumeAsync(this IDevice o) => o.GetAsync<bool>("HasConvertibleToDecibelVolume");
        public static Task<uint> GetBaseVolumeAsync(this IDevice o) => o.GetAsync<uint>("BaseVolume");
        public static Task<uint> GetVolumeStepsAsync(this IDevice o) => o.GetAsync<uint>("VolumeSteps");
        public static Task<bool> GetMuteAsync(this IDevice o) => o.GetAsync<bool>("Mute");
        public static Task<bool> GetHasHardwareVolumeAsync(this IDevice o) => o.GetAsync<bool>("HasHardwareVolume");
        public static Task<bool> GetHasHardwareMuteAsync(this IDevice o) => o.GetAsync<bool>("HasHardwareMute");
        public static Task<ulong> GetConfiguredLatencyAsync(this IDevice o) => o.GetAsync<ulong>("ConfiguredLatency");
        public static Task<bool> GetHasDynamicLatencyAsync(this IDevice o) => o.GetAsync<bool>("HasDynamicLatency");
        public static Task<ulong> GetLatencyAsync(this IDevice o) => o.GetAsync<ulong>("Latency");
        public static Task<bool> GetIsHardwareDeviceAsync(this IDevice o) => o.GetAsync<bool>("IsHardwareDevice");
        public static Task<bool> GetIsNetworkDeviceAsync(this IDevice o) => o.GetAsync<bool>("IsNetworkDevice");
        public static Task<uint> GetStateAsync(this IDevice o) => o.GetAsync<uint>("State");
        public static Task<ObjectPath[]> GetPortsAsync(this IDevice o) => o.GetAsync<ObjectPath[]>("Ports");
        public static Task<ObjectPath> GetActivePortAsync(this IDevice o) => o.GetAsync<ObjectPath>("ActivePort");
        public static Task<IDictionary<string, byte[]>> GetPropertyListAsync(this IDevice o) => o.GetAsync<IDictionary<string, byte[]>>("PropertyList");
        public static Task SetVolumeAsync(this IDevice o, uint[] val) => o.SetAsync("Volume", val);
        public static Task SetMuteAsync(this IDevice o, bool val) => o.SetAsync("Mute", val);
        public static Task SetActivePortAsync(this IDevice o, ObjectPath val) => o.SetAsync("ActivePort", val);
    }
}