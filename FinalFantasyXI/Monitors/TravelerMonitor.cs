using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinalFantasyXI.Monitors
{
    public class TravelerMonitor : IMonitor
    {
        private TravelerSetup _travelerSetup;

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


                try
                {
                    _travelerSetup.RunComponent();
                }
                catch (ThreadInterruptedException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "TravelerMonitor thread interrupted", ex));
                }
                catch (ThreadAbortException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "TravelerMonitor thread aborted", ex));
                }
                catch (OperationCanceledException ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Information, "TravelerMonitor thread cancelled", ex));
                }
                catch (Exception ex)
                {
                    Logger.Log(new LogEntry(LoggingEventType.Error, "TravelerMonitor error", ex));
                    LogViewModel.Write("TravelerMonitor: An error has occurred: please check easyfarm.log for more information");
                    AppServices.InformUser("An error occurred!");
                    // I do want to write exception message for now.
                    LogViewModel.Write(ex.Message);
                }

                TimeWaiter.Pause(100);
            }
        }
    }
}