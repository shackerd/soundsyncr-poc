using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Midicontrol.Midi.NativeSinks.PulseAudio;
using Moq;
using Xunit;

namespace Midicontrol.Tests
{
    public class PulseAudioStreamTests
    {

        [Theory]
        [InlineData(true, 0)]
        [InlineData(false, 127)]
        public async Task ShouldSetVolumeOnToggleMute(bool mute, uint expectedValue)
        {
            Mock<ILogger<PulseAudioStream>> mockLogger = new();

            PulseAudioStream stream = new PulseAudioStream("", mockLogger.Object);

            await stream.ToggleMuteAsync(mute);

            uint value = await stream.GetVolumeAsync();

            Assert.Equal(value, expectedValue);
        }
    }
}