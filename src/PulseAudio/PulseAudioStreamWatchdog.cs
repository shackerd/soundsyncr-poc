using Tmds.DBus;
using Midicontrol.PulseAudio.DBus;

namespace Midicontrol.PulseAudio
{
    public class PulseAudioStreamWatchdog : IDisposable
    {
        private const string _coreSeviceName = "org.PulseAudio.Core1";
        private IDisposable _newPlaybackStreamSubscription;
        private IDisposable _newRecordStreamSubscription;
        private IDisposable _playbackStreamRemovedSubscription;
        private IDisposable _recordStreamRemovedSubscription;
        private bool disposedValue;
        private readonly ICoreProxy _proxy;
        
        public event EventHandler<PulseAudioStreamChangeEventArgs> OnStreamChanged;

        public PulseAudioStreamWatchdog(ICoreProxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException(nameof(proxy));
            }

            _proxy = proxy;
        }

        public Task InitializeAsync() 
        {
            return InitializeAsyncInternal();
        }

        private async Task InitializeAsyncInternal() 
        {
            await ListenToStreamsChangesAsync().ConfigureAwait(false);
        }

        private async Task ListenToStreamsChangesAsync()
        {
            _newPlaybackStreamSubscription = await _proxy.WatchNewPlaybackStreamAsync(
                (path) =>
                {
                    OnStreamChanged?.Invoke(
                        this, 
                        new PulseAudioStreamChangeEventArgs(
                            path.ToString(), 
                            PulseAudioStreamType.PlaybackStream, 
                            PulseAudioStreamChangeType.Created
                        )
                    );
                },
                (ex) => { Console.WriteLine(ex.ToString()); }
            ).ConfigureAwait(false);

            _newRecordStreamSubscription = await _proxy.WatchNewRecordStreamAsync(
                (path) =>
                {
                    OnStreamChanged?.Invoke(
                        this, 
                        new PulseAudioStreamChangeEventArgs(
                            path.ToString(), 
                            PulseAudioStreamType.RecordStream, 
                            PulseAudioStreamChangeType.Created
                        )
                    );
                },
                (ex) =>
                {
                    Console.WriteLine(ex.ToString());
                }
            ).ConfigureAwait(false);

            _playbackStreamRemovedSubscription = await _proxy.WatchPlaybackStreamRemovedAsync(
                (path) =>
                {
                    OnStreamChanged?.Invoke(
                        this, 
                        new PulseAudioStreamChangeEventArgs(
                            path.ToString(), 
                            PulseAudioStreamType.PlaybackStream, 
                            PulseAudioStreamChangeType.Removed
                        )
                    );
                },
                (ex) => { Console.WriteLine(ex.ToString()); }
            ).ConfigureAwait(false);

            _recordStreamRemovedSubscription = await _proxy.WatchRecordStreamRemovedAsync(
                (path) =>
                {
                    OnStreamChanged?.Invoke(
                        this, 
                        new PulseAudioStreamChangeEventArgs(
                            path.ToString(), 
                            PulseAudioStreamType.RecordStream, 
                            PulseAudioStreamChangeType.Removed
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
            await _proxy.ListenForSignalAsync($"{_coreSeviceName}.NewPlaybackStream", array).ConfigureAwait(false);
            await _proxy.ListenForSignalAsync($"{_coreSeviceName}.PlaybackStreamRemoved", array).ConfigureAwait(false);
            await _proxy.ListenForSignalAsync($"{_coreSeviceName}.NewRecordStream", array).ConfigureAwait(false);
            await _proxy.ListenForSignalAsync($"{_coreSeviceName}.RecordStreamRemoved", array).ConfigureAwait(false);
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