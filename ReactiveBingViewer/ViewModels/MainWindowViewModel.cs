using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using ReactiveBingViewer.Models;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveBingViewer.Notifiers;
using System.Windows;

namespace ReactiveBingViewer.ViewModels
{

    public class MainWindowViewModel : IDisposable
    {
        private CompositeDisposable disposables = new CompositeDisposable();
        public LogMessageNotifier logger = new LogMessageNotifier();
        public ProgressNotifier progress = new ProgressNotifier();
        private WebImageStore webImageStore;

        /// <summary>[検索]コマンド</summary>
        public ReactiveCommand SearchCommand { get; private set; }
        /// <summary>[キャンセル]コマンド</summary>
        public ReactiveCommand CancelCommand { get; private set; }

        /// <summary>検索文字列</summary>
        public ReactiveProperty<string> SearchWord { get; private set; }
        /// <summary>検索結果画像一覧</summary>
        public ReadOnlyReactiveCollection<WebImageViewModel> Images { get; private set; }
        /// <summary>選択画像</summary>
        public ReactiveProperty<WebImageViewModel> SelectedImage { get; private set; }

        /// <summary>検索の進捗率</summary>
        public ReactiveProperty<double> PercentProgress { get; private set; }
        /// <summary>ステータスバーに表示するメッセージ</summary>
        public ReadOnlyReactiveProperty<LogMessage> StatusMessage { get; private set; }

        /// <summary>エラーログ一覧表示用コレクション</summary>
        public ReadOnlyReactiveCollection<LogMessage> ErrorLogs { get; private set; }
        /// <summary>エラーログ一覧の表示状態</summary>
        public ReactiveProperty<Visibility> ErrorLogsVisibility { get; private set; }
        /// <summary>エラー一覧のクリア</summary>
        public ReactiveCommand ClearErrorLogsCommand { get; private set; }

        /// <summary>コンストラクタ</summary>
        public MainWindowViewModel()
        {
            //Model
            webImageStore = new WebImageStore(App.BingApiAccountKey, App.VisionApiSubscriptionKey, logger).AddTo(disposables);

            InitializeSearchBar();
            InitializeThumnailBar();
            InitializeStatusBar();
        }

        /// <summary>
        /// 検索バー用プロパティの初期化
        /// </summary>
        private void InitializeSearchBar()
        {
            SearchWord = new ReactiveProperty<string>("");

            //実行中⇔停止中を通知
            var isProcessing = progress.IsProcessingObservable.StartWith(false);

            //[検索]コマンド
            // 検索ボックスに文字が入っていて、検索実行中でない場合に実行可能
            SearchCommand = new[]{
                SearchWord.Select(x => string.IsNullOrWhiteSpace(x)),
                isProcessing
            }
            .CombineLatestValuesAreAllFalse()
            .ToReactiveCommand();
            SearchCommand.Subscribe(_ =>
            {
                webImageStore.Clear();
                webImageStore.DownloadWebImage(SearchWord.Value, progress);
            }).AddTo(disposables);

            //[キャンセル]コマンド
            //検索中の場合のみ実行可能
            CancelCommand = isProcessing.ToReactiveCommand();
            CancelCommand.Subscribe(_ =>
            {
                webImageStore.Cancel();
            }).AddTo(disposables);
        }

        /// <summary>
        /// サムネイルバー用プロパティの初期化
        /// </summary>
        private void InitializeThumnailBar()
        {
            //検索結果画像一覧
            Images = webImageStore.Images
                .ToReadOnlyReactiveCollection(source => new WebImageViewModel(source, App.VisionApiSubscriptionKey, logger));

            //選択画像
            SelectedImage = new ReactiveProperty<WebImageViewModel>();
            SelectedImage.Where(x => x != null).Subscribe(async item =>
            {
                //サムネイルを選択したら実行する処理
                //フルサイズ画像のダウンロードと画像の解析を実行
                await item.DownloadImageDetailsAsync();
            }).AddTo(disposables);
        }

        /// <summary>
        /// ステータスバー用プロパティの初期化
        /// </summary>
        private void InitializeStatusBar()
        {
            //進捗率を通知
            PercentProgress = progress.PercentProgressObservable.ToReactiveProperty();

            //Logファイルに書き出し
            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener("log.txt"));
            logger.Subscribe(log =>
            {
                System.Diagnostics.Trace.WriteLine(log);
                System.Diagnostics.Trace.Flush();
            }).AddTo(disposables);

            //Infoレベルのログはステータスバーに表示
            StatusMessage = logger.Where(x => x.Level == LogLevel.Info)
                .ToReadOnlyReactiveProperty();
            
            //Warn レベル以上 は エラーリストに表示
            ClearErrorLogsCommand = new ReactiveCommand();  //リストクリア用
            ErrorLogs = logger
                .OfType<LogMessage>()
                .Where(x => x.Level >= LogLevel.Warn)
                .ToReadOnlyReactiveCollection(ClearErrorLogsCommand.ToUnit());

            //エラーリスト表示切替。 ErrorLogs にアイテムがある場合のみ表示する
            ErrorLogsVisibility = ErrorLogs
                .CollectionChangedAsObservable()
                .Select(_ => (ErrorLogs.Count > 0) ? Visibility.Visible : Visibility.Collapsed)
                .ToReactiveProperty(Visibility.Collapsed)
                .AddTo(disposables);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) { return; }
            if (disposing)
            {
                disposables.Dispose();
            }
            disposed = true;
        }
        private bool disposed = false;
    }
}
