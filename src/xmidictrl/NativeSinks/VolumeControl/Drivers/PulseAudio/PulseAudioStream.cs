using System.Collections.ObjectModel;
using Midicontrol.Midi.NativeSinks.PulseAudio;
using Midicontrol.PulseAudio.DBus;
using Tmds.DBus;

namespace Midicontrol.PulseAudio
{
    public class PulseAudioStream
    {        
        private readonly ICoreProxy _coreProxy;
        private StreamProperties _properties;
        private const string _applicationProcessBinary = "application.process.binary";
        private const string _applicationProcessId = "application.process.id";
        private const string _applicationProcessName = "application.name";
        private const string _volumeProperty = "Volume";
        private readonly IReadOnlyDictionary<string, string> _readProperties;    
        
        public IStreamProxy Proxy { get; }        
        public string Binary => _readProperties[_applicationProcessBinary];
        public string Pid => _readProperties[_applicationProcessId];
        public string Name => _readProperties[_applicationProcessName];
        public PulseAudioStreamType Type { get; }
        public IReadOnlyDictionary<string, string> Properties => _readProperties;    
        public uint[] Volume => _properties.Volume;

        internal PulseAudioStream(
                IStreamProxy proxy,
                ICoreProxy coreProxy,
                StreamProperties properties,
                PulseAudioStreamType type,
                IReadOnlyDictionary<string, string> readProperties
        )
        {
            Proxy = proxy;            
            Type = type;
            _properties = properties;
            _readProperties = readProperties;
            _coreProxy = coreProxy;

            // Proxy.WatchDeviceUpdatedAsync()                        
            //Proxy.WatchMuteUpdatedAsync()            
        }

        private Task ListenToChangesAsync() {
            return ListenToChangesAsyncInternal();
        }

        private async Task ListenToChangesAsyncInternal() {
            
            await Proxy.WatchVolumeUpdatedAsync(
                (vol) => {
                    _properties.Volume = vol;                
                }
            );

            List<ObjectPath> array = new List<ObjectPath>();
            array.Add(Proxy.ObjectPath);

            await _coreProxy.ListenForSignalAsync("VolumeUpdated", array.ToArray());
            
        }

        public Task SetVolumeAsync(uint value) 
        {
            return this._coreProxy.DiscardSignalForAsync(
                async () => await SetVolumeAsyncInternal(value), 
                "VolumeUpdated", 
                TimeSpan.FromMilliseconds(2)
            );
        }

        private async Task SetVolumeAsyncInternal(uint value) {
            
            for (int i = 0; i < _properties.Volume.Length; i++)
            {
                Volume[i] = value;
            }

            await Proxy
                .SetAsync(_volumeProperty, _properties.Volume).ConfigureAwait(false);
        }

        public static async Task<PulseAudioStream> CreateAsync(IStreamProxy proxy, ICoreProxy coreProxy, PulseAudioStreamType type, IPulseAudioPropertyReader reader)
        {            
            StreamProperties props = await proxy.GetAllAsync();

            IReadOnlyDictionary<string, string> kvs = reader.ReadAll(props.PropertyList);
            PulseAudioStream stream = new PulseAudioStream(proxy, coreProxy, props, type, kvs);            

            await stream.ListenToChangesAsync().ConfigureAwait(false);

            return stream;
        }
    }

    public static class PulseAudioSignalHelperExtensions
    {
        private static async Task DiscardSignalForAsyncInternal(ICoreProxy proxy, Func<Task> func, string signal, TimeSpan? latencyConsideration = null)
        {
            await proxy.StopListeningForSignalAsync(signal);

            await func();
            
            if (latencyConsideration != null)
            {
                await Task.Delay(latencyConsideration.Value.Milliseconds);
            }            

            ObjectPath[] array = new List<ObjectPath>().ToArray();

            await proxy.ListenForSignalAsync(signal, array);
        }

        public static Task DiscardSignalForAsync(this ICoreProxy proxy, Func<Task> func, string signal, TimeSpan? latencyConsideration = null)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException(nameof(proxy));
            }

            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            if(string.IsNullOrEmpty(signal)) {
                throw new ArgumentNullException(nameof(signal));
            }

            return DiscardSignalForAsyncInternal(proxy, func, signal, latencyConsideration);
        }
    }    
}