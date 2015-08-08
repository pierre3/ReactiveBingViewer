using System;

namespace ReactiveBingViewer.Notifiers
{
    /// <summary>
    /// LogMessage 抽象クラス
    /// </summary>
    public abstract class LogMessage
    {
        /// <summary>作成日時</summary>
        public DateTime CreatedAt { get; }
        /// <summary>Log メッセージ</summary>
        public string Message { get; }
        /// <summary>Log レベル</summary>
        public LogLevel Level { get; }
        /// <summary>例外オブジェクト</summary>
        public Exception Exception { get; }
        /// <summary>例外オブジェクトを持っているか</summary>
        public bool HasError { get { return Exception != null; } }

        public LogMessage(string message, LogLevel level, Exception e = null)
        {
            CreatedAt = DateTime.Now;
            Message = message;
            Level = level;
            Exception = e;
        }

        public override string ToString()
        {
            if (HasError)
            {
                return string.Format("{0}:[{1}] {2} [{3}]", Level.Name, CreatedAt.ToString("G"), Message, Exception.Message);
            }
            else
            {
                return string.Format("{0}:[{1}] {2}", Level.Name, CreatedAt.ToString("G"), Message);
            }
        }
    }

    /// <summary>
    /// Trace Log
    /// </summary>
    public class TraceLogMessage : LogMessage
    {
        public TraceLogMessage(string message, Exception e = null)
            : base(message, LogLevel.Trace, e)
        { }
    }

    /// <summary>
    /// Debug Log
    /// </summary>
    public class DebugLogMessage : LogMessage
    {
        public DebugLogMessage(string message, Exception e = null)
            : base(message, LogLevel.Debug, e)
        { }
    }

    /// <summary>
    /// Information Log
    /// </summary>
    public class InfoLogMessage : LogMessage
    {
        public InfoLogMessage(string message, Exception e = null)
            : base(message, LogLevel.Info, e)
        { }
    }

    /// <summary>
    /// Warning Log
    /// </summary>
    public class WarnLogMessage : LogMessage
    {
        public WarnLogMessage(string message, Exception e = null)
            : base(message, LogLevel.Warn, e)
        { }
    }

    /// <summary>
    /// Error Log
    /// </summary>
    public class ErrorLogMessage : LogMessage
    {
        public ErrorLogMessage(string message, Exception e = null)
            : base(message, LogLevel.Error, e)
        { }
    }

    /// <summary>
    /// Fatal Log
    /// </summary>
    public class FatalLogMessage : LogMessage
    {
        public FatalLogMessage(string message, Exception e = null)
            : base(message, LogLevel.Fatal, e)
        { }
    }
}
