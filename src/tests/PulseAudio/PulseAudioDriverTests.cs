
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
            Mock<IMediator> mockMediator = new();

            PulseAudioDriver driver =
                new PulseAudioDriver(
                    mockLogger.Object,
                    new SynchronizationContext(),
                    mockMediator.Object
                );

            await driver.InitializeAsync();

            mockMediator.Verify((m) => m.Publish(It.IsAny<PulseAudioConnectionNotification>(), It.IsAny<CancellationToken>()));
        }
    }
}