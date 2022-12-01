using Midicontrol.Midi.NativeSinks.PulseAudio;
using Xunit;

namespace Midicontrol.Tests
{
    public class PulseAudioExtentionsTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(127, 65535)]
        public void ShouldConvertUIntValueToPulseAudioValue(uint baseValue, uint expectedValue)
        {
            uint resValue = baseValue.ToRatioUInt16();

            Assert.Equal(resValue, expectedValue);
        }
    }
}