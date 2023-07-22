using MediatR;
using Microsoft.Extensions.Logging;
using Midicontrol.Midi.NativeSinks.PulseAudio.DBus;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal interface IPulseAudioConnection
    {
        bool IsInitialized { get; }
        Connection? Connection { get; }
        Task ConnectAsync();
    }

    internal class PulseAudioConnection : IPulseAudioConnection
    {
        private readonly ILogger<IPulseAudioConnection> _logger;
        private readonly SynchronizationContext _synCtx;
        private readonly IMediator _mediator;
        private Connection? _connection;

        public Connection? Connection => _connection;

        public bool IsInitialized => _connection != null;

        public PulseAudioConnection(
            ILogger<IPulseAudioConnection> logger,
            SynchronizationContext synCtx,
            IMediator mediator
        )
        {
            _logger = logger;
            _synCtx = synCtx;
            _mediator = mediator;
        }

        private async Task<string?> ServerLookupAsync()
        {

            Connection sessionConnection = Connection.Session;

            IServerLookupProxy proxy =
                sessionConnection.CreateProxy<IServerLookupProxy>(
                    PulseAudioDBus.ServerLookupServiceName,
                    PulseAudioDBus.ServerLookupObjectPath
                );

            ServerLookupProperties res = await proxy.GetAllAsync().ConfigureAwait(false);

            return res?.Address;
        }

        public Task ConnectAsync()
        {
            return ConnectAsyncInternal();
        }

        private async Task ConnectAsyncInternal()
        {

            string? address = await ServerLookupAsync().ConfigureAwait(false);

            if (string.IsNullOrEmpty(address))
            {
                throw new InvalidOperationException("Cannot resolve PulseAudio DBus");
            }

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
    }
}