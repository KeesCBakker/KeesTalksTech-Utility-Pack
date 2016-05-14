namespace KeesTalksTech.Utilities.Compilation
{
	using System.Collections.Generic;

	/// <summary>
	/// Instructions that are used to compiler a piece of code. Used by the extension methods.
	/// </summary>
	/// <seealso cref="KeesTalksTech.Utilities.Compilation.ICompilerInstructions" />
	public class CompilerInstructions : ICompilerInstructions
	{
		/// <summary>
		/// Gets the assembly locations.
		/// </summary>
		/// <value>
		/// The assembly locations.
		/// </value>
		public List<string> AssemblyLocations { get; } = new List<string>();

		/// <summary>
		/// Gets the name of the class. It is used to get class out of the compiled assembly.
		/// </summary>
		/// <value>
		/// The name of the class.
		/// </value>
		public string ClassName { get; set; }

		/// <summary>
		/// Gets the code.
		/// </summary>
		/// <value>
		/// The code.
		/// </value>
		public string Code { get; set; }

		/// <summary>
		/// Gets the assembly locations.
		/// </summary>
		/// <value>
		/// The assembly locations.
		/// </value>
		string[] ICompilerInstructions.AssemblyLocations
		{
			get { return AssemblyLocations.ToArray(); }
		}
	}
}
