using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using ReactiveBingViewer.IO;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Reactive.Threading.Tasks;
using System.Reactive.Concurrency;

namespace ReactiveBingViewer.Models
{
    /// <summary>
    /// WebImage 用 ヘルパークラス
    /// </summary>
    internal class WebImageHelper
    {
        /// <summary>
        /// Bing 画像検索を行い、取得した画像URLのシーケンスをIObservableとして返却します
        /// </summary>
        /// <param name="searchWord">検索文字列</param>
        /// <param name="accountKey">Bing Search アカウントキー</param>
        /// <param name="skip">スキップする検索結果の件数</param>
        /// <param name="top">取得する検索結果の件数</param>
        /// <returns>画像検索結果</returns>
        public static IObservable<Bing.ImageResult> SearchImageAsObservable(string searchWord, string accountKey, int skip, int top)
        {
            var bing = new Bing.BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/"));
            bing.Credentials = new NetworkCredential("accountKey", accountKey);
            try
            {
                var result = bing.Image(searchWord, null, null, null, null, null, null, skip, top).Execute();
                return result.ToObservable();
            }
            catch (Exception e)
            {
                return Observable.Throw<Bing.ImageResult>(e);
            }
        }

        /// <summary>
        /// Microsoft ProjectOxford Vision API を使用して画像の解析を行います
        /// </summary>
        /// <param name="imageUri">画像URL</param>
        /// <returns>解析結果を格納したAnalysisResultオブジェクト</returns>
        public async static Task<AnalysisResult> AnalyzeImageAsync(Uri imageUri, string subscriptionKey)
        {
            var visionClient = new VisionServiceClient(subscriptionKey);
            return await visionClient.AnalyzeImageAsync(imageUri.AbsoluteUri).ConfigureAwait(false);
        }

        /// <summary>
        /// Webから画像データをダウンロードしてBitmapImageを作成します
        /// </summary>
        /// <param name="url">画像URL</param>
        /// <returns>BitmapImage</returns>
        public static async Task<BitmapImage> DownLoadImageAsync(string url)
        {
            using (var web = new HttpClient())
            {
                var bytes = await web.GetByteArrayAsync(url).ConfigureAwait(false);
                using (var stream = new WrappingStream(new MemoryStream(bytes)))
                {
                    return await CreateBitmap(stream, Reactive.Bindings.UIDispatcherScheduler.Default);
                }
            }
        }

        /// <summary>
        /// 指定したSchedulerで、ストリームからBitmapImageを作成します。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        /// <param name="scheduler">Scheduler</param>
        /// <returns>BitmapImage</returns>
        public static Task<BitmapImage> CreateBitmap(Stream stream, IScheduler scheduler)
        {
            return Observable.Start(() =>
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                //bitmap.Freeze();
                return bitmap;
            }, scheduler).ToTask();
        }

    }
}
