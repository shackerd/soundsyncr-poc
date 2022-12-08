using System;
using Midicontrol.Midi.NativeSinks;
using Midicontrol.Midi.NativeSinks.VolumeControl;
using Xunit;

namespace Midicontrol.Tests;

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