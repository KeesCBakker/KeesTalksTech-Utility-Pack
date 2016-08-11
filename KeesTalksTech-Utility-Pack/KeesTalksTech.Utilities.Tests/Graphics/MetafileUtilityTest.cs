using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;
using KeesTalksTech.Utilities.IO;
using System.Drawing.Imaging;
using System.Linq;
using System.Drawing;

namespace KeesTalksTech.Utilities.Graphics
{
    public class MetafileUtilityTest
    {
        public void MetafileUtilitySaveMetaFile_EMF_PNG()
        {
            using (var emf = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.UnitTests.Resources.MetafileUtility_EMF.emf"))
            {
                using (var test = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.UnitTests.Resources.MetafileUtility_EMF_PNG.png"))
                {
                    using (var converted = new MemoryStream())
                    {
                        MetafileUtility.SaveMetaFile(emf, converted);

                        var equals = StreamUtility.Equals(test, converted);
                        Assert.IsTrue(equals, "Streams are not equal.");
                    }
                }
            }
        }

        public void MetafileUtilitySaveMetaFile_EMF_JPG()
        {
            using (var emf = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.UnitTests.Resources.MetafileUtility_EMF.emf"))
            {
                using (var test = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.UnitTests.Resources.MetafileUtility_EMF_JPG.jpg"))
                {
                    using (var converted = new MemoryStream())
                    {
                        var parameters = new EncoderParameters(1);
                        parameters.Param[0] = new EncoderParameter(Encoder.Quality, 80L);

                        MetafileUtility.SaveMetaFile(emf, converted, format: ImageFormat.Jpeg, parameters: parameters);

                        var equals = StreamUtility.Equals(test, converted);
                        Assert.IsTrue(equals, "Streams are not equal.");
                    }
                }
            }
        }

        public void MetafileUtilitySaveMetaFile_EMF_JPG300x300()
        {
            using (var emf = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.Resources.MetafileUtility_EMF.emf"))
            {
                using (var test = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.Resources.MetafileUtility_EMF_JPG.jpg"))
                {
                    using (var converted = new MemoryStream())
                    {
                        var parameters = new EncoderParameters(1);
                        parameters.Param[0] = new EncoderParameter(Encoder.Quality, 80L);

                        var box = MetafileUtility.GetMetafileMetaData(emf).GetBoundingBoxWithDpiCorrection().Calculate(300, 300);

                        MetafileUtility.SaveMetaFile(emf, converted, box, format: ImageFormat.Jpeg, parameters: parameters);

                        //var equals = StreamUtility.Equals(test, converted);
                        //Assert.IsTrue(equals, "Streams are not equal.");
                        using (var f = File.OpenWrite(@"c:\temp\k1.jpg"))
                        {
                            converted.Position = 0;
                            converted.CopyTo(f);
                        }
                    }
                }
            }
        }

        public void MetafileUtilitySaveMetaFileUsingTwoStages_EMF_JPG300x300()
        {
            using (var emf = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.Resources.MetafileUtility_EMF.emf"))
            {
                using (var test = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.Resources.MetafileUtility_EMF_JPG.jpg"))
                {
                    using (var converted = new MemoryStream())
                    {
                        var parameters = new EncoderParameters(1);
                        parameters.Param[0] = new EncoderParameter(Encoder.Quality, 80L);

                        var box = MetafileUtility.GetMetafileMetaData(emf).GetBoundingBoxWithDpiCorrection().Calculate(300, 300);

                        MetafileUtility.SaveMetaFileUsingTwoStages(emf, converted, box, format: ImageFormat.Jpeg, parameters: parameters);

                        //var equals = StreamUtility.Equals(test, converted);
                        //Assert.IsTrue(equals, "Streams are not equal.");
                        using (var f = File.OpenWrite(@"c:\temp\k3.jpg"))
                        {
                            converted.Position = 0;
                            converted.CopyTo(f);
                        }
                    }
                }
            }
        }
    }
}
