namespace Midicontrol.Midi
{
    public enum MidiMessageType
    {
        NoteOff = 128,
        NoteOn=144,
        PolyPressure=160,
        ControlChange=176,
        ProgramChange=192,
        ChannelPressure=208,
        PitchWheel=224,
        SystemExclusive=240,
        SongPosition=242,
        SongSelect=243,
        TuneRequest=246,
        EndOfExclusive=247,
        TimingClock=248,
        Start=250,
        Continue=251,
        Stop=252,
        ActiveSensing=254,
        SystemReset=255
    }
}