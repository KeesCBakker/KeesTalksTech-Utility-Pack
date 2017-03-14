namespace KeesTalksTech.Utilities.Evaluation
{
	using System;

	/// <summary>
	/// Extension methods to build over evaluators (<see cref="IEvaluator"/>).
	/// </summary>
	public static class EvaluatorExtensions
	{
		/// <summary>
		/// Runs the specified code.
		/// </summary>
		/// <typeparam name="T">The return type.</typeparam>
		/// <param name="evaluator">The evaluator.</param>
		/// <param name="code">The code.</param>
		/// <returns>The result.</returns>
		public static T Run<T>(this IEvaluator evaluator, string code)
		{
			var result = evaluator.Run(code);

			if (result == null)
			{
				return default(T);
			}

			if (typeof(T).IsAssignableFrom(result.GetType()))
			{
				return (T)result;
			}

			result = Convert.ChangeType(result, typeof(T));
			return (T)result;
		}
	}
}
