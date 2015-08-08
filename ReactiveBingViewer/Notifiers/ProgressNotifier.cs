using Reactive.Bindings.Notifiers;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace ReactiveBingViewer.Notifiers
{

    public class ProgressNotifier : IProgressNotifier
    {
        private CountNotifier workCounter = new CountNotifier();
        private ScheduledNotifier<double> progress;

        public IObservable<bool> IsProcessingObservable
        {
            get { return workCounter.Select(x => x != CountChangedStatus.Empty).DistinctUntilChanged(); }
        }

        public IObservable<double> PercentProgressObservable
        {
            get { return progress.DistinctUntilChanged(); }
        }

        public ProgressNotifier()
        {
            progress = new ScheduledNotifier<double>();
        }

        public ProgressNotifier(IScheduler scheduler)
        {
            progress = new ScheduledNotifier<double>(scheduler);
        }

        public IDisposable Start() => workCounter.Increment();
        public void Progress(double percentProgress) => progress.Report(percentProgress);
        public void End() => workCounter.Decrement();
    }
}
