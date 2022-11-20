namespace Midicontrol
{
    public class MidiDeviceListener : IDisposable
    {
        private const byte _defaultOverheatDelay = 10;
        private PortMidi.MidiDeviceInfo _device;
        private bool _listening;
        private CancellationTokenSource _cancellationSource = 
            new CancellationTokenSource();
        private PortMidi.MidiInput? _input;
        private bool _isDisposing;
        private object _stateLock = new object();

        public delegate void MidiMessageHandler(MidiMessage msg);
        public event MidiMessageHandler OnMidiMessage;


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
            if(_cancellationSource.IsCancellationRequested && !_listening){
                _cancellationSource = new CancellationTokenSource();
            }            

            return Task.Factory
                .StartNew(() => Attach())
                .ContinueWith(_ => StartAsyncInternal(_cancellationSource.Token), TaskContinuationOptions.NotOnFaulted);            
        }

        private async Task StartAsyncInternal(CancellationToken cancellationToken)
        {                     
            if(_input == null) {
                throw new InvalidOperationException("Midi input was null");
            }      
            // indicate to current object that we are listening 
            _listening = true;

            // recommended portmidi lib min value     
            const int msgSize = 100;             

            // copy stack ref in local scope for safer execution (multi tasks)
            PortMidi.MidiInput @in = _input;

            do
            {                              
                byte[] buffer = new byte[msgSize];                        

                // Better computed than _in.HasData
                bool hasData = @in.Read(buffer, 0, msgSize) > 0;                                                                                
                
                byte nextLoopWait = _defaultOverheatDelay;

                if(hasData){

                    // re read buffer as Event object 
                    var msg = (MidiMessage)@in.ReadEvent(buffer, 0, msgSize);

                    // todo: inject logger / dispatcher
                    Console.WriteLine(msg.ToString());
                    OnMidiMessage?.Invoke(msg);

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
}