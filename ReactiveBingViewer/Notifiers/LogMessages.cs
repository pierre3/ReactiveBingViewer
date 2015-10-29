using System;

namespace ReactiveBingViewer.Notifiers
{
    /// <summary>
    /// LogMessage 抽象クラス
    /// </summary>
    public class LogMessage
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

        public static LogMessage CreateTraceLog(string message, Exception e = null)
        {
            return new LogMessage(message, LogLevel.Trace, e);
        }

        public static LogMessage CreateDebugLog(string message, Exception e = null)
        {
            return new LogMessage(message, LogLevel.Debug, e);
        }

        public static LogMessage CreateInfoLog(string message, Exception e = null)
        {
            return new LogMessage(message, LogLevel.Info, e);
        }

        public static LogMessage CreateWarnLog(string message, Exception e = null)
        {
            return new LogMessage(message, LogLevel.Warn, e);
        }

        public static LogMessage CreateErrorLog(string message, Exception e = null)
        {
            return new LogMessage(message, LogLevel.Error, e);
        }

        public static LogMessage CreateFatalLog(string message, Exception e = null)
        {
            return new LogMessage(message, LogLevel.Fatal, e);
        }
    }
   
    

   
}
