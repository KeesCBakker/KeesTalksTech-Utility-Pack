namespace KeesTalksTech.Utiltities.Hashing
{
	/* The MIT License (MIT)
	 *
	 * Copyright (c) 2016 Kees C. Bakker, @KeesTalksTech, www.keestalkstech.com
	 * 	
	 * Permission is hereby granted, free of charge, to any person obtaining a copy
	 * of this software and associated documentation files (the "Software"), to deal
	 * in the Software without restriction, including without limitation the rights
	 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	 * copies of the Software, and to permit persons to whom the Software is
	 * furnished to do so, subject to the following conditions:
	 * 
	 * The above copyright notice and this permission notice shall be included in all
	 * copies or substantial portions of the Software.
	 * 
	 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	 * SOFTWARE.
	 */

	using System;
	using System.IO;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Helps with block hashing.
	/// </summary>
	public class BlockHasher : IDisposable
	{
		private HashAlgorithm _hasher;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockHasher"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public BlockHasher(string name)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

			_hasher = HashAlgorithm.Create(name);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockHasher"/> class.
		/// </summary>
		/// <param name="algorithm">The algorithm.</param>
		public BlockHasher(HashAlgorithm algorithm)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm");
			}

			_hasher = algorithm;
		}

		/// <summary>
		/// Indicates how to format the string.
		/// </summary>
		public enum StringFormat
		{
			Hexadecimal,
			Base64
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_hasher.Dispose();
		}

		/// <summary>
		/// Gets the hash.
		/// </summary>
		/// <returns>A byte array with the hash.</returns>
		public byte[] GetHash()
		{
			_hasher.TransformFinalBlock(new byte[0], 0, 0);
			return _hasher.Hash;
		}

		/// <summary>
		/// Gets the string hash.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns>The hash.</returns>
		public string GetStringHash(StringFormat format = StringFormat.Hexadecimal)
		{
			var hash = GetHash();

			switch (format)
			{
				case StringFormat.Hexadecimal:

					{
						string result = "";
						for (int i = 0; i < hash.Length; i++)
						{
							result += hash[i].ToString("x2");
						}
						return result;
					}

				case StringFormat.Base64:
					{
						return Convert.ToBase64String(hash);
					}
				default:
					{

						throw new NotSupportedException();
					}
			}
		}

		/// <summary>
		/// Transforms the specified stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="bufferSize">Size of the buffer.</param>
		public void Transform(Stream stream, int bufferSize = 16 * 1024)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}

			byte[] buffer = new byte[bufferSize];
			int read;

			while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
			{
				Transform(buffer, 0, read);
			}
		}

		/// <summary>
		/// Transforms the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		public void Transform(string str)
		{
			Transform(str, Encoding.UTF8);
		}

		/// <summary>
		/// Transforms the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="encoding">The encoding.</param>
		public void Transform(string str, Encoding encoding)
		{
			var bytes = encoding.GetBytes(str);
			Transform(bytes);
		}

		/// <summary>
		/// Transforms the specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		public void Transform(byte[] buffer)
		{
			Transform(buffer, 0, buffer.Length);
		}

		/// <summary>
		/// Transforms the specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="length">The length.</param>
		public void Transform(byte[] buffer, int offset, int length)
		{
			_hasher.TransformBlock(buffer, offset, length, null, 0);
		}

		/// <summary>
		/// Transforms the asynchronous.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="bufferSize">Size of the buffer.</param>
		/// <returns>The task.</returns>
		public async Task TransformAsync(Stream stream, int bufferSize = 16 * 1024)
		{
			byte[] buffer = new byte[bufferSize];
			int read;

			while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
			{
				Transform(buffer, 0, read);
			}
		}
	}
}