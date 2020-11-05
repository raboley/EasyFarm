using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.ViewModels;
using MemoryAPI;
using MemoryAPI.Navigation;
using Pathfinder;
using Pathfinder.Map;
using Pathfinder.People;
using Pathfinder.Persistence;

namespace EasyFarm.Infrastructure
{
    public class NpcMonitor : IMonitor
    {
        private NpcPersister _npcPersister;
        public NpcMonitor(IMemoryAPI fface)
        {
            _npcPersister = new NpcPersister(fface);
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

                _npcPersister.RunComponent();

                TimeWaiter.Pause(100);
            }
        }
    }
}