namespace Midicontrol.Midi.NativeSinks.BinaryLauncher
{
    internal class BinaryLauncherSink : IMidiMessageSink
    {
        public string Name => "BinaryLauncher";

        public BinaryLauncherSink()
        {
            
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public Task ProcessMessageAsync(IEnumerable<IMidiMessageSinkArgs> args)
        {
            throw new NotImplementedException();
        }
    }
}