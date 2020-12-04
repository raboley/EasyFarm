using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinalFantasyXI.Monitors
{
    public class ZoneMapMonitor : IMonitor
    {
        private ZoneMapPersister _zoneMapPersister;

        public ZoneMapMonitor(IMemoryAPI fface, GameContext gameContext)
        {
            _zoneMapPersister = new ZoneMapPersister(fface, gameContext);
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
                    _zoneMapPersister.RunComponent();
                }
                catch (ThreadInterruptedException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "ZoneMapMonitor thread interrupted", ex));
                }
                catch (ThreadAbortException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "ZoneMapMonitor thread aborted", ex));
                }
                catch (OperationCanceledException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "ZoneMapMonitor thread cancelled", ex));
                }
                catch (Exception ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Error, "ZoneMapMonitor error", ex));
                    LogViewModel.Write("ZoneMapMonitor: An error has occurred: please check easyfarm.log for more information");
                    AppServices.InformUser("An error occurred!");
                    // I do want to write exception message for now.
                    LogViewModel.Write(ex.Message);
                }

                TimeWaiter.Pause(100);
            }
        }
    }
}