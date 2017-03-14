namespace KeesTalksTech.Utilities.Compilation
{
	using System;
	using System.Collections.Concurrent;
	using System.Reflection;

	/// <summary>
	/// Performs compilation and caches the result in memory.
	/// </summary>
	/// <seealso cref="KeesTalksTech.Utilities.Compilation.ICompiler" />
	public class CachedCompiler : ICompiler
	{
		private readonly ConcurrentDictionary<string, Assembly> cache = new ConcurrentDictionary<string, Assembly>();
		private readonly ICompiler compiler;

		/// <summary>
		/// Initializes a new instance of the <see cref="CachedCompiler"/> class.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public CachedCompiler(ICompiler compiler)
		{
			if (compiler == null)
			{
				throw new ArgumentNullException(nameof(compiler));
			}

			this.compiler = compiler;
		}

		/// <summary>
		/// Compiles the specified code the sepcified assembly locations.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="assemblyLocations">The assembly locations.</param>
		/// <returns>
		/// The assembly.
		/// </returns>
		public Assembly Compile(string code, params string[] assemblyLocations)
		{
			string key = GetCacheKey(code, assemblyLocations);

			return cache.GetOrAdd(key, (k) =>
			{
				return compiler.Compile(code, assemblyLocations);
			});
		}

		/// <summary>
		/// Gets the cache key.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="assemblyLocations">The assembly locations.</param>
		/// <returns>
		/// The key.
		/// </returns>
		private string GetCacheKey(string code, string[] assemblyLocations)
		{
			string key = String.Join("|", code, assemblyLocations);
			return key;
		}
	}
}
