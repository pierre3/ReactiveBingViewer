using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ReactiveBingViewer.Models
{
    /// <summary>
    /// WebImage 用 ヘルパークラス
    /// </summary>
    internal static class ServiceClient
    {
        /// <summary>
        /// Bing 画像検索を行い、取得した画像URLのシーケンスをIObservableとして返却します
        /// </summary>
        /// <param name="searchWord">検索文字列</param>
        /// <param name="accountKey">Bing Search アカウントキー</param>
        /// <param name="skip">スキップする検索結果の件数</param>
        /// <param name="top">取得する検索結果の件数</param>
        /// <returns>画像検索結果</returns>
        public static IObservable<Bing.ImageResult> SearchImageAsObservable(string searchWord, string accountKey, int skip, int top,IScheduler scheduler)
        {
            return Observable.Start(() =>
            {
                var bing = new Bing.BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/"));
                bing.Credentials = new NetworkCredential("accountKey", accountKey);
                var query = bing.Image(searchWord, null, null, null, null, null, null);
                query = query.AddQueryOption("$skip", skip);
                query = query.AddQueryOption("$top", top);
                return query.Execute();
            },scheduler).SelectMany(ie => ie);
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

        
    }
}
