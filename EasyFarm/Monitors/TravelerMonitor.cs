using System.Threading;
using System.Threading.Tasks;
using EasyFarm.Classes;
using EasyFarm.Context;
using MemoryAPI;

namespace EasyFarm.Monitors
{
    public class TravelerMonitor : IMonitor
    {
        private TravelerSetup _travelerSetup ;
        public TravelerMonitor(IMemoryAPI fface, GameContext gameContext)
        {
            _travelerSetup = new TravelerSetup(fface, gameContext);
        }
        private CancellationTokenSource _tokenSource;
        
        public void Start()
        {
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(Monitor, _tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void Stop()
        {
            _tokenSource.Cancel();
        }

        public void Monitor()
        {
            while (true)
            {
                if (_tokenSource.IsCancellationRequested)
                {
                    _tokenSource.Token.ThrowIfCancellationRequested();
                }

                _travelerSetup.RunComponent();

                TimeWaiter.Pause(100);
            }
        }
    }
}