using MediatR;
using Microsoft.Extensions.Logging;
using Midicontrol.Midi.NativeSinks.PulseAudio.DBus;
using Midicontrol.Midi.NativeSinks.VolumeControl;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal sealed class PulseAudioDriver : IAudioDriver
    {
        private readonly ILogger<PulseAudioDriver> _logger;
        private readonly IPulseAudioConnection _connection;
        private readonly IPulseAudioWatchdog _watchdog;
        private readonly IMediator _mediator;


        public PulseAudioDriver(
            ILogger<PulseAudioDriver> logger,
            IPulseAudioConnection connection,
            IPulseAudioWatchdog watchdog,
            IMediator mediator
        )
        {
            _logger = logger;
            _connection = connection;
            _watchdog = watchdog;
            _mediator = mediator;
        }
        public Task InitializeAsync()
        {
            return InitializeAsyncInternal();
        }

        private async Task InitializeAsyncInternal()
        {
            await _connection.ConnectAsync();

            if(_connection.Connection == null){
                throw new InvalidOperationException();
            }

            Connection connection = _connection.Connection;

            ICoreProxy proxy =
                connection.CreateProxy<ICoreProxy>(PulseAudioDBus.CoreSeviceName, PulseAudioDBus.CoreObjectPath);

            CoreProperties props = await proxy.GetAllAsync();

            foreach (var item in props.PlaybackStreams)
            {
                await _mediator.Send(new PulseAudioStreamGetRequest(item, Scope.Channel, StreamType.Playback));
            }

            foreach (var item in props.RecordStreams)
            {
                await _mediator.Send(new PulseAudioStreamGetRequest(item, Scope.Channel, StreamType.Record));
            }

            foreach (var item in props.Sinks)
            {
                await _mediator.Send(new PulseAudioStreamGetRequest(item, Scope.Device, StreamType.Playback));
            }

            foreach (var item in props.Sources)
            {
                await _mediator.Send(new PulseAudioStreamGetRequest(item, Scope.Device, StreamType.Record));
            }

            await _watchdog.InitializeAsync();
        }


        public async Task<IEnumerable<IAudioStream>> GetStreamsAsync(string destination)
        {
            IEnumerable<IPulseAudioStream> query = await _mediator.Send(new PulseAudioStreamStoreQueryRequest());
            return query.Where(s => destination.Equals(s.Identifier)).AsEnumerable();
        }

        public async Task ToggleSoloAsync(IAudioStream stream, StreamType type, bool solo)
        {
            IEnumerable<IPulseAudioStream> query = await _mediator.Send(new PulseAudioStreamStoreQueryRequest());

            ObjectPath root = (((IPulseAudioStream)stream).Root as IPulseAudioStream)?.ObjectPath ?? default(ObjectPath);
            var muteStreams = query.Where(s => s.Type == type && s != (IPulseAudioStream)stream && (s.ObjectPath != root) && s.Scope == Scope.Channel);

            await Task.WhenAll(muteStreams.Select(s => s.ToggleMuteAsync(solo)));
        }
    }
}