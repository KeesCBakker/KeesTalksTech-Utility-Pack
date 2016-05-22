using System;
using System.Threading.Tasks;
using System.Web;

namespace KeesTalksTech.Utiltities.Caching.Web
{
	/// <summary>
	/// Extends the <see cref="IWebCacheCow" /> interface to 
	/// </summary>
	public static class WebCacheCowExtensions
	{
		/// <summary>
		/// Creates the value or gets the value for the specified method from cache.
		/// A created value is stored in the cache.
		/// </summary>
		/// <typeparam name="T">The value type.</typeparam>
		/// <param name="obj">The object.</param>
		/// <param name="method">The method.</param>
		/// <param name="creator">The creator. Creates the value.</param>
		/// <param name="cacheMinutes">The cache minutes.</param>
		/// <returns>The value.</returns>
		public static T CreateOrGetFromCache<T>(this IWebCacheCow obj, string method, Func<T> creator, int cacheMinutes = 5)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}
			if (creator == null)
			{
				throw new ArgumentNullException(nameof(creator));
			}

			T value;
			if (obj.TryGetFromCache<T>(method, out value))
			{
				return value;
			}

			value = creator();
			obj.StoreInCache(method, value, cacheMinutes);

			return value;
		}

		/// <summary>
		/// Creates the value or gets the value for the specified method from cache.
		/// A created value is stored in the cache. This method supports async.
		/// </summary>
		/// <typeparam name="T">The value type.</typeparam>
		/// <param name="obj">The object.</param>
		/// <param name="method">The method.</param>
		/// <param name="creator">The creator. Creates the value asynchronously.</param>
		/// <param name="cacheMinutes">The cache minutes.</param>
		/// <returns>The value.</returns>
		public static async Task<T> CreateOrGetFromCache<T>(this IWebCacheCow obj, string method, Func<Task<T>> creator, int cacheMinutes = 5)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}
			if (creator == null)
			{
				throw new ArgumentNullException(nameof(creator));
			}

			T value;
			if (obj.TryGetFromCache<T>(method, out value))
			{
				return value;
			}

			value = await creator();
			obj.StoreInCache(method, value, cacheMinutes);

			return value;
		}

		/// <summary>
		/// Creates the cache key.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="method">The method.</param>
		/// <returns>The key.</returns>
		private static string CreateCacheKey(this IWebCacheCow obj, string method)
		{
			return obj.GetUniqueCacheKey() + "." + method;
		}

		public static bool TryGetFromCache<T>(this IWebCacheCow obj, string method, out T value) 
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}

			string key = obj.CreateCacheKey(method);
			var cache = HttpContext.Current.Cache[key];

			if (cache == null || !(cache is T))
			{
				value = default(T);
				return false;
			}

			value = (T)cache;
			return true;
		}

		/// <summary>
		/// Stores the value for the specified method in the cache.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj">The object.</param>
		/// <param name="method">The method.</param>
		/// <param name="value">The value.</param>
		/// <param name="cacheMinutes">The cache minutes.</param>
		public static void StoreInCache<T>(this IWebCacheCow obj, string method, T value, int cacheMinutes = 5)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}

			string key = obj.CreateCacheKey(method);
			HttpContext.Current.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(cacheMinutes));
		}

		/// <summary>
		/// Removes the value that is cached by the method from cache.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="method">The method.</param>
		public static void RemoveFromCache(this IWebCacheCow obj, string method)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}

			string key = obj.CreateCacheKey(method);
			HttpContext.Current.Cache.Remove(key);
		}
	}
}
