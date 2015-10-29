using Reactive.Bindings;
using ReactiveBingViewer.Notifiers;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

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
        /// <param name="page">ページ番号</param>
        /// <param name="imageCountPerPage">ページ当たりの画像数</param>
        public void DownloadSearchResult(string searchWord, IProgressNotifier progress, int page, int imageCountPerPage)
        {
            if (imageCountPerPage < 10)
            {
                throw new ArgumentException(nameof(imageCountPerPage) + " must be 10 or over.", nameof(imageCountPerPage));
            }

            logger.Info("検索中...");
            progress.Start();
            progress.Progress(0);

            var skip = (page - 1) * imageCountPerPage;

            disposable = ServiceClient.SearchImageAsObservable(searchWord, bingAccountKey, skip, imageCountPerPage, ThreadPoolScheduler.Instance)
                .SelectMany(async imageResult => new {
                    imageResult,
                    imageBytes = await DownloadThumbnailImageBytesAsync(imageResult)
                })
                .Where(a => a.imageBytes != null)
                .Select((a,index)=> new{imageResult = a.imageResult, imageBytes = a.imageBytes, index })
                .ObserveOn(UIDispatcherScheduler.Default)
                .Finally(() => progress.End())
                .Subscribe(
                    onNext: a =>
                    {
                        var thumbnail = BitmapImageHelper.CreateBitmap(a.imageBytes);
                        imagesSource.Add(new WebImage(a.imageResult, thumbnail, logger));
                        progress.Progress((double)(a.index + 1) / imageCountPerPage * 100);
                    },
                    onError: e =>
                    {
                        logger.Error("画像の検索に失敗しました。", e);
                    },
                    onCompleted: () =>
                    {
                        logger.Info("検索が完了しました。");
                        progress.Progress(100);
                    });
        }
        
        /// <summary>
        /// Bing画像検索結果の画像情報に含まれるサムネイル画像のURLから画像データをダウンロードする
        /// </summary>
        /// <param name="imageResult">画像検索結果</param>
        /// <returns>画像データのByte配列</returns>
        private async Task<byte[]> DownloadThumbnailImageBytesAsync(Bing.ImageResult imageResult)
        {
            try
            {
                return await BitmapImageHelper.DownLoadImageBytesAsync(imageResult.Thumbnail.MediaUrl);
            }
            catch (Exception e)
            {
                logger.Warn("サムネイル画像のダウンロードに失敗しました。", e);
                return null;
            }
        }

        /// <summary>
        /// リソースを解放します
        /// </summary>
        public void Dispose()
        {
            Cancel();
            Clear();
        }

    }
}
