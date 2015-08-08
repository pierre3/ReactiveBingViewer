
using System;

namespace ReactiveBingViewer.Notifiers
{
    /// <summary>
    /// ログレベル 構造体
    /// </summary>
    public struct LogLevel : IEquatable<LogLevel>, IComparable<LogLevel>
    {
        public static readonly LogLevel Trace = new LogLevel("Trace", 0);
        public static readonly LogLevel Debug = new LogLevel("Debug", 1);
        public static readonly LogLevel Info = new LogLevel("Info", 2);
        public static readonly LogLevel Warn = new LogLevel("Warn", 3);
        public static readonly LogLevel Error = new LogLevel("Error", 4);
        public static readonly LogLevel Fatal = new LogLevel("Fatal", 5);

        public string Name { get; }
        public int Level { get; }

        public LogLevel(string name, int level)
        {
            Name = name;
            Level = level;
        }

        public static bool operator ==(LogLevel left, LogLevel right) => left.Equals(right);
        public static bool operator !=(LogLevel left, LogLevel right) => !(left == right);
        public static bool operator <(LogLevel left, LogLevel right) => left.CompareTo(right) < 0;
        public static bool operator >(LogLevel left, LogLevel right) => left.CompareTo(right) > 0;
        public static bool operator <=(LogLevel left, LogLevel right) => left.CompareTo(right) <= 0;
        public static bool operator >=(LogLevel left, LogLevel right) => left.CompareTo(right) >= 0;

        public bool Equals(LogLevel other) => Level == other.Level;
        public int CompareTo(LogLevel other) => Level.CompareTo(other.Level);

        public override int GetHashCode() => Name.GetHashCode() ^ Level.GetHashCode();
        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            if (obj.GetType() != typeof(LogLevel))
            {
                return false;
            }

            return Equals((LogLevel)obj);
        }
    }
}
