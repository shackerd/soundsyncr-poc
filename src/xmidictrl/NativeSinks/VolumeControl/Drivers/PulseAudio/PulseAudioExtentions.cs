namespace Midicontrol.Midi.NativeSinks.PulseAudio
{
    public static class PulseAudioExtentions
    {
        public static uint ToRatioUInt16(this uint value)
        {
            // MIDI MSB (Most Significant Byte) cc (ControlChange) max value is encoded on 7-bit -> byte.MaxValue >> 1 (0x7F)

            if(value > 0x7F){
                throw new ArgumentOutOfRangeException("Value exceed 0x7F (127)");
            }

            uint paValue = (uint)(value * 0x205); // 0x204 * 0x7F  will not reach 0xFFFF

            paValue = Math.Clamp(paValue, 0x0, 0xFFFF); // 0xFFFF is volume max value, beyond it is amplified

            return paValue;
        }
    }
}