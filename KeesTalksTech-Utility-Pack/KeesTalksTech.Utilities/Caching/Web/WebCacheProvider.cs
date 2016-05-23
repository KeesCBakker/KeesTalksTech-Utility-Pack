using System;
using System.Web;

namespace KeesTalksTech.Utilities.Caching.Web
{
	public class WebCacheProvider : ICacheProvider
	{
		/// <summary>
		/// Gets the <see cref="System.Object"/> with the specified key.
		/// </summary>
		/// <value>
		/// The <see cref="System.Object"/>.
		/// </value>
		/// <param name="key">The key.</param>
		/// <returns>
		/// The object or <c>null</c> if the key is not present.
		/// </returns>
		public object this[string key]
		{
			get
			{
				return HttpContext.Current.Cache?[key];
			}
		}

		/// <summary>
		/// Inserts the value into the cache.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="slidingExpiration">The sliding expiration.</param>
		public void Insert(string key, object value, TimeSpan slidingExpiration)
		{
			HttpContext.Current?.Cache?.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration);
		}

		/// <summary>
		/// Removes the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		/// The object or <c>null</c> if the key is not present.
		/// </returns>
		public object Remove(string key)
		{
			return HttpContext.Current?.Cache.Remove(key);
		}
	}
}
