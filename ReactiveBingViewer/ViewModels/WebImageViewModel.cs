using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactiveBingViewer.Models;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using ReactiveBingViewer.Notifiers;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ReactiveBingViewer.ViewModels
{
    public class WebImageViewModel : IDisposable
    {
        private WebImage source;
        private ILogger logger;
        private string visionApiSubscriptionKey;
        private CompositeDisposable disposables = new CompositeDisposable();
        private ProgressNotifier progress = new ProgressNotifier();
        private Size imageSize = new Size();

        /// <summary>サムネイル画像</summary>
        public ReadOnlyReactiveProperty<ImageSource> Thumbnail { get; }
        /// <summary>表示用画像</summary>
        public ReadOnlyReactiveProperty<ImageSource> DisplayImage { get; }
        /// <summary>解析結果重ね合わせ用画像</summary>
        public ReadOnlyReactiveProperty<ImageSource> Overlay { get; }
        /// <summary>リンク元ページのURL</summary>
        public ReadOnlyReactiveProperty<string> SourceUrl { get; }
        /// <summary>リンク元ページのタイトル</summary>
        public ReadOnlyReactiveProperty<string> SourceTitle { get; }
        /// <summary>画像解析結果</summary>
        public ReadOnlyReactiveProperty<ImageProperty> ImageProperty { get; }
        /// <summary>実行中フラグ</summary>
        public ReadOnlyReactiveProperty<bool> IsProcessing { get; }
        /// <summary>プログレスバー表示状態</summary>
        public ReadOnlyReactiveProperty<Visibility> ProgressVisibility { get; }

        /// <summary>画像サイズ変更時に実行するコマンド</summary>
        public ReactiveCommand<Size> SizeChangedCommand { get; private set; }
        /// <summary>リンク元ページURLのハイパーリンククリック時に実行するコマンド</summary>
        public ReactiveCommand NavigateCommand { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="source">関連付けるWebImageオブジェクト</param>
        /// <param name="visionApiSubscriptionKey">Microsoft Vision API のSubscription Key</param>
        /// <param name="logger">ログメッセージ通知オブジェクト</param>
        public WebImageViewModel(WebImage source, string visionApiSubscriptionKey, ILogger logger)
        {
            this.source = source;
            this.logger = logger;
            this.visionApiSubscriptionKey = visionApiSubscriptionKey;

            //Model(WebImage)のプロパティをReactivePropertyに変換
            Thumbnail = source.ObserveProperty(x => x.Thumbnail).ToReadOnlyReactiveProperty();
            DisplayImage = source.ObserveProperty(x => x.DisplayImage).ToReadOnlyReactiveProperty();
            Overlay = source.ObserveProperty(x => x.Overlay).ToReadOnlyReactiveProperty();
            SourceUrl = source.ObserveProperty(x => x.SourceUrl).ToReadOnlyReactiveProperty();
            SourceTitle = source.ObserveProperty(x => x.SourceTitle).ToReadOnlyReactiveProperty();
            ImageProperty = source.ObserveProperty(x => x.ImageProperty).ToReadOnlyReactiveProperty();

            //実行中フラグ
            IsProcessing = progress.IsProcessingObservable.StartWith(false)
                .ToReadOnlyReactiveProperty();

            //プログレスバーの表示切替 - 実行中のみ表示する
            ProgressVisibility = IsProcessing
                .Select(x => x ? Visibility.Visible : Visibility.Collapsed)
                .ToReadOnlyReactiveProperty();

            //画像のサイズ変更時に実行するコマンド
            SizeChangedCommand = new ReactiveCommand<Size>();
            SizeChangedCommand
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(size =>
                {
                    //現在のサイズで 顔領域の矩形を再描画
                    source.DrawFaceRect(size);
                    //最新のサイズを保存しておく ⇒画像再選択時に使う
                    imageSize = size;
                })
                .AddTo(disposables);

            //リンク元ページURLのハイパーリンククリック時
            NavigateCommand = new ReactiveCommand();
            NavigateCommand.Subscribe(_ =>
            {
                Process.Start(source.SourceUrl);
            }).AddTo(disposables);

        }

        /// <summary>
        /// フルサイズ画像のダウンロードと画像解析を実行します
        /// </summary>
        /// <returns>Task</returns>
        public async Task DownloadImageDetailsAsync()
        {
            await Task.WhenAll(
                DownLoadFullImageAsync(),
                AnalyzeImageAsync());
            source.DrawFaceRect(imageSize);
        }

        /// <summary>
        /// フルサイズ画像をダウンロードします
        /// </summary>
        /// <returns>Task</returns>
        private async Task DownLoadFullImageAsync()
        {
            using (progress.Start())
            {
                await source.DownLoadFullImageAsync();
            }
        }

        /// <summary>
        /// Vision APIを使用して画像を解析します
        /// </summary>
        /// <returns>Task</returns>
        private async Task AnalyzeImageAsync()
        {
            using (progress.Start())
            {
                await source.AnalyzeImageAsync(visionApiSubscriptionKey);
            }
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
        
    }

}
