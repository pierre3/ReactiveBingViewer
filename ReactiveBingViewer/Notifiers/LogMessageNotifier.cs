using System;
using Reactive.Bindings.Notifiers;
using System.Reactive.Concurrency;

namespace ReactiveBingViewer.Notifiers
{
    /// <summary>
    /// Logメッセージ通知オブジェクト
    /// </summary>
    public class LogMessageNotifier : ScheduledNotifier<LogMessage>, ILogger
    {
        public LogMessageNotifier(IScheduler scheduler) : base(scheduler) { }

        public LogMessageNotifier() : base() { }

        public void Trace(string message) => Report(new TraceLogMessage(message));
        public void Debug(string message) => Report(new DebugLogMessage(message));
        public void Info(string message) => Report(new InfoLogMessage(message));
        public void Warn(string message) => Report(new WarnLogMessage(message));
        public void Error(string message) => Report(new ErrorLogMessage(message));
        public void Fatal(string message) => Report(new FatalLogMessage(message));
        public void Trace(string message, Exception e) => Report(new TraceLogMessage(message, e));
        public void Debug(string message, Exception e) => Report(new DebugLogMessage(message, e));
        public void Info(string message, Exception e) => Report(new InfoLogMessage(message, e));
        public void Warn(string message, Exception e) => Report(new WarnLogMessage(message, e));
        public void Error(string message, Exception e) => Report(new ErrorLogMessage(message, e));
        public void Fatal(string message, Exception e) => Report(new FatalLogMessage(message, e));
    }
}
