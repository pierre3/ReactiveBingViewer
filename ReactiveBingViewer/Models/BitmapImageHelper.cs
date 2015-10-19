using ReactiveBingViewer.IO;
using System;
using System.IO;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ReactiveBingViewer.Models
{
    /// <summary>
    /// BitpamImageの生成を補助するクラス
    /// </summary>
    public static class BitmapImageHelper
    {
        /// <summary>
        /// 指定したURLからダウンロードした画像データを使用して、指定したDispatcher上でBitmapImageを生成します
        /// </summary>
        /// <param name="url">画像URL</param>
        /// <param name="dispatcherForBitmapCreation">BitmapImageを生成するDispatcher</param>
        /// <returns>BitmapImage</returns>
        public static async Task<BitmapImage> DownloadImageAsync(string url, Dispatcher dispatcherForBitmapCreation)
        {
            var bytes = await DownLoadImageBytesAsync(url).ConfigureAwait(false);
            return await dispatcherForBitmapCreation.InvokeAsync(() => CreateBitmap(bytes));
        }

        /// <summary>
        /// 指定したURLから画像データをByte配列で取得します
        /// </summary>
        /// <param name="url">画像URL</param>
        /// <returns>画像データ</returns>
        public static async Task<byte[]> DownLoadImageBytesAsync(string url)
        {
            using (var web = new HttpClient())
            {
                return await web.GetByteArrayAsync(url).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Byte配列の画像データからBitmapImageを生成します
        /// </summary>
        /// <param name="bytes">画像データ</param>
        /// <param name="freezing">生成したBitmapImageをFreezeする場合Trueを指定</param>
        /// <returns>BitmapImage</returns>
        public static BitmapImage CreateBitmap(byte[] bytes, bool freezing = true)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new WrappingStream(stream);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                if (freezing && bitmap.CanFreeze)
                { bitmap.Freeze(); }
                return bitmap;
            }
        }

        /// <summary>
        /// 画像URLからBitmapImageを生成するIObservable<T>を取得します
        /// </summary>
        /// <param name="uri">画像URL</param>
        /// <returns>BitmapImageの生成を監視するIObservable<T></returns>
        public static IObservable<BitmapImage> CreateBitmapAsObservable(Uri uri)
        {
            return Observable.Create<BitmapImage>(observer =>
            {
                EventHandler completed = (obj, __) =>
                {
                    var bi = (BitmapImage)obj;
                    observer.OnNext(bi);
                    observer.OnCompleted();
                };
                EventHandler<ExceptionEventArgs> failed = (_, e) =>
                {
                    observer.OnError(e.ErrorException);
                };
                var bitmap = new BitmapImage();
                try
                {
                    bitmap.BeginInit();
                    bitmap.UriSource = uri;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.DownloadCompleted += completed;
                    bitmap.DownloadFailed += failed;
                    bitmap.EndInit();
                    if (!bitmap.IsDownloading)
                    {
                        observer.OnNext(bitmap);
                        observer.OnCompleted();
                    }
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                return ()=> {
                    bitmap.DownloadCompleted -= completed;
                    bitmap.DownloadFailed -= failed;
                    bitmap.Freeze();
                };

            });
        }
    }

}
