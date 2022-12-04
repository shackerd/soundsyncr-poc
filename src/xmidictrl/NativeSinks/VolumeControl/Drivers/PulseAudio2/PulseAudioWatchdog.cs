using MediatR;
using Microsoft.Extensions.Logging;
using Midicontrol.PulseAudio.DBus;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal class PulseAudioWatchdog : IDisposable
    {
        private IDisposable? _newPlaybackStreamSubscription;
        private IDisposable? _newRecordStreamSubscription;
        private IDisposable? _playbackStreamRemovedSubscription;
        private IDisposable? _recordStreamRemovedSubscription;
        private bool disposedValue;
        private ICoreProxy? _proxy;
        private readonly IPulseAudioConnection _connection;
        private readonly IMediator _mediator;
        private readonly ILogger<PulseAudioWatchdog> _logger;

        public PulseAudioWatchdog(IPulseAudioConnection connection, IMediator mediator, ILogger<PulseAudioWatchdog> logger)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            _connection = connection;
            _mediator = mediator;
            _logger = logger;
        }

        public Task InitializeAsync()
        {
            _proxy = _connection?.Connection?.CreateProxy<ICoreProxy>(PulseAudioDBus.CoreSeviceName, PulseAudioDBus.CoreObjectPath);

            if(_proxy == null) {
                throw new InvalidOperationException("Cannot create proxy");
            }

            return InitializeAsyncInternal();
        }

        private async Task InitializeAsyncInternal()
        {
            await ListenToStreamsChangesAsync().ConfigureAwait(false);
        }

        private async Task ListenToStreamsChangesAsync()
        {
            _newPlaybackStreamSubscription = await _proxy.WatchNewPlaybackStreamAsync(
                async (path) =>
                {
                    IPulseAudioStream stream =
                        await _mediator.Send<IPulseAudioStream>(
                            new PulseAudioStreamLoadRequest(path, Scope.Channel, StreamType.Playback)
                        );

                    await _mediator.Publish(
                        new PulseAudioStreamChangeNotification(
                            stream,
                            PulseAudioStreamChangeNotificationType.Created
                        )
                    );
                },
                (ex) => { Console.WriteLine(ex.ToString()); }
            ).ConfigureAwait(false);

            _newRecordStreamSubscription = await _proxy.WatchNewRecordStreamAsync(
                async (path) =>
                {
                    IPulseAudioStream stream =
                        await _mediator.Send<IPulseAudioStream>(
                            new PulseAudioStreamLoadRequest(path, Scope.Channel, StreamType.Record)
                        );

                    await _mediator.Publish(
                        new PulseAudioStreamChangeNotification(
                            stream,
                            PulseAudioStreamChangeNotificationType.Created
                        )
                    );
                },
                (ex) =>
                {
                    Console.WriteLine(ex.ToString());
                }
            ).ConfigureAwait(false);

            _playbackStreamRemovedSubscription = await _proxy.WatchPlaybackStreamRemovedAsync(
                async (path) =>
                {
                    IPulseAudioStream stream =
                        await _mediator.Send<IPulseAudioStream>(
                            new PulseAudioStreamGetRequest(path, Scope.Channel, StreamType.Playback)
                        );

                    await _mediator.Publish(
                        new PulseAudioStreamChangeNotification(
                            stream,
                            PulseAudioStreamChangeNotificationType.Deleted
                        )
                    );
                },
                (ex) => { Console.WriteLine(ex.ToString()); }
            ).ConfigureAwait(false);

            _recordStreamRemovedSubscription = await _proxy.WatchRecordStreamRemovedAsync(
                async (path) =>
                {
                    IPulseAudioStream stream =
                        await _mediator.Send<IPulseAudioStream>(
                            new PulseAudioStreamGetRequest(path, Scope.Channel, StreamType.Record)
                        );

                    await _mediator.Publish(
                        new PulseAudioStreamChangeNotification(
                            stream,
                            PulseAudioStreamChangeNotificationType.Deleted
                        )
                    );
                },
                (ex) =>
                {
                    Console.WriteLine(ex.ToString());
                }
            ).ConfigureAwait(false);

            ObjectPath[] array = new List<ObjectPath>().ToArray();

            // Notify PulseAudio we are listening to the following events
            await _proxy.ListenForSignalAsync($"{PulseAudioDBus.CoreSeviceName}.NewPlaybackStream", array).ConfigureAwait(false);
            await _proxy.ListenForSignalAsync($"{PulseAudioDBus.CoreSeviceName}.PlaybackStreamRemoved", array).ConfigureAwait(false);
            await _proxy.ListenForSignalAsync($"{PulseAudioDBus.CoreSeviceName}.NewRecordStream", array).ConfigureAwait(false);
            await _proxy.ListenForSignalAsync($"{PulseAudioDBus.CoreSeviceName}.RecordStreamRemoved", array).ConfigureAwait(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _newPlaybackStreamSubscription?.Dispose();
                    _newRecordStreamSubscription?.Dispose();
                    _playbackStreamRemovedSubscription?.Dispose();
                    _recordStreamRemovedSubscription?.Dispose();
                    _newPlaybackStreamSubscription = null;
                    _newRecordStreamSubscription = null;
                    _playbackStreamRemovedSubscription = null;
                    _recordStreamRemovedSubscription = null;
                }
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~PulseAudioStreamWatchdog()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}