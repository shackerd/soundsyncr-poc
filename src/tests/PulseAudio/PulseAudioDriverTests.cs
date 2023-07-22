
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Midicontrol.Midi.NativeSinks;
using Midicontrol.Midi.NativeSinks.PulseAudio;
using Moq;
using Xunit;

namespace Midicontrol.Tests
{
    public class PulseAudioDriverTests
    {

        [Fact]
        public async Task ShouldPublishConnectionNotificationOnInitialize()
        {
            Mock<ILogger<PulseAudioDriver>> mockLogger = new();
            Mock<ILogger<IPulseAudioConnection>> mockLoggerConnection = new();
            Mock<ILogger<PulseAudioWatchdog>> mockLoggerWatcher = new();
            Mock<IMediator> mockMediator = new();

            PulseAudioConnection connection =
                new PulseAudioConnection(mockLoggerConnection.Object, new SynchronizationContext(), mockMediator.Object);

            PulseAudioDriver driver =
                new PulseAudioDriver(
                    mockLogger.Object,
                    connection,
                    new PulseAudioWatchdog(connection, mockMediator.Object, mockLoggerWatcher.Object, new PulseAudioWatchdogHandleStore()),
                    mockMediator.Object
                );

            await driver.InitializeAsync();

            mockMediator.Verify((m) => m.Publish(It.IsAny<PulseAudioConnectionNotification>(), It.IsAny<CancellationToken>()));
        }
    }
}