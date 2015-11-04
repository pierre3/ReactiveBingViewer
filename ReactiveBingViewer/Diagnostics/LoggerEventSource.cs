//copied from http://neue.cc/2015/11/03_520.html

using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;

namespace ReactiveBingViewer.Diagnostics
{
    [EventSource(Name = "LoggerEventSource")]
    public class LoggerEventSource : EventSource
    {
        public static readonly LoggerEventSource Log = new LoggerEventSource();

        public class Keywords
        {
            public const EventKeywords Logging = (EventKeywords)1;
        }

        string FormatPath(string filePath)
        {
            if (filePath == null) return "";

            var xs = filePath.Split('\\');
            var len = xs.Length;
            if (len >= 3)
            {
                return xs[len - 3] + "/" + xs[len - 2] + "/" + xs[len - 1];
            }
            else if (len == 2)
            {
                return xs[len - 2] + "/" + xs[len - 1];
            }
            else if (len == 1)
            {
                return xs[len - 1];
            }
            else
            {
                return "";
            }
        }

        [Event(1, Level = EventLevel.LogAlways, Keywords = Keywords.Logging, Message = "[{2}:{3}][{1}]{0}")]
        public void LogAlways(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
        {
            WriteEvent(1, message ?? "", memberName ?? "", FormatPath(filePath) ?? "", line);
        }

        [Event(2, Level = EventLevel.Critical, Keywords = Keywords.Logging, Message = "[{2}:{3}][{1}]{0}")]
        public void Critical(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
        {
            WriteEvent(2, message ?? "", memberName ?? "", FormatPath(filePath) ?? "", line);
        }

        [Event(3, Level = EventLevel.Error, Keywords = Keywords.Logging, Message = "[{2}:{3}][{1}]{0}")]
        public void Error(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
        {
            WriteEvent(3, message ?? "", memberName ?? "", FormatPath(filePath) ?? "", line);
        }

        [Event(4, Level = EventLevel.Warning, Keywords = Keywords.Logging, Message = "[{2}:{3}][{1}]{0}")]
        public void Warning(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
        {
            WriteEvent(4, message ?? "", memberName ?? "", FormatPath(filePath) ?? "", line);
        }

        [Event(5, Level = EventLevel.Informational, Keywords = Keywords.Logging, Message = "[{2}:{3}][{1}]{0}")]
        public void Informational(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
        {
            WriteEvent(5, message ?? "", memberName ?? "", FormatPath(filePath) ?? "", line);
        }

        [Event(6, Level = EventLevel.Verbose, Keywords = Keywords.Logging, Message = "[{2}:{3}][{1}]{0}")]
        public void Verbose(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
        {
            WriteEvent(6, message ?? "", memberName ?? "", FormatPath(filePath) ?? "", line);
        }

        [Event(7, Level = EventLevel.Error, Keywords = Keywords.Logging, Version = 1)]
        public void Exception(string type, string stackTrace, string message)
        {
            WriteEvent(7, type ?? "", stackTrace ?? "", message ?? "");
        }

        [Conditional("DEBUG")]
        [Event(8, Level = EventLevel.Verbose, Keywords = Keywords.Logging, Message = "[{2}:{3}][{1}]{0}")]
        public void Debug(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
        {
            WriteEvent(8, message ?? "", memberName ?? "", FormatPath(filePath) ?? "", line);
        }

        [NonEvent]
        public IDisposable MeasureExecution(string label, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
        {
            return new StopwatchMonitor(this, label ?? "", memberName ?? "", FormatPath(filePath) ?? "", line);
        }

        [Event(9, Level = EventLevel.Informational, Keywords = Keywords.Logging, Message = "[{0}][{2}:{3}][{1}]{4}ms")]
        void MeasureExecution(string label, string memberName, string filePath, int line, double duration)
        {
            WriteEvent(9, label ?? "", memberName ?? "", FormatPath(filePath) ?? "", line, duration);
        }

        class StopwatchMonitor : IDisposable
        {
            readonly LoggerEventSource logger;
            readonly string label;
            readonly string memberName;
            readonly string filePath;
            readonly int line;
            Stopwatch stopwatch;

            public StopwatchMonitor(LoggerEventSource logger, string label, string memberName, string filePath, int line)
            {
                this.logger = logger;
                this.label = label;
                this.memberName = memberName;
                this.filePath = filePath;
                this.line = line;
                stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                if (stopwatch != null)
                {
                    stopwatch.Stop();
                    logger.MeasureExecution(label, memberName, filePath, line, stopwatch.Elapsed.TotalMilliseconds);
                    stopwatch = null;
                }
            }
        }
    }

}
