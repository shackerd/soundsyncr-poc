using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Midicontrol.CLI;
using Midicontrol.Midi;
using Midicontrol.Midi.NativeSinks;
using Moq;
using Xunit;

namespace tests;

public class VolumeControlSinkTests
{
    [Fact]
    public async Task ShouldSetVolume()
    {
        Mock<ILogger<VolumeControlSink>> mockLogger = new();
        Mock<IAudioDriver> mockDriver = new();
        Mock<IAudioStream> mockStream = new();
        Mock<IMidiMessageSinkArgs> mockArg = new();

        mockArg.Setup((a) => a.Action).Returns(() => "bind://channel/playback");
        mockArg.Setup((a) => a.Destination).Returns(() => "myProcess");
        mockArg.Setup((a) => a.Value).Returns(() => 0);

        mockStream.Setup((s) => s.Identifier).Returns(() => "myProcess");
        mockStream.Setup((s) => s.Scope).Returns(() => Scope.Channel);
        mockStream.Setup((s) => s.Type).Returns(() => StreamType.Playback);

        mockDriver
            .Setup((d) => d.GetStreamsAsync(It.IsAny<string>()))
            .Returns(() => Task.FromResult(new List<IAudioStream>() { mockStream.Object }.AsEnumerable()));

        VolumeControlSink sink = new VolumeControlSink(mockLogger.Object, mockDriver.Object);

        await sink.ProcessMessageAsync(new List<IMidiMessageSinkArgs>() { mockArg.Object });

        mockStream.Verify((s) => s.SetVolumeAsync(It.IsAny<uint>()));
    }

    [Theory]
    [InlineData(false, 0)]
    [InlineData(true, 127)]
    public async Task ShouldToggleMute(bool muteValue, int midiMsgValue)
    {
        Mock<ILogger<VolumeControlSink>> mockLogger = new();
        Mock<IAudioDriver> mockDriver = new();
        Mock<IAudioStream> mockStream = new();
        Mock<IMidiMessageSinkArgs> mockArg = new();

        mockArg.Setup((a) => a.Action).Returns(() => "mute://channel/playback");
        mockArg.Setup((a) => a.Destination).Returns(() => "myProcess");
        mockArg.Setup((a) => a.Value).Returns(() => midiMsgValue);

        mockStream.Setup((s) => s.Identifier).Returns(() => "myProcess");
        mockStream.Setup((s) => s.Scope).Returns(() => Scope.Channel);
        mockStream.Setup((s) => s.Type).Returns(() => StreamType.Playback);

        mockDriver
            .Setup((d) => d.GetStreamsAsync(It.IsAny<string>()))
            .Returns(() => Task.FromResult(new List<IAudioStream>() { mockStream.Object }.AsEnumerable()));

        VolumeControlSink sink = new VolumeControlSink(mockLogger.Object, mockDriver.Object);

        await sink.ProcessMessageAsync(new List<IMidiMessageSinkArgs>() { mockArg.Object });

        mockStream.Verify((s) => s.ToggleMuteAsync(It.Is<bool>((v) => v == muteValue)));
    }
}