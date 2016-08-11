using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace KeesTalksTech.Utilities.IO
{
    [TestClass]
    public class StreamUtilityTest
    {
        [TestMethod]
        public void StreamUtilityStreamEquals_StringStreams_True()
        {
            string s1 = "ABCDE", s2 = "ABCDE";

            MemoryStream ms1 = new MemoryStream(Encoding.UTF8.GetBytes(s1));
            MemoryStream ms2 = new MemoryStream(Encoding.UTF8.GetBytes(s2));

            var result = StreamUtility.Equals(ms1, ms2);

            Assert.IsTrue(result, "Streams should be equal.");
        }

        [TestMethod]
        public void StreamUtilityStreamEquals_StringStreams_False()
        {
            string s1 = "ABCDE", s2 = "12345";

            MemoryStream ms1 = new MemoryStream(Encoding.UTF8.GetBytes(s1));
            MemoryStream ms2 = new MemoryStream(Encoding.UTF8.GetBytes(s2));

            var result = StreamUtility.Equals(ms1, ms2);

            Assert.IsFalse(result, "Streams should not be equal.");
        }
    }
}
