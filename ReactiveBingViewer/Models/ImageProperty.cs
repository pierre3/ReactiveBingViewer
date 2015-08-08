using System.Linq;
using System.Text;
using Microsoft.ProjectOxford.Vision.Contract;

namespace ReactiveBingViewer.Models
{
    /// <summary>
    /// 画像プロパティ
    /// </summary>
    public class ImageProperty : AnalysisResult
    {
        /// <summary>
        /// Vison API 解析結果
        /// </summary>
        public AnalysisResult AnalysisData { get; private set; }
        /// <summary>
        /// 画像のURL
        /// </summary>
        public string MediaUrl { get; private set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sourceUrl">画像URL</param>
        /// <param name="source">Vision API 解析結果</param>
        public ImageProperty(string sourceUrl, AnalysisResult source = null)
        {
            this.AnalysisData = source;
            this.MediaUrl = sourceUrl;
        }

        /// <summary>
        /// このオブジェクトの文字列表現を取得します
        /// </summary>
        /// <returns>画像の解析結果の文字列(手抜き)</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Media URL : {0}", MediaUrl);
            sb.AppendLine("");
            sb.AppendLine("");

            if (AnalysisData == null)
            {

                return string.Empty;
            }

            sb.AppendFormat("Adoult : {0} (score = {1})", AnalysisData.Adult.IsAdultContent, AnalysisData.Adult.AdultScore);
            sb.AppendLine("");
            sb.AppendFormat("Racy : {0} (score = {1})", AnalysisData.Adult.IsRacyContent, AnalysisData.Adult.RacyScore);
            sb.AppendLine("");
            if (AnalysisData.Categories == null)
            {
                sb.AppendFormat("Categories : {0}", "None");
            }
            else
            {
                sb.AppendFormat("Categories : {0}", string.Join(", ", AnalysisData.Categories.Select(c => c.Name)));
            }
            sb.AppendLine("");
            sb.AppendFormat("DominantColors : {0}", string.Join(", ", AnalysisData.Color.DominantColors.AsEnumerable()));
            sb.AppendLine("");
            sb.AppendFormat("DominantColorForeground : {0}", AnalysisData.Color.DominantColorForeground);
            sb.AppendLine("");
            sb.AppendFormat("DominantColorBackground : {0}", AnalysisData.Color.DominantColorBackground);
            sb.AppendLine("");
            sb.AppendFormat("AccentColor : {0}", AnalysisData.Color.AccentColor);
            sb.AppendLine("");
            sb.AppendFormat("MetaData : [{0}:({1}×{2})]", AnalysisData.Metadata.Format, AnalysisData.Metadata.Width, AnalysisData.Metadata.Height);
            sb.AppendLine("");

            return sb.ToString();
        }
    }
}
