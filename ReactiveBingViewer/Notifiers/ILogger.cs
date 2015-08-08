﻿using System;

namespace ReactiveBingViewer.Notifiers
{
    public interface ILogger
    {
        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);

        void Trace(string message,Exception e);
        void Debug(string message, Exception e);
        void Info(string message, Exception e);
        void Warn(string message, Exception e);
        void Error(string message, Exception e);
        void Fatal(string message, Exception e);
    }
}
