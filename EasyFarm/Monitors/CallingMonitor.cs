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
    public class CallingMonitor : IMonitor
    {
        private MobPersister _mobPersister;

        public CallingMonitor(IMemoryAPI fface, GameContext gameContext)
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


                try
                {
                    _mobPersister.RunComponent();
                }
                catch (ThreadInterruptedException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "CallingMonitor thread interrupted", ex));
                }
                catch (ThreadAbortException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "CallingMonitor thread aborted", ex));
                }
                catch (OperationCanceledException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "CallingMonitor thread cancelled", ex));
                }
                catch (Exception ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Error, "CallingMonitor error", ex));
                    LogViewModel.Write("CallingMonitor: An error has occurred: please check easyfarm.log for more information");
                    AppServices.InformUser("An error occurred!");
                    // I do want to write exception message for now.
                    LogViewModel.Write(ex.Message);
                }

                TimeWaiter.Pause(100);
            }
        }
    }
}