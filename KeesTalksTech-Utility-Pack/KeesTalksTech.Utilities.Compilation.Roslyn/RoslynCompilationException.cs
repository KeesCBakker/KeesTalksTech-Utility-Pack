namespace KeesTalksTech.Utilities.Compilation.Roslyn
{
	using Microsoft.CodeAnalysis.Emit;
	using System;

	/// <summary>
	/// Object that stores the compilation exception for the Roslyn compiler.
	/// </summary>
	/// <seealso cref="System.Exception" />
	public class RoslynCompilationException : Exception
	{
		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <value>
		/// The result.
		/// </value>
		public EmitResult Result { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="RoslynCompilationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="result">The result.</param>
		public RoslynCompilationException(string message, EmitResult result) : base(message)
		{
			this.Result = result;
		}
	}
}
