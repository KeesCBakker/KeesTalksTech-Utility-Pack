using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;
using KeesTalksTech.Utilities.IO;
using System.Drawing.Imaging;
using System.Linq;

namespace KeesTalksTech.Utilities.Graphics
{
    [TestClass]
    public class MetafileUtilityTest
    {
        [TestMethod]
        public void MetafileUtility_EMF_PNG()
        {
            using (var emf = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.Resources.MetafileUtility_EMF.emf"))
            {
                using (var test = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.Resources.MetafileUtility_EMF_PNG.png"))
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

        [TestMethod]
        public void MetafileUtility_EMF_JPG()
        {
            using (var emf = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.Resources.MetafileUtility_EMF.emf"))
            {
                using (var test = typeof(MetafileUtilityTest).Assembly.GetManifestResourceStream("KeesTalksTech.Utilities.Resources.MetafileUtility_EMF_JPG.jpg"))
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
    }
}
