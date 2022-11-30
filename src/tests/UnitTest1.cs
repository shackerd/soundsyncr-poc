using Midicontrol.CLI;
using Xunit;

namespace tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        DebugCommand c = new DebugCommand(null);
        Assert.True(true);
    }
}