#if (DEBUG)
namespace KeesTalksTech.Utiltities.Hashing
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System.IO;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;

	[TestClass]
	public class BlockHasherTest
	{
		[TestCategory("UnitTest")]
		[TestMethod]
		public async Task MD5HashString_AsyncStreamTransform_HexadecimalResult()
		{
			using (var blockhasher = new BlockHasher("md5"))
			{
				var mr = new MemoryStream();
				var buffer = Encoding.UTF8.GetBytes("Hello world!");
				mr.Write(buffer, 0, buffer.Length);
				mr.Position = 0;

				await blockhasher.TransformAsync(mr);
				var a1 = blockhasher.GetStringHash();

				Assert.AreEqual(a1, "86fb269d190d2c85f6e0468ceca42a20", "Invalid MD5 hash result.");
			}
		}

		[TestCategory("UnitTest")]
		[TestMethod]
		public void MD5HashString_MultipleTransforms_HexadecimalResult()
		{
			using (var blockhasher = new BlockHasher("md5"))
			{
				blockhasher.Transform("Hello");
				blockhasher.Transform(" ");
				blockhasher.Transform("world");
				blockhasher.Transform("!");

				var a1 = blockhasher.GetStringHash();

				Assert.AreEqual(a1, "86fb269d190d2c85f6e0468ceca42a20", "Invalid MD5 hash result.");
			}
		}

		[TestCategory("UnitTest")]
		[TestMethod]
		public void MD5HashString_SingleTransformBuffer_HexadecimalResult()
		{
			using (var blockhasher = new BlockHasher("md5"))
			{
				var buffer = Encoding.UTF8.GetBytes("Hello world!");
				blockhasher.Transform(buffer);
				var a1 = blockhasher.GetStringHash();

				Assert.AreEqual(a1, "86fb269d190d2c85f6e0468ceca42a20", "Invalid MD5 hash result.");
			}
		}

		[TestCategory("UnitTest")]
		[TestMethod]
		public void MD5HashString_SingleTransformPartialBuffer_HexadecimalResult()
		{
			using (var blockhasher = new BlockHasher("md5"))
			{
				var buffer = Encoding.UTF8.GetBytes("Hello world!");
				blockhasher.Transform(buffer, 6, buffer.Length - 6);
				var a1 = blockhasher.GetStringHash();

				Assert.AreEqual(a1, "08cf82251c975a5e9734699fadf5e9c0", "Invalid MD5 hash result.");
			}
		}

		[TestCategory("UnitTest")]
		[TestMethod]
		public void MD5HashString_SingleTransformString_HexadecimalResult()
		{
			using (var blockhasher = new BlockHasher("md5"))
			{
				blockhasher.Transform("Hello world!");
				var a1 = blockhasher.GetStringHash();

				Assert.AreEqual(a1, "86fb269d190d2c85f6e0468ceca42a20", "Invalid MD5 hash result.");
			}
		}
		[TestCategory("UnitTest")]
		[TestMethod]
		public void MD5HashString_StreamTransform_HexadecimalResult()
		{
			using (var blockhasher = new BlockHasher("md5"))
			{
				var mr = new MemoryStream();
				var buffer = Encoding.UTF8.GetBytes("Hello world!");
				mr.Write(buffer, 0, buffer.Length);
				mr.Position = 0;

				blockhasher.Transform(mr);
				var a1 = blockhasher.GetStringHash();

				Assert.AreEqual(a1, "86fb269d190d2c85f6e0468ceca42a20", "Invalid MD5 hash result.");
			}
		}

		[TestCategory("UnitTest")]
		[TestMethod]
		public void SHA1HashString_GivenHasherSingleTransform_HexadecimalResult()
		{
			using (var blockhasher = new BlockHasher(HashAlgorithm.Create("sha1")))
			{
				blockhasher.Transform("Hello world!");
				var a1 = blockhasher.GetStringHash();

				Assert.AreEqual(a1, "d3486ae9136e7856bc42212385ea797094475802", "Invalid SHA1 hash result.");
			}
		}
	}
}
#endif