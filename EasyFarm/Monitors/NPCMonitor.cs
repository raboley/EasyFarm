using System;
using System.Threading;
using System.Threading.Tasks;
using EasyFarm.Classes;
using EasyFarm.Context;
using EasyFarm.Logging;
using EasyFarm.ViewModels;
using MemoryAPI;

namespace EasyFarm.Monitors
{
    public class NpcMonitor : IMonitor
    {
        private NpcPersister _npcPersister;

        public NpcMonitor(IMemoryAPI fface, GameContext gameContext)
        {
            _npcPersister = new NpcPersister(fface, gameContext);
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


                try
                {
                    _npcPersister.RunComponent();
                }
                catch (ThreadInterruptedException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "NPCMonitor thread interrupted", ex));
                }
                catch (ThreadAbortException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "NPCMonitor thread aborted", ex));
                }
                catch (OperationCanceledException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "NPCMonitor thread cancelled", ex));
                }
                catch (Exception ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Error, "NPCMonitor error", ex));
                    LogViewModel.Write("NPCMonitor: An error has occurred: please check easyfarm.log for more information");
                    AppServices.InformUser("An error occurred!");
                    // I do want to write exception message for now.
                    LogViewModel.Write(ex.Message);
                }

                TimeWaiter.Pause(100);
            }
        }
    }
}