using KeesTalksTech.Utilities.Compilation;
using System;

namespace KeesTalksTech.Utilities.Compilation
{
	/// <summary>
	/// Extends the <see cref="ICompiler" /> namespace by adding some extra compilation methods.
	/// Will make life easier.
	/// </summary>
	public static class CompilerExtensions
	{
		/// <summary>
		/// Compiles the and create object.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="instructions">The instructions.</param>
		/// <param name="constructorParameters">The constructor parameters.</param>
		/// <returns></returns>
		public static object CompileAndCreateObject(this ICompiler compiler, ICompilerInstructions instructions, params object[] constructorParameters)
		{
			var assembly = compiler.Compile(instructions.AssemblyLocations, instructions.Code);

			var type = assembly.GetType(instructions.ClassName);

			return Activator.CreateInstance(type, constructorParameters);
		}

		/// <summary>
		/// Compiles the and create object.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="compiler">The compiler.</param>
		/// <param name="instructions">The instructions.</param>
		/// <param name="constructorParameters">The constructor parameters.</param>
		/// <returns></returns>
		public static T CompileAndCreateObject<T>(this ICompiler compiler, ICompilerInstructions instructions, params object[] constructorParameters)
		{
			return (T)compiler.CompileAndCreateObject(instructions, constructorParameters);
		}

		/// <summary>
		/// Runs the producer.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="instructions">The instructions.</param>
		/// <param name="constructorParameters">The constructor parameters. Leave empty when the constructor has no parameters.</param>
		/// <returns></returns>
		public static object RunProducer(this ICompiler compiler, ICompilerInstructions instructions, params object[] constructorParameters)
		{
			var scriptObject = CompileAndCreateObject<IProducer>(compiler, instructions, constructorParameters);
			return scriptObject.Run();
		}

		/// <summary>
		/// Runs the script.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="instructions">The instructions.</param>
		/// <param name="constructorParameters">The constructor parameters. Leave empty when the constructor has no parameters.</param>
		public static void RunScript(this ICompiler compiler, ICompilerInstructions instructions, params object[] constructorParameters)
		{
			var scriptObject = CompileAndCreateObject<IScript>(compiler, instructions, constructorParameters);
			scriptObject.Run();
		}
	}
}
