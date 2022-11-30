using System;
using Midicontrol.CLI;
using Midicontrol.Midi.NativeSinks;
using Xunit;

namespace tests;

public class VolumeControlActionTests
{
    [Fact]
    public void ShouldParseAdditionalParams()
    {
        string param = "mode";
        string value = "TwoWay";
        string arg = $"bind://channel/playback?{param}={value}";

        VolumeControlAction action = new VolumeControlAction(arg);

        Assert.True(action.AdditionalParams.Count > 0);
        Assert.Equal(action.AdditionalParams[param], value);
    }

    [Fact]
    public void ShouldThrowAnExceptionWhenActionIsUnkown()
    {
        string arg = $"whatever://channel/playback";

        Assert.Throws<ArgumentException>(() => new VolumeControlAction(arg));
    }

    [Fact]
    public void ShouldThrowAnExceptionWhenScopeIsUnkown()
    {
        string arg = $"bind://whatever/playback";

        Assert.Throws<ArgumentException>(() => new VolumeControlAction(arg));
    }

    [Fact]
    public void ShouldThrowAnExceptionWhenStreamTypeIsUnkown()
    {
        string arg = $"bind://channel/whatever";

        Assert.Throws<ArgumentException>(() => new VolumeControlAction(arg));
    }
}