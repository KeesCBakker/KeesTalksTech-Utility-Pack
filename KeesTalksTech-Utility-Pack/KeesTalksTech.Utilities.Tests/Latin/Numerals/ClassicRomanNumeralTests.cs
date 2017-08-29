namespace KeesTalksTech.Utilities.Latin.Numerals
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ClassicRomanNumeralTests
    {
        [TestCategory("UnitTest")]
        [TestMethod]
        public void RomanNumeral_ToString_FullNotation()
        {
            Assert.AreEqual("I",        new RomanNumeral(1).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("II",       new RomanNumeral(2).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("III",      new RomanNumeral(3).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("IIII",     new RomanNumeral(4).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("V",        new RomanNumeral(5).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("VI",       new RomanNumeral(6).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("VII",      new RomanNumeral(7).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("VIII",     new RomanNumeral(8).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("VIIII",    new RomanNumeral(9).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("X",        new RomanNumeral(10).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("XI",       new RomanNumeral(11).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("XVIII",    new RomanNumeral(18).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("XVIIII",   new RomanNumeral(19).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("CXVIII",   new RomanNumeral(118).ToString(RomanNumeralNotation.Additive));
            Assert.AreEqual("CXVIIII",  new RomanNumeral(119).ToString(RomanNumeralNotation.Additive));

            Assert.AreEqual("NULLA",    new RomanNumeral(0).ToString(RomanNumeralNotation.Additive));
        }

        [TestCategory("UnitTest")]
        [TestMethod]
        public void RomanNumeral_ToString_SubtractiveNotation()
        {
            Assert.AreEqual("I",        new RomanNumeral(1).ToString());
            Assert.AreEqual("II",       new RomanNumeral(2).ToString());
            Assert.AreEqual("III",      new RomanNumeral(3).ToString());
            Assert.AreEqual("IV",       new RomanNumeral(4).ToString());
            Assert.AreEqual("V",        new RomanNumeral(5).ToString());
            Assert.AreEqual("VI",       new RomanNumeral(6).ToString());
            Assert.AreEqual("VII",      new RomanNumeral(7).ToString());
            Assert.AreEqual("VIII",     new RomanNumeral(8).ToString());
            Assert.AreEqual("IX",       new RomanNumeral(9).ToString());
            Assert.AreEqual("X",        new RomanNumeral(10).ToString());
            Assert.AreEqual("XI",       new RomanNumeral(11).ToString());
            Assert.AreEqual("XVIII",    new RomanNumeral(18).ToString());
            Assert.AreEqual("XIX",      new RomanNumeral(19).ToString());
            Assert.AreEqual("CXVIII",   new RomanNumeral(118).ToString());
            Assert.AreEqual("CXIX",     new RomanNumeral(119).ToString());

            Assert.AreEqual("NULLA",    new RomanNumeral(0).ToString());
        }

        [TestCategory("UnitTest")]
        [TestMethod]
        public void RomanNumeral_Parse_ClassicNotation()
        {
            Assert.AreEqual(1,  RomanNumeral.Parse("I").Number);
            Assert.AreEqual(2,  RomanNumeral.Parse("II").Number);
            Assert.AreEqual(3,  RomanNumeral.Parse("III").Number);
            Assert.AreEqual(4,  RomanNumeral.Parse("IIII").Number);
            Assert.AreEqual(5,  RomanNumeral.Parse("V").Number);
            Assert.AreEqual(6,  RomanNumeral.Parse("VI").Number);
            Assert.AreEqual(7,  RomanNumeral.Parse("VII").Number);
            Assert.AreEqual(8,  RomanNumeral.Parse("VIII").Number);
            Assert.AreEqual(9,  RomanNumeral.Parse("VIIII").Number);
            Assert.AreEqual(10, RomanNumeral.Parse("X").Number);
            Assert.AreEqual(11, RomanNumeral.Parse("XI").Number);

            Assert.AreEqual(1910, RomanNumeral.Parse("MDCCCCX").Number);
            Assert.AreEqual(1910, RomanNumeral.Parse("MCMX").Number);

            Assert.AreEqual(118, RomanNumeral.Parse("CXVIII").Number);
            Assert.AreEqual(118, RomanNumeral.Parse("CIIXX").Number);
            Assert.AreEqual(118, RomanNumeral.Parse("CXIIX").Number);

            Assert.AreEqual(119, RomanNumeral.Parse("CXVIIII").Number);
            Assert.AreEqual(119, RomanNumeral.Parse("CXIX").Number);


            Assert.AreEqual(0,  RomanNumeral.Parse("NULLA").Number);
        }

        [TestCategory("UnitTest")]
        [TestMethod]
        public void RomanNumeral_Parse_InvalidClassicNotation()
        {
            //substractive notation
            Assert.IsNull(RomanNumeral.Parse("IV"));
            Assert.IsNull(RomanNumeral.Parse("IX"));

            //invalid order
            Assert.IsNull(RomanNumeral.Parse("IVL"));

            //valid order
            Assert.IsNotNull(RomanNumeral.Parse("LVI"));
        }
    }
}