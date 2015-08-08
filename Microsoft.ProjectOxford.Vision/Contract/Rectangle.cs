// *********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
//
// *********************************************************

namespace Microsoft.ProjectOxford.Vision.Contract
{
    /// <summary>
    /// The class for rectangle.
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left of the face rectangle.
        /// </value>
        public int Left { get; set; }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top of the face rectangle.
        /// </value>
        public int Top { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Froms the string.
        /// </summary>
        /// <param name="string">The string.</param>
        /// <returns>The Rectangle.</returns>
        public static Rectangle FromString(string @string)
        {
            if (!string.IsNullOrWhiteSpace(@string))
            {
                var box = @string.Split(',');
                int left, top, width, height;

                if (box.Length == 4)
                {
                    if (int.TryParse(box[0], out left) &&
                        int.TryParse(box[1], out top) &&
                        int.TryParse(box[2], out width) &&
                        int.TryParse(box[3], out height))
                    {
                        return new Rectangle()
                        {
                            Left = left,
                            Height = height,
                            Top = top,
                            Width = width
                        };
                    }
                }
            }

            return null;
        }
    }
}
