using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio.DBus
{
    static class CoreExtensions
    {
        public static Task<uint> GetInterfaceRevisionAsync(this ICoreProxy o) => o.GetAsync<uint>("InterfaceRevision");
        public static Task<string> GetNameAsync(this ICoreProxy o) => o.GetAsync<string>("Name");
        public static Task<string> GetVersionAsync(this ICoreProxy o) => o.GetAsync<string>("Version");
        public static Task<bool> GetIsLocalAsync(this ICoreProxy o) => o.GetAsync<bool>("IsLocal");
        public static Task<string> GetUsernameAsync(this ICoreProxy o) => o.GetAsync<string>("Username");
        public static Task<string> GetHostnameAsync(this ICoreProxy o) => o.GetAsync<string>("Hostname");
        public static Task<uint[]> GetDefaultChannelsAsync(this ICoreProxy o) => o.GetAsync<uint[]>("DefaultChannels");
        public static Task<uint> GetDefaultSampleFormatAsync(this ICoreProxy o) => o.GetAsync<uint>("DefaultSampleFormat");
        public static Task<uint> GetDefaultSampleRateAsync(this ICoreProxy o) => o.GetAsync<uint>("DefaultSampleRate");
        public static Task<uint> GetAlternateSampleRateAsync(this ICoreProxy o) => o.GetAsync<uint>("AlternateSampleRate");
        public static Task<ObjectPath[]> GetCardsAsync(this ICoreProxy o) => o.GetAsync<ObjectPath[]>("Cards");
        public static Task<ObjectPath[]> GetSinksAsync(this ICoreProxy o) => o.GetAsync<ObjectPath[]>("Sinks");
        public static Task<ObjectPath> GetFallbackSinkAsync(this ICoreProxy o) => o.GetAsync<ObjectPath>("FallbackSink");
        public static Task<ObjectPath[]> GetSourcesAsync(this ICoreProxy o) => o.GetAsync<ObjectPath[]>("Sources");
        public static Task<ObjectPath> GetFallbackSourceAsync(this ICoreProxy o) => o.GetAsync<ObjectPath>("FallbackSource");
        public static Task<ObjectPath[]> GetPlaybackStreamsAsync(this ICoreProxy o) => o.GetAsync<ObjectPath[]>("PlaybackStreams");
        public static Task<ObjectPath[]> GetRecordStreamsAsync(this ICoreProxy o) => o.GetAsync<ObjectPath[]>("RecordStreams");
        public static Task<ObjectPath[]> GetSamplesAsync(this ICoreProxy o) => o.GetAsync<ObjectPath[]>("Samples");
        public static Task<ObjectPath[]> GetModulesAsync(this ICoreProxy o) => o.GetAsync<ObjectPath[]>("Modules");
        public static Task<ObjectPath[]> GetClientsAsync(this ICoreProxy o) => o.GetAsync<ObjectPath[]>("Clients");
        public static Task<ObjectPath> GetMyClientAsync(this ICoreProxy o) => o.GetAsync<ObjectPath>("MyClient");
        public static Task<string[]> GetExtensionsAsync(this ICoreProxy o) => o.GetAsync<string[]>("Extensions");
        public static Task SetDefaultChannelsAsync(this ICoreProxy o, uint[] val) => o.SetAsync("DefaultChannels", val);
        public static Task SetDefaultSampleFormatAsync(this ICoreProxy o, uint val) => o.SetAsync("DefaultSampleFormat", val);
        public static Task SetDefaultSampleRateAsync(this ICoreProxy o, uint val) => o.SetAsync("DefaultSampleRate", val);
        public static Task SetAlternateSampleRateAsync(this ICoreProxy o, uint val) => o.SetAsync("AlternateSampleRate", val);
        public static Task SetFallbackSinkAsync(this ICoreProxy o, ObjectPath val) => o.SetAsync("FallbackSink", val);
        public static Task SetFallbackSourceAsync(this ICoreProxy o, ObjectPath val) => o.SetAsync("FallbackSource", val);
    }
}