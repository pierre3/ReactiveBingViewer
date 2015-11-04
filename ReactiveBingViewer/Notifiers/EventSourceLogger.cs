using ReactiveBingViewer.Diagnostics;
using System;

namespace ReactiveBingViewer.Notifiers
{
    /// <summary>
    /// Logメッセージ通知オブジェクト
    /// </summary>
    public class EventSourceLogger : ILogger
    {

        public LoggerEventSource EventSource { get; private set; }
        public EventSourceLogger(LoggerEventSource eventSource =null)
        {
            EventSource = eventSource?? LoggerEventSource.Log;
        }

        public void Trace(string message) => EventSource.Verbose(message);
        public void Debug(string message) => EventSource.Debug(message);
        public void Info(string message) => EventSource.Informational(message);
        public void Warn(string message) => EventSource.Warning(message);
        public void Error(string message) => EventSource.Error(message);
        public void Fatal(string message) => EventSource.Critical(message);
        public void Trace(string message, Exception e)
        {
            Trace(message);
            LogException(e);
        }
        public void Debug(string message, Exception e)
        {
            Debug(message);
            LogException(e);
        }
        public void Info(string message, Exception e)
        {
            Info(message);
            LogException(e);
        }
        public void Warn(string message, Exception e)
        {
            Warn(message);
            LogException(e);
        }
        public void Error(string message, Exception e)
        {
            Error(message);
            LogException(e);
        }
        public void Fatal(string message, Exception e)
        {
            Fatal(message);
            LogException(e);
        }


        private void LogException(Exception e)
        {
            EventSource.Exception(e.GetType().FullName, e.StackTrace.ToString(), e.Message);
        }
    }
}
