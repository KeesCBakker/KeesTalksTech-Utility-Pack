namespace KeesTalksTech.Utilities.Compilation
{
	public interface ICompilerInstructions
	{
		/// <summary>
		/// Gets the assembly locations.
		/// </summary>
		/// <value>
		/// The assembly locations.
		/// </value>
		string[] AssemblyLocations { get; }

		/// <summary>
		/// Gets the code.
		/// </summary>
		/// <value>
		/// The code.
		/// </value>
		string Code { get; }

		/// <summary>
		/// Gets the name of the class. It is used to get class out of the compiled assembly.
		/// </summary>
		/// <value>
		/// The name of the class.
		/// </value>
		string ClassName { get; }
	}
}
