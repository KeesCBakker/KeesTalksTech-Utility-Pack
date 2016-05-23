using KeesTalksTech.Utilities.Locking;
using System;
using System.Threading.Tasks;

namespace KeesTalksTech.Utilities.Caching
{
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
		/// Creates the value or gets the value for the specified method from cache.
		/// A created value is stored in the cache.
		/// </summary>
		/// <typeparam name="T">The value type.</typeparam>
		/// <param name="method">The method.</param>
		/// <param name="creator">The creator. Creates the value.</param>
		/// <param name="cacheMinutes">The cache minutes.</param>
		/// <returns>The value.</returns>
		public T CreateOrGetFromCache<T>(string method, Func<T> creator, int cacheMinutes = 5)
		{
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}
			if (creator == null)
			{
				throw new ArgumentNullException(nameof(creator));
			}

			var key = CreateCacheKey(method);

			return _monitor.ExecuteWithinMonitor(key, () =>
			{
				T value;
				if (TryGetFromCache(method, out value))
				{
					return value;
				}

				value = creator();
				_cacheProvider.Insert(key, value, TimeSpan.FromMinutes(cacheMinutes));
				return value;
			});
		}

		/// <summary>
		/// Creates the value or gets the value for the specified method from cache.
		/// A created value is stored in the cache. This method supports async.
		/// </summary>
		/// <typeparam name="T">The value type.</typeparam>
		/// <param name="method">The method.</param>
		/// <param name="creator">The creator. Creates the value asynchronously.</param>
		/// <param name="cacheMinutes">The cache minutes.</param>
		/// <returns>
		/// The value.
		/// </returns>
		public async Task<T> CreateOrGetFromCache<T>(string method, Func<Task<T>> creator, int cacheMinutes = 5)
		{
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}
			if (creator == null)
			{
				throw new ArgumentNullException(nameof(creator));
			}

			var key = CreateCacheKey(method);

			return await _monitor.ExecuteWithinMonitor(key, async () =>
			{
				T value;
				if (TryGetFromCache(method, out value))
				{
					return value;
				}

				value = await creator();
				StoreInCache(method, value, cacheMinutes);

				return value;
			});
		}

		/// <summary>
		/// Removes the value that is cached by the method from cache.
		/// </summary>
		/// <param name="method">The method.</param>
		public void RemoveFromCache(string method)
		{
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}

			string key = CreateCacheKey(method);
			_monitor.ExecuteWithinMonitor(key, () =>
			{
				_cacheProvider.Remove(key);
			});
		}

		/// <summary>
		/// Stores the value for the specified method in the cache.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="method">The method.</param>
		/// <param name="value">The value.</param>
		/// <param name="cacheMinutes">The cache minutes.</param>
		public void StoreInCache<T>(string method, T value, int cacheMinutes = 5)
		{
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}

			string key = CreateCacheKey(method);
			_monitor.ExecuteWithinMonitor(key, () =>
			{
				_cacheProvider.Insert(key, value, TimeSpan.FromMinutes(cacheMinutes));
			});
		}

		/// <summary>
		/// Tries to get the value from cache.
		/// </summary>
		/// <typeparam name="T">The value type.</typeparam>
		/// <param name="method">The method.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if successful.</returns>
		public bool TryGetFromCache<T>(string method, out T value)
		{
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}

			string key = CreateCacheKey(method);
			var cache = _cacheProvider[key];

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
		/// <param name="method">The method.</param>
		/// <returns>The key.</returns>
		private string CreateCacheKey(string method)
		{
			return _prefix + "." + method;
		}
	}
}