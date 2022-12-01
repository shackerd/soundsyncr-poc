using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Midicontrol.Midi.NativeSinks.PulseAudio;
using Midicontrol.PulseAudio.DBus;
using Moq;
using Xunit;

namespace Midicontrol.Tests
{
    public class PulseAudioStreamTests
    {
        [Fact]
        public async Task ShouldThrowAnExceptionWhenProxyIsNotInitialized()
        {
            Mock<ILogger<PulseAudioChannelStream>> mockLogger = new();

            PulseAudioChannelStream stream =
                new PulseAudioChannelStream(
                    "",
                    Midi.NativeSinks.StreamType.Playback,
#pragma warning disable CS8625  // Wanted scenario
                    null,
#pragma warning restore CS8625
                    mockLogger.Object
                );

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await stream.ToggleMuteAsync(false)
            );

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await stream.GetVolumeAsync()
            );

            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await stream.SetVolumeAsync(0)
            );
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task ShouldToggleMute(bool mute)
        {
            Mock<ILogger<PulseAudioChannelStream>> mockLogger = new();
            Mock<IStreamProxy> mockProxy = new();

            mockProxy
                .Setup(p => p.SetAsync(It.IsAny<string>(), It.IsAny<bool>())) // SetAsync("Mute", val)
                .Verifiable();

            PulseAudioChannelStream stream =
                new PulseAudioChannelStream(
                    "",
                    Midi.NativeSinks.StreamType.Playback,
                    mockProxy.Object,
                    mockLogger.Object
                );

            await stream.ToggleMuteAsync(mute);

            mockProxy.Verify(p => p.SetAsync(It.Is<string>((v) => v == "Mute"), It.Is<bool>((v) => v == mute)));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(127, 65535)]
        public async Task ShouldSetProperVolume(uint value, uint expectedValue)
        {
            Mock<ILogger<PulseAudioChannelStream>> mockLogger = new();
            Mock<IStreamProxy> mockProxy = new();

            mockProxy
                .Setup(p => p.SetAsync(It.IsAny<string>(), It.IsAny<uint[]>())) // SetAsync("Volume", val)
                .Verifiable();

            PulseAudioChannelStream stream =
                new PulseAudioChannelStream(
                    "",
                    Midi.NativeSinks.StreamType.Playback,
                    mockProxy.Object,
                    mockLogger.Object
                );

            await stream.SetVolumeAsync(value);

            mockProxy.Verify(p => p.SetAsync(It.Is<string>((v) => v == "Volume"), It.Is<uint[]>((v) => v[0] == expectedValue)));
        }
    }
}