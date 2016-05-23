using KeesTalksTech.Utilities.Locking;
using System;
using System.Threading.Tasks;

namespace KeesTalksTech.Utilities.Caching
{
    /// <summary>
    /// Helps with the caching and creation of values. Supports asynchronous creation which makes
    /// it great for the caching of API calls.
    /// </summary>
    public class CacheCow
	{
		private ICacheProvider _cacheProvider;
		private NamedMonitor _monitor = new NamedMonitor();
		private string _prefix = Guid.NewGuid().ToString();

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheCow"/> class.
		/// </summary>
		/// <param name="cacheProvider">The cache provider.</param>
		public CacheCow(ICacheProvider cacheProvider)
		{
			if (cacheProvider == null)
			{
				throw new ArgumentNullException(nameof(cacheProvider));
			}

			_cacheProvider = cacheProvider;
		}

		/// <summary>
		/// Creates the value or gets the value for the specified key from cache.
		/// A created value is stored in the cache.
		/// </summary>
		/// <typeparam name="T">The value type.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="creator">The creator. Creates the value.</param>
		/// <param name="cacheMinutes">The cache minutes.</param>
		/// <returns>The value.</returns>
		public T CreateOrGetFromCache<T>(string key, Func<T> creator, int cacheMinutes = 5)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}
			if (creator == null)
			{
				throw new ArgumentNullException(nameof(creator));
			}

			var cacheKey = CreateCacheKey(key);

			return _monitor.ExecuteWithinMonitor(cacheKey, () =>
			{
				T value;
				if (TryGetFromCache(cacheKey, out value))
				{
					return value;
				}

				value = creator();
				_cacheProvider.Insert(cacheKey, value, TimeSpan.FromMinutes(cacheMinutes));
				return value;
			});
		}

		/// <summary>
		/// Creates the value or gets the value for the specified key from cache.
		/// A created value is stored in the cache. This key supports async.
		/// </summary>
		/// <typeparam name="T">The value type.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="creator">The creator. Creates the value asynchronously.</param>
		/// <param name="cacheMinutes">The cache minutes.</param>
		/// <returns>
		/// The value.
		/// </returns>
		public async Task<T> CreateOrGetFromCache<T>(string key, Func<Task<T>> creator, int cacheMinutes = 5)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}
			if (creator == null)
			{
				throw new ArgumentNullException(nameof(creator));
			}

			var cacheKey = CreateCacheKey(key);

			return await _monitor.ExecuteWithinMonitor(cacheKey, async () =>
			{
				T value;
				if (TryGetFromCache(cacheKey, out value))
				{
					return value;
				}

				value = await creator();
				StoreInCache(cacheKey, value, cacheMinutes);

				return value;
			});
		}

		/// <summary>
		/// Removes the value that is cached by the key from cache.
		/// </summary>
		/// <param name="key">The key.</param>
		public void RemoveFromCache(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			string cacheKey = CreateCacheKey(key);
			_monitor.ExecuteWithinMonitor(cacheKey, () =>
			{
				_cacheProvider.Remove(cacheKey);
			});
		}

		/// <summary>
		/// Stores the value for the specified key in the cache.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="cacheMinutes">The cache minutes.</param>
		public void StoreInCache<T>(string key, T value, int cacheMinutes = 5)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			string cacheKey = CreateCacheKey(key);
			_monitor.ExecuteWithinMonitor(cacheKey, () =>
			{
				_cacheProvider.Insert(cacheKey, value, TimeSpan.FromMinutes(cacheMinutes));
			});
		}

		/// <summary>
		/// Tries to get the value from cache.
		/// </summary>
		/// <typeparam name="T">The value type.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if successful.</returns>
		public bool TryGetFromCache<T>(string key, out T value)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}

			string cacheKey = CreateCacheKey(key);
			var cache = _cacheProvider[cacheKey];

			if (cache == null || !(cache is T))
			{
				value = default(T);
				return false;
			}

			value = (T)cache;
			return true;
		}

		/// <summary>
		/// Creates the cache key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The cache key.</returns>
		private string CreateCacheKey(string key)
		{
			return _prefix + "." + key;
		}
	}
}