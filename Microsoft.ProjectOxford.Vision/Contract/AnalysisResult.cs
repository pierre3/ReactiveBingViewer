// *********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
//
// *********************************************************

namespace Microsoft.ProjectOxford.Vision.Contract
{
    using System;

    /// <summary>
    /// The class for analysis result.
    /// </summary>
    public class AnalysisResult
    {
        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        /// <value>
        /// The request identifier.
        /// </value>
        public Guid RequestId { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public Metadata Metadata { get; set; }

        /// <summary>
        /// Gets or sets the type of the image.
        /// </summary>
        /// <value>
        /// The type of the image.
        /// </value>
        public ImageType ImageType { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the adult.
        /// </summary>
        /// <value>
        /// The adult.
        /// </value>
        public Adult Adult { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public Category[] Categories { get; set; }

        /// <summary>
        /// Gets or sets the faces.
        /// </summary>
        /// <value>
        /// The faces.
        /// </value>
        public Face[] Faces { get; set; }
    }
}
