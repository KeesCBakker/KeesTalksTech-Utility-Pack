using System.Reflection;
	
namespace KeesTalksTech.Utilities.Compilation
{
	public interface ICompiler
	{
		/// <summary>
		/// Compiles the specified code the sepcified assembly locations.
		/// </summary>
		/// <param name="assemblyLocations">The assembly locations.</param>
		/// <param name="code">The code.</param>
		/// <returns>The assembly.</returns>
		Assembly Compile(string[] assemblyLocations, string code);
	}
}
