using System.Windows;

namespace ReactiveBingViewer
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public static string BingApiAccountKey { get; private set; }
        public static string VisionApiSubscriptionKey { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            Reactive.Bindings.UIDispatcherScheduler.Initialize();

            base.OnStartup(e);
            if (e.Args.Length >= 2)
            {
                BingApiAccountKey = e.Args[0];
                VisionApiSubscriptionKey = e.Args[1];
            }
            else
            {
                BingApiAccountKey = ReactiveBingViewer.Properties.Settings.Default.BingApiAccountKey;
                VisionApiSubscriptionKey = ReactiveBingViewer.Properties.Settings.Default.VisionApiSubscriptionKey;
            }

        }
    }
}
