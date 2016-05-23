#if (DEBUG)
namespace KeesTalksTech.Utilities.Evaluation
{
	using KeesTalkstech.Utilities.Compilation.CodeDom;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class EvaluatorTest
	{
		[TestMethod]
		public void Evaluator_MathPowScript_IntResult()
		{
			using (var compiler = new CodeDomCompiler())
			{
				var evaluator = new Evaluator(compiler);

				double result = 0;
				for (var i = 1; i <= 10; i++)
				{
					var script = "return Math.Pow(2, " + i + ");";
					result += evaluator.Run<double>(script);
				}

				Assert.AreEqual(2046, result);
			}
		}
	}
}
#endif