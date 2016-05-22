using System;

namespace KeesTalksTech.Utiltities.Caching
{
	public interface ICacheProvider
	{
		/// <summary>
		/// Inserts the value into the cache.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="slidingExpiration">The sliding expiration.</param>
		void Insert(string key, object value, TimeSpan slidingExpiration);

		/// <summary>
		/// Removes the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		/// The object or <c>null</c> if the key is not present.
		/// </returns>
		object Remove(string key);

		/// <summary>
		/// Gets the <see cref="System.Object"/>  that is associated with the specified key from the cache.
		/// </summary>
		/// <value>
		/// The <see cref="System.Object"/>.
		/// </value>
		/// <param name="key">The key.</param>
		/// <returns>The object or <c>null</c> if the key is not present.</returns>
		object this[string key] { get; }
	}
}
