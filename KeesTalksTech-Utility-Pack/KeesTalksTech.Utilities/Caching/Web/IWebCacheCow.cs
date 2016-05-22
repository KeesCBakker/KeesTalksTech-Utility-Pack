using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KeesTalksTech.Utiltities.Caching.Web
{
	/// <summary>
	/// Indicates the class wants to use caching for the web. It extens the class with
	/// powerful async sliding expiration cache. Perfect for wrapping API's with request
	/// limits.
	/// </summary>
	public interface IWebCacheCow
	{
		/// <summary>
		/// Gets the unique cache key.
		/// </summary>
		/// <returns>The key.</returns>
		string GetUniqueCacheKey();
	}
}
