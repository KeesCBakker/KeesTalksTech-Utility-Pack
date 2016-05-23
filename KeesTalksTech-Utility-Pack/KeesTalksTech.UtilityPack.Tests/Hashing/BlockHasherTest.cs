namespace KeesTalksTech.Utilities.Hashing
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;

	[TestClass]
	public class BlockHasherTest
	{
		[TestCategory("UnitTest")]
		[TestMethod]
		public void BlockHasher_ApiRequest_HMACSHA1()
		{
			//api information
			string apiKey = "DvTVukOITB5GJ5r79IEy3J9LALZ1LLex";
			string clientSecret = "5d18SSM38x1lGjMD5qCX1FJGsw4jJ12t";

			//request data
			Dictionary<string, string> request = new Dictionary<string, string>();
			request.Add("Message", "Hello world!");
			request.Add("PublishDate", "1984-09-12");
			request.Add("Tags", "first,message,ever");
			request.Add("Active", "true");

			string signature = null;

			using (var algorithm = new HMACSHA1(Encoding.ASCII.GetBytes(clientSecret)))
			{
				var hasher = new BlockHasher(algorithm);

				var orderedKeys = request.Keys.OrderBy(k => k);

				//hash keys
				foreach (var k in orderedKeys)
				{
					hasher.Transform(k);
					hasher.Transform(",");
				}

				//hash values
				foreach (var k in orderedKeys)
				{
					hasher.Transform(request[k]);
					hasher.Transform(",");
				}

				hasher.Transform(apiKey);

				signature = hasher.GetStringHash(BlockHasher.StringFormat.Base64);
			}

			Assert.AreEqual("279y647EmEXFNFH2ZtNesIc6Skw=", signature);
		}

		[TestCategory("UnitTest")]
		[TestMethod]
		public async Task BlockHasher_AsyncStreamTransform_MD5HashStringHexadecimalResult()
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
		public void BlockHasher_GivenHasherSingleTransform_SHA1HashStringHexadecimalResult()
		{
			using (var blockhasher = new BlockHasher(HashAlgorithm.Create("sha1")))
			{
				blockhasher.Transform("Hello world!");
				var a1 = blockhasher.GetStringHash();

				Assert.AreEqual(a1, "d3486ae9136e7856bc42212385ea797094475802", "Invalid SHA1 hash result.");
			}
		}

		[TestCategory("UnitTest")]
		[TestMethod]
		public void BlockHasher_MultipleTransforms_MD5HashStringHexadecimalResult()
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
		public void BlockHasher_SingleTransformBuffer_MD5HashStringHexadecimalResult()
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
		public void BlockHasher_SingleTransformPartialBuffer_MD5HashStringHexadecimalResult()
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
		public void BlockHasher_SingleTransformString_MD5HashStringHexadecimalResult()
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
		public void BlockHasher_StreamTransform_MD5HashStringHexadecimalResult()
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

		public async Task<string> HashFile(string path, string algorithm = "md5")
		{
			using (var file = File.OpenRead(path))
			{
				using (var blockhasher = new BlockHasher("md5"))
				{
					await blockhasher.TransformAsync(file);
					return blockhasher.GetStringHash();
				}
			}
		}
	}
}