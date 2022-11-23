using Tmds.DBus;
using Midicontrol.PulseAudio.DBus;
using Microsoft.Extensions.Logging;

namespace Midicontrol.PulseAudio
{
    public interface IPulseAudioStreamStore
    {
        IEnumerable<PulseAudioStream> RecordStreams { get; }
        IEnumerable<PulseAudioStream> PlaybackStreams { get; }
        IEnumerable<PulseAudioStream> AllStreams { get; }

        Task InitializeAsync();
    }

    internal class PulseAudioStreamStore : IPulseAudioStreamStore
    {
        private const string _coreSeviceName = "org.PulseAudio.Core1";
        private const string _core1ObjectPath = "/org/pulseaudio/core1";
        private const string _streamServiceName = "org.PulseAudio.Core1.Stream";

        private readonly Connection _connection;
        private readonly ICoreProxy _proxy;
        private readonly ILogger _logger;
        private readonly List<PulseAudioStream> _streams = new List<PulseAudioStream>();
        private readonly PulseAudioStreamWatchdog _watchdog;
        public IEnumerable<PulseAudioStream> RecordStreams => _streams.Where(s => s.Type == PulseAudioStreamType.RecordStream);
        public IEnumerable<PulseAudioStream> PlaybackStreams => _streams.Where(s => s.Type == PulseAudioStreamType.PlaybackStream);
        public IEnumerable<PulseAudioStream> AllStreams => _streams.AsEnumerable();

        public PulseAudioStreamStore(Connection connection, ILogger<IPulseAudioStreamStore> logger)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            _connection = connection;
            _logger = logger;
            _proxy = _connection.CreateProxy<ICoreProxy>(_coreSeviceName, _core1ObjectPath);
            _watchdog = new PulseAudioStreamWatchdog(_proxy);
        }

        public Task InitializeAsync()
        {   
            _watchdog.OnStreamChanged += 
                new EventHandler<PulseAudioStreamChangeEventArgs>(
                    async (s, e) => await OnStreamChange(s, e)
                );

            return InitializeAsyncInternal();
        }

        private async Task OnStreamChange(object sender, PulseAudioStreamChangeEventArgs e) 
        {
            switch (e.ChangeType)
            {
                case PulseAudioStreamChangeType.Created:
                    await OnStreamCreated(e.ObjectPath, e.StreamType);
                    break;
                case PulseAudioStreamChangeType.Removed:
                    OnStreamDeleted(e.ObjectPath);
                    break;
            }
        }

        private async Task InitializeAsyncInternal()
        {
            await GatherStreamsAsync(_proxy).ConfigureAwait(false);

            await _watchdog.InitializeAsync().ConfigureAwait(false);
        }

        private async Task GatherStreamsAsync(ICoreProxy proxy)
        {
            CoreProperties core = await proxy.GetAllAsync().ConfigureAwait(false);

            foreach (ObjectPath path in core.PlaybackStreams)
            {
                PulseAudioStream stream = await GetStreamObjectAsync(_connection, path, PulseAudioStreamType.PlaybackStream).ConfigureAwait(false);
                _streams.Add(stream);
            }

            foreach (ObjectPath path in core.RecordStreams)
            {
                PulseAudioStream stream = await GetStreamObjectAsync(_connection, path, PulseAudioStreamType.RecordStream).ConfigureAwait(false);
                _streams.Add(stream);
            }
        }

        private async Task OnStreamCreated(ObjectPath path, PulseAudioStreamType type)
        {
            // todo lock (concurrent)
            PulseAudioStream stream =
                await GetStreamObjectAsync(_connection, path, type).ConfigureAwait(false);

            _logger.LogInformation($"Created {stream.Type.ToString().ToLower()}: {stream.Binary} [{stream.Pid}]");

            _streams.Add(stream);
        }

        private void OnStreamDeleted(ObjectPath path)
        {
            // todo lock (concurrent)
            PulseAudioStream? stream = _streams.FirstOrDefault(s => s.Proxy.ObjectPath == path);

            if (stream == null)
            {
                return;
            }

            _logger.LogInformation($"Removed {stream.Type.ToString().ToLower()}: {stream.Binary} [{stream.Pid}]");

            _streams.Remove(stream);
        }

        private async Task<PulseAudioStream> GetStreamObjectAsync(Connection connection, ObjectPath path, PulseAudioStreamType type)
        {
            IStreamProxy streamProxy = connection.CreateProxy<IStreamProxy>(_streamServiceName, path);
            ICoreProxy coreProxy = connection.CreateProxy<ICoreProxy>(_coreSeviceName, _core1ObjectPath);
            PulseAudioStream stream = await PulseAudioStream.CreateAsync(streamProxy, coreProxy, type).ConfigureAwait(false);
            return stream;
        }        
    }       
}