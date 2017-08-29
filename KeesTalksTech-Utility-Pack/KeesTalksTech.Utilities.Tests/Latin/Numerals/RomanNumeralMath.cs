namespace KeesTalksTech.Utilities.Latin.Numerals
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RomanNumeralMath
    {
        [TestCategory("UnitTest")]
        [TestMethod]
        public void RomanNumeral_Add_Int()
        {
            var x = new RomanNumeral(0) + 4;
            Assert.AreEqual(4, x.Number);
        }

        [TestCategory("UnitTest")]
        [TestMethod]
        public void RomanNumeral_Add_String()
        {
            var x = new RomanNumeral(0) + "IV";
            Assert.AreEqual(4, x.Number);
        }

        [TestCategory("UnitTest")]
        [TestMethod]
        public void RomanNumeral_Add_IntAndString()
        {
            var x = new RomanNumeral(0) + 4 + "IV";
            Assert.AreEqual(8, x.Number);
        }

        [TestCategory("UnitTest")]
        [TestMethod]
        public void RomanNumeral_Add_StringAndInt()
        {
            var x = new RomanNumeral(0) + "IV" + 4;
            Assert.AreEqual(8, x.Number);
        }

        [TestCategory("UnitTest")]
        [TestMethod]
        public void RomanNumeral_Assign_String()
        {
            RomanNumeral x = "IV";
            Assert.AreEqual(4, x.Number);
        }

        [TestCategory("UnitTest")]
        [TestMethod]
        public void RomanNumeral_Assign_Int()
        {
            RomanNumeral x = 4;
            Assert.AreEqual(4, x.Number);
        }

        [TestCategory("UnitTest")]
        [TestMethod]
        public void RomanNumeral_NumeralAddString_ToString()
        {
            RomanNumeral x = "IV";
            string result = x + "IV";

            Assert.AreEqual("VIII", result);
        }
    }
}