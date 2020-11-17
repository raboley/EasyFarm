using System.Threading;
using System.Threading.Tasks;
using EasyFarm.Classes;
using EasyFarm.Context;
using MemoryAPI;

namespace EasyFarm.Monitors
{
    public class MobMonitor : IMonitor
    {
        private MobPersister _mobPersister ;
        public MobMonitor(IMemoryAPI fface, GameContext gameContext)
        {
            _mobPersister = new MobPersister(fface, gameContext);
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

                _mobPersister.RunComponent();

                TimeWaiter.Pause(100);
            }
        }
    }
}