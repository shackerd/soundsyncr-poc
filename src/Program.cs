using System.ComponentModel;
using System.Text;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Midicontrol
{
    public class Program {
        public static void Main(string[] args)
        {                        
            var app = new CommandApp();
            app.Configure(config => {
                config
                    .AddCommand<DeviceCommand>("device")
                    .WithAlias("dev");
            });
            // app.Run(args);
            app.Run(new string[2]{"device", "--list"});
        }
    }

    internal class DeviceCommand : AsyncCommand<DeviceCommandSettings>
    {
        // xmidictrl -v
        // xmidictrl -d -l
        // xmidictrl --device --list
        // xmidictrl --device --list
        // xmidictrl -c -l
        // xmidictrl --channel --list
        public override async Task<int> ExecuteAsync(CommandContext context, DeviceCommandSettings settings)
        {
            if(settings.List){
                Table table = new Table();
                table.AddColumns("Id", "Interface", "Name", "Input", "Output", "Opened");
                table.Border(TableBorder.Rounded);

                foreach (var item in PortMidi.MidiDeviceManager.AllDevices)
                {
                    table.AddRow(item.ID.ToString(), item.Interface, item.Name, item.IsInput.ToString(), item.IsOutput.ToString(), item.IsOpened.ToString());
                }

                AnsiConsole.Write(table);

                var source = new CancellationTokenSource();
                var device = PortMidi.MidiDeviceManager.AllDevices.Last();

                MidiDeviceListener listener = new MidiDeviceListener(device);
                
                Console.WriteLine($"Listening to device: [underline green]{device.Name}[/]");
                Task listenerTask = listener.StartAsync();

                if((listenerTask.Status & TaskStatus.Faulted) == TaskStatus.Faulted)
                {
                    AnsiConsole.WriteException(listenerTask.Exception);
                }
                else {                    
                    Task.Run(() => AnsiConsole.Markup($"Listening to device: [underline green]{device.Name}[/]"));
                }

                await Task.Delay(5000);
                await listener.StopAsync();
                await Task.Delay(2000);
                listenerTask = listener.StartAsync();
                Console.ReadLine();
            }
            // Omitted
            return 0;
        }        
    }

    public class MidiDeviceListener : IDisposable
    {
        private const byte _defaultOverheatDelay = 10;
        private PortMidi.MidiDeviceInfo _device;
        private bool _listening;
        private CancellationTokenSource _cancellationSource = 
            new CancellationTokenSource();
        private PortMidi.MidiInput _input;
        private bool _isDisposing;
        private object _stateLock = new object();

        public MidiDeviceListener(PortMidi.MidiDeviceInfo device)
        {
            _device = device;            
        }

        private void Attach() {

            lock (_stateLock)
            {                            
                if(_device.IsOpened){
                    throw new InvalidOperationException("Device is already attached!");
                }

                _input = PortMidi.MidiDeviceManager.OpenInput(_device.ID);
                RefreshDevice();
            }
        }

        private void Detach(bool disposing = false) {
            
            lock(_stateLock)
            {
                if(disposing) {                    
                    if(_device.IsOpened) {
                        _input?.Close();                                     
                    }
                    // we don't need to refresh device when disposing
                    return;
                } 

                if(!_device.IsOpened){
                    throw new InvalidOperationException("Device is already unattached!");
                }

                _input?.Close();

                RefreshDevice();
            }
        }

        private void RefreshDevice() {
            lock (_stateLock)
            {
                _device = PortMidi.MidiDeviceManager.AllDevices.First(i => i.ID == _device.ID);
            }
        }


        public Task StartAsync()
        {           
            if(_input == null) {
                throw new InvalidOperationException("Midi input was null");
            }

            if(_cancellationSource.IsCancellationRequested && !_listening){
                _cancellationSource = new CancellationTokenSource();
            }            

            return Task.Factory
                .StartNew(() => Attach())
                .ContinueWith(_ => StartAsyncInternal(_cancellationSource.Token), TaskContinuationOptions.NotOnFaulted);            
        }

        private async Task StartAsyncInternal(CancellationToken cancellationToken)
        {                           
            // indicate to current object that we are listening 
            _listening = true;

            // recommended portmidi lib min value     
            const int msgSize = 100;             

            // copy stack ref in local scope for safer execution (multi tasks)
            PortMidi.MidiInput _in = _input;

            do
            {                              
                byte[] buffer = new byte[msgSize];                        

                // Better computed than _in.HasData
                bool hasData = _in.Read(buffer, 0, msgSize) > 0;                                                                                
                
                byte nextLoopWait = _defaultOverheatDelay;

                if(hasData){

                    // re read buffer as Event object 
                    var msg = (MidiMessage)_in.ReadEvent(buffer, 0, msgSize);

                    // todo: inject logger 
                    Console.WriteLine(msg.ToString());

                    // better response time in case of data
                    nextLoopWait = 2; 
                }

                // workaround to lighten CPU load 
                await Task.Delay(nextLoopWait);
                

            } while (!cancellationToken.IsCancellationRequested);          

            // indicate to current object that we had stopped listening 
            _listening = false;             
        }     

        public Task StopAsync() {
            return StopAsyncInternal().ContinueWith(_ => Detach());
        }

        private async Task StopAsyncInternal(){
            
            // sends cancellation signal
            _cancellationSource.Cancel();

            // wait for next loop iteration in StartAsyncInternal to handle cancellation
            await Task.Delay(_defaultOverheatDelay); 
        }

        public void Dispose() {
            if(!_isDisposing){
                _isDisposing = true;
                try
                {
                    // Ensure to detach from device before disposing
                    Detach(true);
                    _input = null;                    
                }
                catch (System.Exception)
                {                    
                    throw;
                }
            }
        }
    }

    internal struct PMidiMessage
    {

        //https://portmidi.github.io/portmidi_docs/group__grp__device.html#gabd50a31baaa494ad8b405f9ad54c966e
        
// PMEXPORT PmError Pm_OpenOutput	(	PortMidiStream ** 	stream,
// PmDeviceID 	outputDevice,
// void * 	outputDriverInfo,
// int32_t 	bufferSize,
// PmTimeProcPtr 	time_proc,
// void * 	time_info,
// int32_t 	latency 
// )	

    }

    internal class MidiMessage
    {
        public DateTime PublishedOn { get; }
        public long Controller { get; }
        public long Value { get; }
        public MidiMessageType Type { get; }

        private MidiMessage(long controller, long value, long status)
        {
            Controller = controller;
            Value = value;
            Type = (MidiMessageType)status;
            PublishedOn = DateTime.Now;
        }

        public static explicit operator MidiMessage(PortMidi.Event @event){
            return new MidiMessage(@event.Date1, @event.Data2, @event.Status);
        }

        public override string ToString()
        {
            return $"{PublishedOn.ToString("yyyy-MM-dd HH:mm:ss")}, {Type}, {Controller}, {Value}";
        }
    }

    internal class DeviceCommandSettings : CommandSettings
    {
        [Description("Enumerate available MIDI devices")]
        [CommandOption("-l|--list")]
        public bool List { get; set; }

        [CommandOption("-s|--set-default <ID>")]
        public bool SetDefault { get; set; }
    }

    internal class SetDefaultDeviceCommandSettings: CommandSettings 
    {

    }

    internal enum MidiMessageType
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

