namespace KeesTalksTech.Utilities.Graphics
{
    using System;

    /// <summary>
    /// Calculates a bounding box.
    /// </summary>
    public class BoundingBox
    {
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public float Width { get; private set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public float Height { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingBox"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public BoundingBox(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Scales the boundingbox using the specified scale.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <returns>A new bounding box.</returns>
        public BoundingBox Scale(float scale)
        {
            return new BoundingBox(Width * scale, Height * scale);
        }

        /// <summary>
        /// Calculates the new bounding box based on the max width and max height.
        /// </summary>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <returns>A new bounding box.</returns>
        public BoundingBox Calculate(float maxWidth, float maxHeight)
        {
            var resolution = Width / Height;
            var maxResolution = maxWidth / maxHeight;

            if (resolution == maxResolution)
            {
                return new BoundingBox(maxWidth, maxHeight);
            }

            if (maxResolution == 1)
            {
                var diff = maxWidth / Math.Max(Width, Height);
                return new BoundingBox(
                    Width * diff,
                    Height * diff
                );
            }

            if (resolution == 1)
            {
                var min = Math.Min(maxWidth, maxHeight);
                var diff = min / Width;

                return new BoundingBox(
                    Width * diff,
                    Height * diff
                );
            }

            var h = (maxWidth / Width) * Height;
            if (h <= maxHeight)
            {
                return new BoundingBox(maxWidth, h);
            }

            var w = (maxHeight / Height) * Width;
            return new BoundingBox(w, maxHeight);
        }
    }
}
