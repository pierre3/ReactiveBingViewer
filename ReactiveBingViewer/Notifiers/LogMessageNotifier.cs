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

        public void Trace(string message) => Report(LogMessage.CreateTraceLog(message));
        public void Debug(string message) => Report(LogMessage.CreateDebugLog(message));
        public void Info(string message) => Report(LogMessage.CreateInfoLog(message));
        public void Warn(string message) => Report(LogMessage.CreateWarnLog(message));
        public void Error(string message) => Report(LogMessage.CreateErrorLog(message));
        public void Fatal(string message) => Report(LogMessage.CreateFatalLog(message));
        public void Trace(string message, Exception e) => Report(LogMessage.CreateTraceLog(message, e));
        public void Debug(string message, Exception e) => Report(LogMessage.CreateDebugLog(message, e));
        public void Info(string message, Exception e) => Report(LogMessage.CreateInfoLog(message, e));
        public void Warn(string message, Exception e) => Report(LogMessage.CreateWarnLog(message, e));
        public void Error(string message, Exception e) => Report(LogMessage.CreateErrorLog(message, e));
        public void Fatal(string message, Exception e) => Report(LogMessage.CreateFatalLog(message, e));
    }
}
