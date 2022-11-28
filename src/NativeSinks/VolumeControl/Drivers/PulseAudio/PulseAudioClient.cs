using Tmds.DBus;
using Midicontrol.PulseAudio.DBus;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Midicontrol.PulseAudio
{  
    public class PulseAudioClient
    {
        private const string _serverLookupServiceName = "org.PulseAudio1";
        private const string _serverLookupObjectPath = "/org/pulseaudio/server_lookup1";                
        private Connection _connection;

        private readonly SynchronizationContext _synCtx;
        private readonly ILogger _logger;
        private readonly ILogger<IPulseAudioStreamStore> _storeLogger;

        private IPulseAudioStreamStore _streamStore;
        private bool _initialized;

        public bool Initialized => _initialized;

        public IPulseAudioStreamStore StreamStore => _streamStore;

        public PulseAudioClient(SynchronizationContext synCtx, ILogger<PulseAudioClient> logger, ILogger<IPulseAudioStreamStore> storeLogger)
        {
            _synCtx = synCtx;
            _logger = logger;
            _storeLogger = storeLogger;
        }

        private async Task<string> ServerLookupAsync(){
            
            Connection sessionConnection = Connection.Session;

            IServerLookupProxy proxy = 
                sessionConnection.CreateProxy<IServerLookupProxy>(
                    _serverLookupServiceName, 
                    _serverLookupObjectPath
                );

            ServerLookupProperties res = await proxy.GetAllAsync().ConfigureAwait(false);                             
            
            return res.Address;
        }

        public Task ConnectAsync() {
            return Task.Factory.StartNew(() => ConnectAsyncInternal());
        }

        private async Task ConnectAsyncInternal(){
                        
            string address = await ServerLookupAsync().ConfigureAwait(false);            
            
            ClientConnectionOptions options = new ClientConnectionOptions(address);

            options.SynchronizationContext = _synCtx;
            
            _connection = new Connection(options);

            _connection.StateChanged += new EventHandler<ConnectionStateChangedEventArgs>(OnConnectionEvent);

            ConnectionInfo inf = await _connection.ConnectAsync().ConfigureAwait(false);

            _streamStore = new PulseAudioStreamStore(_connection, _storeLogger);

            await _streamStore.InitializeAsync().ConfigureAwait(false);

            _initialized = true;
        }        

        private void OnConnectionEvent(object _, ConnectionStateChangedEventArgs eventArgs) 
        {
            switch (eventArgs.State)
            {
                case ConnectionState.Disconnecting:
                case ConnectionState.Disconnected:
                    throw new Exception("Connection close");                    
                default:
                    _logger.LogInformation($"Pulse Audio : {eventArgs.State}");
                    break;
            }
        }        
    }        
}