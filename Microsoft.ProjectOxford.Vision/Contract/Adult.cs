// *********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
//
// *********************************************************

namespace Microsoft.ProjectOxford.Vision.Contract
{
    /// <summary>
    /// The class for adult.
    /// </summary>
    public class Adult
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is adult content.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is adult content; otherwise, <c>false</c>.
        /// </value>
        public bool IsAdultContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is racy content.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is racy content; otherwise, <c>false</c>.
        /// </value>
        public bool IsRacyContent { get; set; }

        /// <summary>
        /// Gets or sets the adult score.
        /// </summary>
        /// <value>
        /// The adult score.
        /// </value>
        public double AdultScore { get; set; }

        /// <summary>
        /// Gets or sets the racy score.
        /// </summary>
        /// <value>
        /// The racy score.
        /// </value>
        public double RacyScore { get; set; }
    }
}
