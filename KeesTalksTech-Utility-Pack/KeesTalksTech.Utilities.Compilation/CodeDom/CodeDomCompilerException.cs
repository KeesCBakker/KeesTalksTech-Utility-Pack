namespace KeesTalksTech.Utilities.Compilation.CodeDom
{
	using System;
	using System.CodeDom.Compiler;

	/// <summary>
	/// Object that stores the compilation exception for the Roslyn compiler.
	/// </summary>
	/// <seealso cref="System.Exception" />
	public class CodeDomCompilerException : Exception
	{
		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <value>
		/// The result.
		/// </value>
		public CompilerResults Result { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="RoslynCompilationException" /> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="result">The result.</param>
		/// <param name="innerException">The inner exception.</param>
		public CodeDomCompilerException(string message, CompilerResults result, Exception innerException = null) : base(message, innerException)
		{
			this.Result = result;
		}
	}
}
