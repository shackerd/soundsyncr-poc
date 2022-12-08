using MediatR;
using Microsoft.Extensions.Logging;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal class PulseAudioStreamStore
    {
        private readonly List<IPulseAudioStream> _streams = new List<IPulseAudioStream>();

        public List<IPulseAudioStream> Streams => _streams; // implement methods later

        public PulseAudioStreamStore()
        {

        }
    }

    internal class PulseAudioStreamStoreInterface : IRequestHandler<PulseAudioStreamGetRequest, IPulseAudioStream?>, IRequestHandler<PulseAudioStreamStoreQueryRequest, IEnumerable<IPulseAudioStream>>, INotificationHandler<PulseAudioStreamChangeNotification>
    {
        private readonly ILogger<PulseAudioStreamStoreInterface> _logger;
        private readonly IMediator _mediator;
        private readonly PulseAudioStreamStore _store;
        private readonly IPulseAudioStreamLoader _loader;

        public PulseAudioStreamStoreInterface(ILogger<PulseAudioStreamStoreInterface> logger, IMediator mediator, PulseAudioStreamStore store, IPulseAudioStreamLoader loader)
        {
            _logger = logger;
            _mediator = mediator;
            _store = store;
            _loader = loader;
        }

        public Task<IPulseAudioStream?> Handle(PulseAudioStreamGetRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return HandleInternal(request);
        }

        private async Task<IPulseAudioStream?> HandleInternal(PulseAudioStreamGetRequest request)
        {
            Func<IPulseAudioStream, bool> predicate =
                s => s.ObjectPath.Equals(request.Path);

            IPulseAudioStream? stream = _store.Streams.SingleOrDefault(predicate);

            if (stream == null)
            {
                stream = await _loader.GetAsync(request.Path, request.Scope, request.Type);

                if (stream != null)
                {
                    _store.Streams.Add(stream);
                }
            }

            return stream;
        }

        public Task Handle(PulseAudioStreamChangeNotification notification, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Func<IPulseAudioStream, bool> predicate =
                s => s.ObjectPath != default(ObjectPath) && s.ObjectPath.Equals(notification.Stream.ObjectPath);

            IEnumerable<IPulseAudioStream> results =
                _store.Streams.Where(predicate);
            switch (notification.Type)
            {
                // we already loaded stream in HandleInternal
                case PulseAudioStreamChangeNotificationType.Created:
                    return Task.CompletedTask;

                case PulseAudioStreamChangeNotificationType.Deleted:

                    if(results.Count() > 1) {
                        _logger.LogWarning($"More than one stream match to {notification.Stream.Identifier} on {notification.Type} {notification.Stream.Scope} when deleting");
                    }

                    foreach (IPulseAudioStream stream in results.ToArray()) // creates a copy, so iteration will not be done on the same resource where we want to delete items
                    {
                        _store.Streams.Remove(stream);
                    }

                    break;
                default:
                    throw new InvalidOperationException();
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<IPulseAudioStream>> Handle(PulseAudioStreamStoreQueryRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(_store.Streams.AsEnumerable());
        }
    }
}