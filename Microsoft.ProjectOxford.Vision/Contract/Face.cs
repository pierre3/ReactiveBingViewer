// *********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
//
// *********************************************************

namespace Microsoft.ProjectOxford.Vision.Contract
{
    /// <summary>
    /// The class for face.
    /// </summary>
    public class Face
    {
        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the face rectangle.
        /// </summary>
        /// <value>
        /// The face rectangle.
        /// </value>
        public FaceRectangle FaceRectangle { get; set; }
    }
}
