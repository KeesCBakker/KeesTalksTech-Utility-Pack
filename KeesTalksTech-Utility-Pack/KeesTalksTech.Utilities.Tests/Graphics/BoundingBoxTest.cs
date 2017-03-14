using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeesTalksTech.Utilities.Graphics
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class BoundingBoxTest
    {
        [TestMethod]
		[TestCategory("UnitTest")]
        public void BoundingBoxScale_Landscape_Greater()
        {
            var bb = new BoundingBox(200, 100);
            bb = bb.Scale(2);

            Assert.AreEqual(400, bb.Width);
            Assert.AreEqual(200, bb.Height);
        }

        [TestMethod]
		[TestCategory("UnitTest")]
        public void BoundingBoxScale_Portrait_Greater()
        {
            var bb = new BoundingBox(100, 200);
            bb = bb.Scale(2);

            Assert.AreEqual(200, bb.Width);
            Assert.AreEqual(400, bb.Height);
        }

        [TestMethod]
		[TestCategory("UnitTest")]
        public void BoundingBoxScale_Landscape_Smaller()
        {
            var bb = new BoundingBox(200, 100);
            bb = bb.Scale(0.5f);

            Assert.AreEqual(100, bb.Width);
            Assert.AreEqual(50, bb.Height);
        }

        [TestMethod]
		[TestCategory("UnitTest")]
        public void BoundingBoxScale_Portrait_Smaller()
        {
            var bb = new BoundingBox(100, 200);
            bb = bb.Scale(0.5f);

            Assert.AreEqual(50, bb.Width);
            Assert.AreEqual(100, bb.Height);
        }

        [TestMethod]
		[TestCategory("UnitTest")]
        public void BoundingBoxCalculate_Landscape_SmallerSquare()
        {
            var bb = new BoundingBox(200, 100);
            bb = bb.Calculate(100, 100);

            Assert.AreEqual(100, bb.Width);
            Assert.AreEqual(50, bb.Height);
        }

        [TestMethod]
		[TestCategory("UnitTest")]
        public void BoundingBoxCalculate_Landscape_BiggerSquare()
        {
            var bb = new BoundingBox(200, 100);
            bb = bb.Calculate(400, 400);

            Assert.AreEqual(400, bb.Width);
            Assert.AreEqual(200, bb.Height);
        }

        [TestMethod]
		[TestCategory("UnitTest")]
        public void BoundingBoxCalculate_Square_BiggerLandscape()
        {
            var bb = new BoundingBox(200, 200);
            bb = bb.Calculate(300, 400);

            Assert.AreEqual(300, bb.Width);
            Assert.AreEqual(300, bb.Height);
        }

        [TestMethod]
		[TestCategory("UnitTest")]
        public void BoundingBoxCalculate_Landscape_SameResolutionLandscape()
        {
            var bb = new BoundingBox(200, 100);
            bb = bb.Calculate(400, 200);

            Assert.AreEqual(400, bb.Width);
            Assert.AreEqual(200, bb.Height);
        }

        [TestMethod]
		[TestCategory("UnitTest")]
        public void BoundingBoxCalculate_Landscape_BiggerWidescape()
        {
            var bb = new BoundingBox(200, 100);
            bb = bb.Calculate(400, 150);

            Assert.AreEqual(300, bb.Width);
            Assert.AreEqual(150, bb.Height);
        }

        [TestMethod]
		[TestCategory("UnitTest")]
        public void BoundingBoxCalculate_Portrait_BiggerSkyscraper()
        {
            var bb = new BoundingBox(100, 200);
            bb = bb.Calculate(150, 400);

            Assert.AreEqual(150, bb.Width);
            Assert.AreEqual(300, bb.Height);
        }

        [TestMethod]
		[TestCategory("UnitTest")]
        public void BoundingBoxCalculate_Portrait_Landscape()
        {
            var bb = new BoundingBox(100, 200);
            bb = bb.Calculate(400, 150);

            Assert.AreEqual(75, bb.Width);
            Assert.AreEqual(150, bb.Height);
        }
    }
}