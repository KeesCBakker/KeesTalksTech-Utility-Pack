namespace KeesTalksTech.Utilities.Graphics
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Utility methods for working with meta files (WMF, EMF).
    /// </summary>
    public static class MetafileUtility
    {
        /// <summary>
        /// Gets the metafile meta data.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The meta data.</returns>
        public static MetafileMeta GetMetafileMetaData(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                return GetMetafileMetaData(stream);
            }
        }

        /// <summary>
        /// Gets the metafile meta data.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The meta data.</returns>
        public static MetafileMeta GetMetafileMetaData(Stream stream)
        {
            var p = stream.Position;
            stream.Position = 0;

            try
            {
                using (var img = new Metafile(stream))
                {
                    return new MetafileMeta(img);
                }
            }
            finally
            {
                stream.Position = p;
            }
        }

        /// <summary>
        /// Saves the meta file.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="box">The box. If no box is specified a 4x scale of the original will be returned.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="format">The format. Default is PNG.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public static void SaveMetaFile(
            Stream source,
            Stream destination,
            BoundingBox box = null,
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

                //Determine default background color. 
                //Not all formats support transparency. 
                if (backgroundColor == null)
                {
                    backgroundColor = GetDefaultBackgroundColor(f);
                }

                //header contains DPI information
                var header = img.GetMetafileHeader();

                if (box == null)
                {
                    box = new MetafileMeta(img).GetBoundingBoxWithDpiCorrection(4);
                }

                var width = (int)Math.Round(box.Width, 0, MidpointRounding.ToEven);
                var height = (int)Math.Round(box.Height, 0, MidpointRounding.ToEven);

                using (var bitmap = new Bitmap(width, height))
                {
                    using (var g = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        //fills the background
                        g.Clear(backgroundColor.Value);

                        //reuse the width and height to draw the image
                        //in 100% of the square of the bitmap
                        g.DrawImage(img, 0, 0, bitmap.Width, bitmap.Height);
                    }

                    //get codec based on GUID
                    ImageCodecInfo codec = GetCodec(f);

                    bitmap.Save(destination, codec, parameters);
                }
            }
        }

        /// <summary>
        /// Saves the meta file.
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <param name="box">The box. If no box is specified a 4x scale of the original will be returned.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="format">The format. Default is PNG.</param>
        /// <param name="parameters">The parameters.</param>
        public static void SaveMetaFile(
            string sourceFilePath,
            string destinationFilePath,
            BoundingBox box = null,
            Color? backgroundColor = null,
            ImageFormat format = null,
            EncoderParameters parameters = null)
        {
            using (var destination = File.OpenWrite(destinationFilePath))
            {
                using (var stream = File.OpenRead(sourceFilePath))
                {
                    SaveMetaFile(stream, destination, box, backgroundColor, format);
                }
            }
        }

        /// <summary>
        /// Saves the meta file using two stages. The meta file will be converted to 4x 
        /// its size PNG before it is converted to the target format.
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        /// <param name="box">The box.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="format">The format.</param>
        /// <param name="parameters">The parameters.</param>
        public static void SaveMetaFileUsingTwoStages(
                            string sourceFilePath,
            string destinationFilePath,
            BoundingBox box = null,
            Color? backgroundColor = null,
            ImageFormat format = null,
            EncoderParameters parameters = null)
        {
            using (var source = File.OpenRead(sourceFilePath))
            {
                using (var destination = File.OpenWrite(destinationFilePath))
                {
                    SaveMetaFileUsingTwoStages(source, destination, box, backgroundColor, format, parameters);
                }
            }
        }


        /// <summary>
        /// Saves the meta file using two stages. The meta file will be converted to 4x
        /// its size PNG before it is converted to the target format.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="box">The box.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="format">The format.</param>
        /// <param name="parameters">The parameters.</param>
        public static void SaveMetaFileUsingTwoStages(
            Stream source,
            Stream destination,
            BoundingBox box = null,
            Color? backgroundColor = null,
            ImageFormat format = null,
            EncoderParameters parameters = null)
        {

            using (var png = new MemoryStream())
            {
                BoundingBox pngBox;
                if (box == null)
                {
                    box = GetMetafileMetaData(source).GetBoundingBoxWithDpiCorrection(4);
                    pngBox = box;
                }
                else
                {
                    pngBox = box.Scale(4);
                }

                //safe PNG - default settings
                SaveMetaFile(source, png);

                png.Position = 0;

                using (var pngImage = Image.FromStream(png))
                {
                    using (var targetImage = new Bitmap((int)box.Width, (int)box.Height))
                    {
                        var f = format ?? ImageFormat.Png;

                        using (var g = System.Drawing.Graphics.FromImage(targetImage))
                        {
                            if (backgroundColor == null)
                            {
                                backgroundColor = GetDefaultBackgroundColor(f);
                            }

                            g.Clear(backgroundColor.Value);
                            g.DrawImage(pngImage, 0, 0, targetImage.Width, targetImage.Height);
                        }

                        var codec = GetCodec(f);
                        targetImage.Save(destination, codec, parameters);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the codec.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The codec or <c>null</c>.</returns>
        private static ImageCodecInfo GetCodec(ImageFormat format)
        {
            return ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == format.Guid);
        }

        /// <summary>
        /// Gets the default color of the background.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The color.</returns>
        private static Color? GetDefaultBackgroundColor(ImageFormat format)
        {
            Color? backgroundColor;
            var transparentFormats = new ImageFormat[] { ImageFormat.Gif, ImageFormat.Png, ImageFormat.Wmf, ImageFormat.Emf };
            var isTransparentFormat = transparentFormats.Contains(format);

            backgroundColor = isTransparentFormat ? Color.Transparent : Color.White;
            return backgroundColor;
        }
    }
}