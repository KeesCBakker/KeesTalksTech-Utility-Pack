using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeesTalksTech.Utilities.Graphics
{
    public class MetafileMeta
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public float DpiX { get; private set; }

        public float DpiY { get; private set; }

        public MetafileMeta(Metafile file)
        {
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
