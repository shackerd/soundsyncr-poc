using MediatR;
using Microsoft.Extensions.Logging;
using Midicontrol.PulseAudio.DBus;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal sealed class PulseAudioDriver : IAudioDriver
    {
        private readonly ILogger<PulseAudioDriver> _logger;
        private readonly SynchronizationContext _synCtx;
        private readonly IMediator _mediator;
        private Connection? _connection;

        public PulseAudioDriver(
            ILogger<PulseAudioDriver> logger,
            SynchronizationContext synCtx,
            IMediator mediator
        )
        {
            _logger = logger;
            _synCtx = synCtx;
            _mediator = mediator;
        }
        public Task InitializeAsync()
        {
            return ConnectAsync();
        }

        private async Task<string> ServerLookupAsync(){

            Connection sessionConnection = Connection.Session;

            IServerLookupProxy proxy =
                sessionConnection.CreateProxy<IServerLookupProxy>(
                    PulseAudioDBus.ServerLookupServiceName,
                    PulseAudioDBus.ServerLookupObjectPath
                );

            ServerLookupProperties res = await proxy.GetAllAsync().ConfigureAwait(false);

            return res.Address;
        }

        private async Task ConnectAsync(){

            string address = await ServerLookupAsync().ConfigureAwait(false);

            ClientConnectionOptions options = new ClientConnectionOptions(address);

            options.SynchronizationContext = _synCtx;

            _connection = new Connection(options);

            _connection.StateChanged += new EventHandler<ConnectionStateChangedEventArgs>(OnConnectionEvent);

            ConnectionInfo inf = await _connection.ConnectAsync().ConfigureAwait(false);
        }

        private async void OnConnectionEvent(object? _, ConnectionStateChangedEventArgs eventArgs)
        {
            _logger.LogInformation($"Pulse Audio : {eventArgs.State}");

            switch (eventArgs.State)
            {
                case ConnectionState.Created:
                case ConnectionState.Connecting:
                case ConnectionState.Disconnecting:
                    break;
                default:
                    await _mediator
                        .Publish(
                            new PulseAudioConnectionNotification(
                                eventArgs.State == ConnectionState.Connected,
                                _connection
                            )
                        )
                        .ConfigureAwait(false);
                    break;
            }
        }

        public Task<IEnumerable<IAudioStream>> GetStreamsAsync(string destination)
        {
            throw new NotImplementedException();
        }

        public Task ToggleSoloAsync(IAudioStream stream, bool solo)
        {
            throw new NotImplementedException();
        }
    }
}