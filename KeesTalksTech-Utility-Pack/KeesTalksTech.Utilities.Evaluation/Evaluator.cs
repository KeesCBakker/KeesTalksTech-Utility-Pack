﻿namespace KeesTalksTech.Utilities.Evaluation
{
	using KeesTalksTech.Utilities.Compilation;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// The evaluator aids in the compilation of classes. It will take care 
	/// of the ceremony needed to wrap the code into a class, compile it and retrieve the result.
	/// </summary>
	public class Evaluator : IEvaluator
	{
		private readonly ICompiler _compiler;
		private readonly Type _producerType = typeof(IProducer);

		/// <summary>
		/// Initializes a new instance of the <see cref="Evaluator"/> class.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public Evaluator(ICompiler compiler) : this(compiler, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Evaluator"/> class. This constructor can be 
		/// used by classes that inherit the compiler to change the base type of the compiled 
		/// producer class.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="producerType">Type of the producer.</param>
		protected Evaluator(ICompiler compiler, Type producerType = null)
		{
			if (compiler == null)
			{
				throw new ArgumentNullException(nameof(compiler));
			}

			_compiler = compiler;

			if (producerType != null && this._producerType != producerType)
			{
				if (!typeof(IProducer).IsAssignableFrom(producerType))
				{
					throw new NotSupportedException("The baseType parameter needs to be an implementation of IProducer.");
				}

				this._producerType = producerType;
				AssemblyLocations.Add(producerType.Assembly.Location);
			}

			Usings.Add("System");
			AssemblyLocations.Add(typeof(object).Assembly.Location);
			AssemblyLocations.Add(typeof(IProducer).Assembly.Location);
		}

		/// <summary>
		/// Some definitions might live in a different assembly that
		/// needs to be referenced in order to compile the DLL.
		/// </summary>
		public List<string> AssemblyLocations { get; } = new List<string>();

		/// <summary>
		/// Adds usings to the code - this makes it easier to create 
		/// scripts because objects can be used by their name instead
		/// of their full name.
		/// </summary>
		/// <value>
		/// The usings.
		/// </value>
		public List<string> Usings { get; } = new List<string>();

		/// <summary>
		/// Generates the class en compiles it into a producer.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns>The producer.</returns>
		public IProducer CreateProducer(string code)
		{
			var args = new CompilerInstructions();
			args.ClassName = "_" + Guid.NewGuid().ToString("N");
			args.Code = @"<<USINGS>>

	public class <<CLASS_NAME>>: <<BASE_TYPE>>
	{
		public <<MODIFIER>> object Run()
		{
			<<CODE>>;
	
			return null;
		}
	}";
			args.Code = args.Code.Replace("<<CLASS_NAME>>", args.ClassName);
			args.Code = args.Code.Replace("<<BASE_TYPE>>", FixFullName(_producerType));
			args.Code = args.Code.Replace("<<CODE>>", code);
			args.AssemblyLocations.AddRange(this.AssemblyLocations);

			if (Usings.Count > 0)
			{
				string usings = "using " + String.Join(";\nusing ", Usings) + ";";
				args.Code = args.Code.Replace("<<USINGS>>", usings);
			}
			else
			{
				args.Code = args.Code.Replace("<<USINGS>>", "");
			}

			if (_producerType.IsClass)
			{
				args.Code = args.Code.Replace("<<MODIFIER>>", "override");
			}
			else
			{
				args.Code = args.Code.Replace("<<MODIFIER>>", "");
			}

			return _compiler.CompileAndCreateObject<IProducer>(args);
		}

		/// <summary>
		/// Runs the specified code.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <returns>The result.</returns>
		public object Run(string code)
		{
			var producer = CreateProducer(code);
			return producer.Run();
		}

		/// <summary>
		/// Fixes the full name.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The full name.</returns>
		private string FixFullName(Type type)
		{
			return type.FullName.Replace("+", ".");
		}
	}
}
