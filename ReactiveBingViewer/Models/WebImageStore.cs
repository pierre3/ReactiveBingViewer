using ReactiveBingViewer.Notifiers;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace ReactiveBingViewer.Models
{
    /// <summary>
    /// WebImage のコレクションを提供するクラス
    /// </summary>
    /// <remarks>
    /// Bing画像検索して取得したURLから画像をダウンロードして保持します
    /// </remarks>
    public class WebImageStore : IDisposable
    {
        private ObservableCollection<WebImage> imagesSource;
        private ReadOnlyObservableCollection<WebImage> readonlyImages;
        private IDisposable disposable = Disposable.Empty;
        private string bingAccountKey;
        private string visionApiSubscriptionKey;
        private ILogger logger;

        /// <summary>
        /// ダウンロードした画像のコレクションを取得します
        /// </summary>
        public ReadOnlyObservableCollection<WebImage> Images { get { return readonlyImages; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WebImageStore(string bingApiAccountKey, string visionApiSubscriptionKey, ILogger logger)
        {
            this.logger = logger ?? new EmptyLogger();

            imagesSource = new ObservableCollection<WebImage>();
            readonlyImages = new ReadOnlyObservableCollection<WebImage>(this.imagesSource);

            bingAccountKey = bingApiAccountKey;
            this.visionApiSubscriptionKey = visionApiSubscriptionKey;
        }

        /// <summary>
        /// 実行中のダウンロード処理をキャンセルします
        /// </summary>
        public void Cancel()
        {
            disposable.Dispose();
            logger.Info("検索をキャンセルしました");
        }

        /// <summary>
        /// コレクションに保持しているWebImageオブジェクトをクリアします
        /// </summary>
        public void Clear()
        {
            imagesSource.Clear();
        }

        /// <summary>
        /// 指定した検索文字列でBing画像検索を行います。<para>
        /// 検索結果の画像URLから画像をダウンロードして内部コレクションに保持します。</para>
        /// </summary>
        /// <param name="searchWord">検索文字列</param>
        /// <param name="progress">進捗状態通知オブジェクト</param>
        public void DownloadWebImage(string searchWord, IProgressNotifier progress)
        {
            logger.Info("検索中...");
            progress.Start();
            progress.Progress(0);

            var skip = 0;
            var count = 50;

            disposable = WebImageHelper.SearchImageAsObservable(searchWord, bingAccountKey, skip, count)
                .SelectMany(async bingResult =>
                {
                    var image = new WebImage(bingResult,logger);
                    try
                    {
                        await image.DownLoadThumbnailAsync();
                        return image;
                    }
                    catch (Exception e)
                    {
                        logger.Warn(string.Format("サムネイル画像のダウンロードに失敗しました。[{0}]", bingResult.Thumbnail.MediaUrl), e);
                        return null;
                    }
                })
                .Select((image, n) => new { image, percentProgress = (double)(n+1)/count * 100 })
                .Where(a => a.image != null)
                .Finally(()=>progress.End())
                .Subscribe(
                    onNext: a =>
                    {
                        imagesSource.Add(a.image);
                        progress.Progress(a.percentProgress);
                    },
                    onError: e =>
                    {
                        logger.Error("画像の検索に失敗しました。",e);
                    },
                    onCompleted: () =>
                    {   
                        logger.Info("検索が完了しました。");
                        progress.Progress(100);
                    });
        }

        /// <summary>
        /// リソースを解放します
        /// </summary>
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
                Cancel();
                Clear();
            }
            disposed = true;
        }

        private bool disposed = false;
    }
}
