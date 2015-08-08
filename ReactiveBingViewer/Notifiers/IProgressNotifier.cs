using System;

namespace ReactiveBingViewer.Notifiers
{
    /// <summary>
    /// 進捗の通知機能を提供するインターフェース
    /// </summary>
    /// <typeparam name="T">通知するメッセージの型</typeparam>
    public interface IProgressNotifier
    {
        IDisposable Start();
        void Progress(double percentProgress);
        void End();
    }
}
