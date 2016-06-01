using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace KeesTalksTech.Utilities.Graphics
{
    /// <summary>
    /// Utility methods for working with meta files (WMF, EMF).
    /// </summary>
    public static class MetafileUtility
    {
        /// <summary>
        /// Saves the meta file.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="scale">The scale. Default value is 4.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="format">The format. Default is PNG.</param>
        /// <param name="parameters">The parameters.</param>
        public static void SaveMetaFile(
            Stream source,
            Stream destination,
            float scale = 4f,
            Color? backgroundColor = null,
            ImageFormat format = null,
            EncoderParameters parameters = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            using (var img = new Metafile(source))
            {
                var f = format ?? ImageFormat.Png;

                //Determine default background color. Not all formats support transparency. 
                if (backgroundColor == null)
                {
                    var transparentFormats = new ImageFormat[] { ImageFormat.Gif, ImageFormat.Png, ImageFormat.Wmf, ImageFormat.Emf };
                    var isTransparentFormat = transparentFormats.Contains(f);

                    backgroundColor = isTransparentFormat ? Color.Transparent : Color.White;
                }

                var header = img.GetMetafileHeader();
                var width = (int)Math.Round((scale * img.Width / header.DpiX * 100), 0, MidpointRounding.ToEven);
                var height = (int)Math.Round((scale * img.Height / header.DpiY * 100), 0, MidpointRounding.ToEven);

                using (var bitmap = new Bitmap(width, height))
                {
                    using (var g = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        g.Clear(backgroundColor.Value);
                        g.DrawImage(img, 0, 0, bitmap.Width, bitmap.Height);
                    }

                    //get codec based on GUID
                    var codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == f.Guid);

                    bitmap.Save(destination, codec, parameters);
                }
            }
        }

        /// <summary>
        /// Saves the meta file.
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <param name="scale">The scale. Default value is 4.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="format">The format. Default is PNG.</param>
        /// <param name="parameters">The parameters.</param>
        public static void SaveMetaFile(
            string sourceFilePath,
            string destinationFilePath,
            float scale = 4f,
            Color? backgroundColor = null,
            ImageFormat format = null,
            EncoderParameters parameters = null)
        {
            using (var destination = File.OpenWrite(destinationFilePath))
            {
                using (var stream = File.OpenRead(sourceFilePath))
                {
                    SaveMetaFile(stream, destination, scale, backgroundColor, format);
                }
            }
        }
    }
}