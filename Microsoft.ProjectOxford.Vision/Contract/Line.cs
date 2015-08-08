// *********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
//
// *********************************************************

namespace Microsoft.ProjectOxford.Vision.Contract
{
    using Newtonsoft.Json;

    /// <summary>
    /// The class for Ocr line.
    /// </summary>
    public class Line
    {
        /// <summary>
        /// Gets or sets the bounding box.
        /// </summary>
        /// <value>
        /// The bounding box.
        /// </value>
        public string BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the words.
        /// </summary>
        /// <value>
        /// The words.
        /// </value>
        public Word[] Words { get; set; }

        /// <summary>
        /// Gets the rectangle.
        /// </summary>
        /// <value>
        /// The rectangle.
        /// </value>
        [JsonIgnore]
        public Rectangle Rectangle 
        {
            get
            {
                return Rectangle.FromString(this.BoundingBox);
            }
        }
    }
}
