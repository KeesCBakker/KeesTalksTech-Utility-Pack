using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeesTalksTech.Utilities.Graphics
{
    /// <summary>
    /// Meta information for a meta file (EMF, WMF).
    /// </summary>
    public class MetafileMeta
    {
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the horizontal DPI.
        /// </summary>
        /// <value>
        /// The DPI.
        /// </value>
        public float DpiX { get; private set; }

        /// <summary>
        /// Gets the vertical DPI.
        /// </summary>
        /// <value>
        /// The DPI.
        /// </value>
        public float DpiY { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetafileMeta"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public MetafileMeta(Metafile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            Width = file.Width;
            Height = file.Height;

            var header = file.GetMetafileHeader();

            DpiX = header.DpiX;
            DpiY = header.DpiY;
        }

        /// <summary>
        /// Gets the bounding box with dpi correction.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <returns>The bounding box.</returns>
        public BoundingBox GetBoundingBoxWithDpiCorrection(uint scale = 1)
        {
            var width = (Width * scale / DpiX * 100);
            var height = (Height * scale / DpiY * 100);

            return new BoundingBox(width, height);
        }
    }
}