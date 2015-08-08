// *********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
//
// *********************************************************

namespace Microsoft.ProjectOxford.Vision.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The class for color.
    /// </summary>
    public class Color
    {
        /// <summary>
        /// Gets or sets the color of the accent.
        /// </summary>
        /// <value>
        /// The color of the accent.
        /// </value>
        public string AccentColor { get; set; }

        /// <summary>
        /// Gets or sets the dominant color foreground.
        /// </summary>
        /// <value>
        /// The dominant color foreground.
        /// </value>
        public string DominantColorForeground { get; set; }

        /// <summary>
        /// Gets or sets the dominant color background.
        /// </summary>
        /// <value>
        /// The dominant color background.
        /// </value>
        public string DominantColorBackground { get; set; }

        /// <summary>
        /// Gets or sets the dominant colors.
        /// </summary>
        /// <value>
        /// The dominant colors.
        /// </value>
        public string[] DominantColors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is binary image.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is binary image; otherwise, <c>false</c>.
        /// </value>
        public bool IsBWImg { get; set; }
    }
}
