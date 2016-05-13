using System.Collections.Generic;
using KeesTalksTech.Utilities.Compilation;

namespace KeesTalksTech.Utiltities.Evaluation
{
	/// <summary>
	/// Indicates the object implements an evaluator that helps with the creation and compilation of code.
	/// </summary>
	public interface IEvaluator
	{
		/// <summary>
		/// Some definitions might live in a different assembly that
		/// needs to be referenced in order to compile the DLL.
		/// </summary>
		List<string> AssemblyLocations { get; }

		/// <summary>
		/// Adds usings to the code - this makes it easier to create 
		/// scripts because objects can be used by their name instead
		/// of their full name.
		/// </summary>
		/// <value>
		/// The usings.
		/// </value>
		List<string> Usings { get; }

		/// <summary>
		/// Creates the producer.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns>A producer.</returns>
		IProducer CreateProducer(string code);

		/// <summary>
		/// Runs the specified code.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns>The result.</returns>
		object Run(string code);
	}
}