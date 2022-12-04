using System.Collections.ObjectModel;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using Midicontrol.PulseAudio.DBus;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal interface IPulseAudioStreamLoader : IRequestHandler<PulseAudioStreamLoadRequest, IPulseAudioStream>
    {
        Task<IPulseAudioStream> GetAsync(ObjectPath path, Scope scope, StreamType type);
    }

    internal class PulseAudioStreamLoader : IPulseAudioStreamLoader
    {
        private readonly Connection _connection;
        private readonly ILogger<PulseAudioStreamLoader> _logger;
        private readonly ILogger<PulseAudioDeviceStream> _deviceLogger;
        private readonly ILogger<PulseAudioChannelStream> _channelLogger;
        private readonly ICoreProxy _proxy;

        private const string _applicationProcessBinary = "application.process.binary";
        // private const string _applicationProcessId = "application.process.id";
        // private const string _applicationProcessName = "application.name";

        public PulseAudioStreamLoader(
            IPulseAudioConnection connection,
            ILogger<PulseAudioStreamLoader> logger,
            ILogger<PulseAudioDeviceStream> deviceLogger,
            ILogger<PulseAudioChannelStream> channelLogger
        )
        {
            _connection = connection?.Connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _deviceLogger = deviceLogger;
            _channelLogger = channelLogger;
            _proxy = _connection.CreateProxy<ICoreProxy>(PulseAudioDBus.CoreSeviceName, PulseAudioDBus.CoreObjectPath);
        }

        public async Task<IPulseAudioStream> GetAsync(ObjectPath path, Scope scope, StreamType type)
        {
            switch (scope)
            {
                case Scope.Device:
                    return await LoadDeviceStreamAsync(path, type);
                case Scope.Channel:
                    return await LoadChannelStreamAsync(path, type);
                default:
                    throw new InvalidOperationException();
            }
        }

        private async Task<IPulseAudioStream> LoadChannelStreamAsync(ObjectPath path, StreamType type)
        {
            IStreamProxy proxy = _connection.CreateProxy<IStreamProxy>(PulseAudioDBus.StreamServiceName, path);

            StreamProperties props = await proxy.GetAllAsync();

            IReadOnlyDictionary<string, string> kvs = ReadAll(props.PropertyList);

            PulseAudioChannelStream stream = new PulseAudioChannelStream(kvs[_applicationProcessBinary], path, type, proxy, _channelLogger);

            return stream;
        }

        private async Task<IPulseAudioStream> LoadDeviceStreamAsync(ObjectPath path, StreamType type)
        {
            IDevice proxy = _connection.CreateProxy<IDevice>(PulseAudioDBus.DeviceServiceName, path);
            DeviceProperties props = await proxy.GetAllAsync();

            PulseAudioDeviceStream stream = new PulseAudioDeviceStream(type, props.Name, path, proxy, _deviceLogger);

            return stream;
        }

        private IReadOnlyDictionary<string, string> ReadAll(IDictionary<string, byte[]> properties)
        {
            Dictionary<string, string> converted = new();

            foreach (string propName in properties.Keys)
            {
                converted.Add(propName, ReadProperty(properties, propName));
            }

            return new ReadOnlyDictionary<string, string>(converted);
        }

        private string ReadProperty(IDictionary<string, byte[]> properties, string propName)
        {
            byte[] rawValue = properties[propName];

            // skip last nullbyte
            byte[] value = rawValue.Take(rawValue.Length - 1).ToArray();

            return Encoding.Default.GetString(value);
        }

        public Task<IPulseAudioStream> Handle(PulseAudioStreamLoadRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return GetAsync(request.Path, request.Scope, request.Type);
        }
    }
}