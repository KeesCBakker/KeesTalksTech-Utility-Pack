namespace KeesTalksTech.Utilities.Evaluation
{
	using KeesTalksTech.Utilities.Compilation.CodeDom;
	using KeesTalksTech.Utilities.Compilation;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System.Text;

	[TestClass]
	public class EvaluatorGenericTTest
	{
		public abstract class MyProducer : IProducer
		{
			public StringBuilder Builder { get; } = new StringBuilder();

			public int Counter { get; set; }

			public string Message { get; set; }

			public abstract object Run();
		}

		[TestMethod]
		public void Evaluator_CodeDomCompilerBuilderScript_StringResult()
		{
			using (var compiler = new CodeDomCompiler())
			{
				var evaluator = new Evaluator<MyProducer>(compiler);

				var script = @"
				Builder.Append(Counter.ToString(""0000""));
				Builder.Append("" "");
				Builder.AppendLine(Message);";

				var producer = evaluator.CreateProducer(script);
				producer.Message = "Hello World!";
				producer.Counter = 1;

				for (var i = 0; i <= 10; i++)
				{
					producer.Run();
					producer.Counter *= 2;
				}

				Assert.AreEqual(@"0001 Hello World!
0002 Hello World!
0004 Hello World!
0008 Hello World!
0016 Hello World!
0032 Hello World!
0064 Hello World!
0128 Hello World!
0256 Hello World!
0512 Hello World!
1024 Hello World!
", producer.Builder.ToString());
			}
		}
	}
}