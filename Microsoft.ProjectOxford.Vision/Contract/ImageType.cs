// *********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
//
// *********************************************************

namespace Microsoft.ProjectOxford.Vision.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Image Type
    /// </summary>
    public class ImageType
    {
        /// <summary>
        /// Gets or sets the type of the clip art.
        /// </summary>
        /// <value>
        /// The type of the clip art.
        /// </value>
        public int ClipArtType { get; set; }

        /// <summary>
        /// Gets or sets the type of the line drawing.
        /// </summary>
        /// <value>
        /// The type of the line drawing.
        /// </value>
        public int LineDrawingType { get; set; }
    }
}
