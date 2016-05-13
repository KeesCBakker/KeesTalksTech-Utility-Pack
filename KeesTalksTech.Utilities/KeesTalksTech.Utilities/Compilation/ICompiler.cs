namespace KeesTalksTech.Utilities.Compilation
{
	using System.Reflection;

	public interface ICompiler
	{
		/// <summary>
		/// Compiles the specified code the sepcified assembly locations.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="assemblyLocations">The assembly locations.</param>
		/// <returns>The assembly.</returns>
		Assembly Compile(string code, params string[] assemblyLocations);
	}
}