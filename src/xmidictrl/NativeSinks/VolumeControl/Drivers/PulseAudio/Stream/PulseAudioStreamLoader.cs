using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using MediatR;
using Microsoft.Extensions.Logging;
using Midicontrol.Midi.NativeSinks.PulseAudio.DBus;
using Midicontrol.Midi.NativeSinks.VolumeControl;
using Tmds.DBus;

namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    internal interface IPulseAudioStreamLoader
    {
        Task<IPulseAudioStream?> GetAsync(ObjectPath path, Scope scope, StreamType type);
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

        public async Task<IPulseAudioStream?> GetAsync(ObjectPath path, Scope scope, StreamType type)
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

        private async Task<IPulseAudioStream?> LoadChannelStreamAsync(ObjectPath path, StreamType type)
        {
            IStreamProxy proxy = _connection.CreateProxy<IStreamProxy>(PulseAudioDBus.StreamServiceName, path);

            StreamProperties? props = null;

            try
            {
                if (proxy == null)
                {
                    _logger.LogError($"Cannot create proxy for {type} ({path})");
                    return null;
                }

                props = await proxy.GetAllAsync();
            }
            catch (Tmds.DBus.DBusException ex)
            {
                _logger.LogError(ex, $"Stream was removed too early {type} ({path})");
                return null;
            }

            IReadOnlyDictionary<string, string> kvs = ReadAll(props!.PropertyList!);

            IPulseAudioStream? rootStream = await LoadDeviceStreamAsync(props.Device, type);

            if (rootStream == null)
            {
                _logger.LogWarning($"Cannot load device stream ({props.Device}), {type} stream is now orphaned ({path}), skipping...");
                return null;
            }

            // process binary name may have (deleted) or whatever suffix, so we ensure to get proper value by searching for first word
            string identifier = Regex.Match(kvs[_applicationProcessBinary], "^[^()\\s]+.*?").Value;

            PulseAudioChannelStream stream = new PulseAudioChannelStream(rootStream, identifier, path, type, proxy, _channelLogger);

            return stream;
        }

        private async Task<IPulseAudioStream?> LoadDeviceStreamAsync(ObjectPath path, StreamType type)
        {
            IDevice proxy = _connection.CreateProxy<IDevice>(PulseAudioDBus.DeviceServiceName, path);

            DeviceProperties? props = null;

            try
            {
                if (proxy == null)
                {
                    _logger.LogError($"Cannot create proxy for {type} ({path})");
                    return null;
                }

                props = await proxy.GetAllAsync();
            }
            catch (Tmds.DBus.DBusException ex)
            {
                _logger.LogError(ex, $"Stream was removed too early {type} ({path})");
                return null;
            }

            IReadOnlyDictionary<string, string> kvs = ReadAll(props!.PropertyList!);

            string name = kvs["device.description"];

            PulseAudioDeviceStream stream = new PulseAudioDeviceStream(type, props!.Name!, path, proxy, _deviceLogger);

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
            Span<byte> value = new Span<byte>(properties[propName]);

            // all values are string encoded with an additional nullbyte at the end
            return Encoding.Default.GetString(value.Slice(0, value.Length - 1));
        }
    }
}