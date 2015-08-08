using System;

namespace ReactiveBingViewer.Notifiers
{

    /// <summary>
    /// 何もしない ILogger
    /// </summary>
    public class EmptyLogger : ILogger
    {
        public void Debug(string message){}

        public void Debug(string message, Exception e) { }

        public void Error(string message) { }

        public void Error(string message, Exception e) { }

        public void Fatal(string message) { }

        public void Fatal(string message, Exception e) { }

        public void Info(string message) { }

        public void Info(string message, Exception e) { }

        public void Trace(string message) { }

        public void Trace(string message, Exception e) { }

        public void Warn(string message) { }

        public void Warn(string message, Exception e) { }
    }

}
