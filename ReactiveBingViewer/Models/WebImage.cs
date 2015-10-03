using Reactive.Bindings;
using ReactiveBingViewer.Notifiers;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ReactiveBingViewer.Models
{
    /// <summary>
    /// Webからダウンロードした画像
    /// </summary>
    public class WebImage : INotifyPropertyChanged
    {
        private ImageSource thumbnail;
        private ImageSource displayImage;
        private ImageSource overlay;
        private Uri mediaUrl;
        private string sourceUrl;
        private string sourceTitle;
        private ImageProperty imageProperty;
        private Bing.ImageResult bingResult;
        private ILogger logger;

        /// <summary>サムネイル用 ImageSource</summary>
        public ImageSource Thumbnail
        {
            get { return thumbnail; }
            private set
            {
                if (thumbnail == value) { return; }
                thumbnail = value;
                RaisePropertyChanged(nameof(Thumbnail));
            }
        }

        /// <summary>表示用 ImageSource</summary>
        public ImageSource DisplayImage
        {
            get { return displayImage; }
            private set
            {
                if (displayImage == value) { return; }
                displayImage = value;
                RaisePropertyChanged(nameof(DisplayImage));
            }
        }

        /// <summary>画像情報を画像に重ねて表示するための ImageSource</summary>
        public ImageSource Overlay
        {
            get { return overlay; }
            private set
            {
                if (overlay == value) { return; }
                overlay = value;
                RaisePropertyChanged(nameof(Overlay));
            }
        }

        /// <summary>画像取得先 URL</summary>
        public Uri MediaUrl
        {
            get { return mediaUrl; }
            private set
            {
                if (mediaUrl == value) { return; }
                mediaUrl = value;
                RaisePropertyChanged(nameof(MediaUrl));
            }
        }

        /// <summary>リンク元ページのURL</summary>
        public string SourceUrl
        {
            get { return sourceUrl; }
            private set
            {
                if (sourceUrl == value) { return; }
                sourceUrl = value;
                RaisePropertyChanged(nameof(SourceUrl));
            }
        }

        /// <summary>リンク元ページのタイトル</summary>
        public string SourceTitle
        {
            get { return sourceTitle; }
            private set
            {
                if (sourceTitle == value) { return; }
                sourceTitle = value;
                RaisePropertyChanged(nameof(SourceTitle));
            }
        }

        /// <summary>Microsoft Vision API による画像解析結果</summary>
        public ImageProperty ImageProperty
        {
            get { return imageProperty; }
            private set
            {
                if (imageProperty == value) { return; }
                imageProperty = value;
                RaisePropertyChanged(nameof(ImageProperty));
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="bingResult">Bing画像検索結果</param>
        /// <param name="logger">Logメッセージ通知オブジェクト</param>
        public WebImage(Bing.ImageResult bingResult, ILogger logger)
        {
            if (bingResult == null) { throw new ArgumentNullException("bingResult"); }

            this.bingResult = bingResult;
            this.MediaUrl = new Uri(bingResult.MediaUrl);
            this.sourceUrl = bingResult.SourceUrl;
            this.sourceTitle = bingResult.Title;
            this.logger = logger ?? new EmptyLogger();
        }

        /// <summary>
        /// サムネイル画像をダウンロードします
        /// </summary>
        /// <returns>Task</returns>
        public async Task DownLoadThumbnailAsync()
        {
            logger.Info(string.Format("サムネイル画像をダウンロードしています...[{0}]", this.MediaUrl.AbsoluteUri));
            try
            {
                this.Thumbnail = await WebImageHelper.DownLoadImageAsync(bingResult.Thumbnail.MediaUrl,
                    UIDispatcherScheduler.Default).ConfigureAwait(false);
                logger.Info(string.Format("サムネイル画像のダウンロードが完了しました。[{0}]", this.MediaUrl.AbsoluteUri));
            }
            catch (Exception e)
            {
                logger.Warn(string.Format("サムネイル画像のダウンロードに失敗しました。[{0}]", this.MediaUrl.AbsoluteUri), e);
                this.Thumbnail = null;
            }
        }

        /// <summary>
        /// フルサイズの画像データをダウンロードします
        /// </summary>
        /// <returns>Task</returns>
        public async Task DownLoadFullImageAsync()
        {
            if (this.DisplayImage != null) { return; }
            logger.Info(string.Format("画像をダウンロードしています...[{0}]", this.MediaUrl.AbsoluteUri));
            try
            {
                this.DisplayImage = await WebImageHelper.DownLoadImageAsync(this.MediaUrl.AbsoluteUri,
                    Application.Current.Dispatcher).ConfigureAwait(false);
                logger.Info(string.Format("画像のダウンロードが完了しました。[{0}]", this.MediaUrl.AbsoluteUri));
            }
            catch (Exception e)
            {
                logger.Warn(string.Format("画像のダウンロードに失敗しました。[{0}]", this.MediaUrl.AbsoluteUri), e);
            }
        }

        /// <summary>
        /// Microsoft.ProjectOxford.Vision API による画像解析を実行します。
        /// </summary>
        /// <param name="subscriptionKey">Vision API のsubscripsion Key</param>
        /// <returns>Task</returns>
        public async Task AnalyzeImageAsync(string subscriptionKey)
        {
            if (ImageProperty == null)
            {
                ImageProperty = new ImageProperty(this.MediaUrl.AbsoluteUri);
            }
            if (ImageProperty.AnalysisData != null) { return; }

            logger.Info(string.Format("画像を解析しています...[{0}]", this.MediaUrl.AbsoluteUri));
            try
            {
                var result = await WebImageHelper.AnalyzeImageAsync(this.MediaUrl, subscriptionKey).ConfigureAwait(false);
                this.ImageProperty = new ImageProperty(this.MediaUrl.AbsoluteUri, result);

                logger.Info(string.Format("画像の解析が完了しました。[{0}]", this.MediaUrl.AbsoluteUri));
            }
            catch (Exception e)
            {
                logger.Warn(string.Format("画像の解析に失敗しました。[{0}]", this.MediaUrl.AbsoluteUri), e);
            }
        }

        /// <summary>
        /// 画像解析APIで取得した顔認識情報(顔領域の矩形、顔認識結果から推定される年齢と性別)を
        /// 画像上に重ねて表示するためのオーバーレイ画像を生成します
        /// </summary>
        /// <param name="imageSize">画像サイズ</param>
        public void DrawFaceRect(Size imageSize)
        {
            if (ImageProperty == null || ImageProperty.AnalysisData == null) { return; }
            if (this.DisplayImage == null) { return; }
            if (this.bingResult.Width == null || this.bingResult.Height == null)
            { return; }

            int sourceWidth = (int)this.bingResult.Width;
            int sourceHeight = (int)this.bingResult.Height;
            if (sourceWidth == 0 || sourceHeight == 0) { return; }


            int height, width;
            if (imageSize.IsEmpty)
            {
                height = sourceHeight;
                width = sourceWidth;
            }
            else
            {
                if (sourceWidth > sourceHeight)
                {
                    height = (int)Math.Truncate(imageSize.Height);
                    width = (int)Math.Truncate(imageSize.Height * (double)sourceWidth / sourceHeight);
                }
                else
                {
                    width = (int)Math.Truncate(imageSize.Width);
                    height = (int)Math.Truncate(imageSize.Width * (double)sourceHeight / sourceWidth);
                }
            }
            if (width <= 0 && height <= 0) { return; }

            var x_ratio = (double)width / sourceWidth;
            var y_ratio = (double)height / sourceHeight;

            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                foreach (var face in ImageProperty.AnalysisData.Faces)
                {
                    var rect = new Rect(
                        face.FaceRectangle.Left * x_ratio,
                        face.FaceRectangle.Top * y_ratio,
                        face.FaceRectangle.Width * x_ratio,
                        face.FaceRectangle.Height * y_ratio);
                    dc.DrawRectangle(
                        Brushes.Transparent,
                        new Pen(Brushes.DarkGray, 1),
                        new Rect(new Point(rect.Left + 1, rect.Top + 1), rect.Size));
                    dc.DrawRectangle(
                        Brushes.Transparent,
                        new Pen(Brushes.White, 1),
                        rect);

                    var text = ((face.Gender == "Female") ? "♀:" : (face.Gender == "Male") ? "♂:" : "?:") + face.Age.ToString();
                    var culture = CultureInfo.CurrentCulture;
                    var flowDir = FlowDirection.LeftToRight;
                    var font = new Typeface("Meiryo");
                    var fontSize = (width < 400) ? 9 : 16;

                    var shadowFormat = new FormattedText(text, culture, flowDir, font, fontSize, Brushes.DarkGray);
                    dc.DrawText(shadowFormat, new Point(rect.Left + 1, rect.Top - shadowFormat.Height + 1));

                    var format = new FormattedText(text, culture, flowDir, font, fontSize, Brushes.OrangeRed);
                    dc.DrawText(format, new Point(rect.Left, rect.Top - format.Height));
                }
            }
            var target = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
            target.Render(visual);
            target.Freeze();

            this.Overlay = target;
        }

        #region INotifyPropertyChanged の実装
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }

}
