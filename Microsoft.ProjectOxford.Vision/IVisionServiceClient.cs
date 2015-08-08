// *********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
//
// *********************************************************

namespace Microsoft.ProjectOxford.Vision
{
    using Microsoft.ProjectOxford.Vision.Contract;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Vision service client interfaces.
    /// </summary>
    public interface IVisionServiceClient
    {
        /// <summary>
        /// Analyzes the image.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="visualFeatures">The visual features.</param>
        /// <returns>The AnalysisResult object.</returns>
        Task<AnalysisResult> AnalyzeImageAsync(string url, string[] visualFeatures = null);

        /// <summary>
        /// Analyzes the image.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="visualFeatures">The visual features.</param>
        /// <returns>The AnalysisResult object.</returns>
        Task<AnalysisResult> AnalyzeImageAsync(Stream imageStream, string[] visualFeatures = null);

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="smartCropping">if set to <c>true</c> [smart cropping].</param>
        /// <returns>The byte array.</returns>
        Task<byte[]> GetThumbnailAsync(string url, int width, int height, bool smartCropping = true);

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="smartCropping">if set to <c>true</c> [smart cropping].</param>
        /// <returns>The byte array.</returns>
        Task<byte[]> GetThumbnailAsync(Stream stream, int width, int height, bool smartCropping = true);

        /// <summary>
        /// Recognizes the text asynchronous.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="detectOrientation">if set to <c>true</c> [detect orientation].</param>
        /// <returns>The OCR object.</returns>
        Task<OcrResults> RecognizeTextAsync(string imageUrl, string languageCode = LanguageCodes.AutoDetect, bool detectOrientation = true);

        /// <summary>
        /// Recognizes the text asynchronous.
        /// </summary>
        /// <param name="imageStream">The image stream.</param>
        /// <param name="languageCode">The language code.</param>
        /// <param name="detectOrientation">if set to <c>true</c> [detect orientation].</param>
        /// <returns>The OCR object.</returns>
        Task<OcrResults> RecognizeTextAsync(Stream imageStream, string languageCode = LanguageCodes.AutoDetect, bool detectOrientation = true);
    }
}
