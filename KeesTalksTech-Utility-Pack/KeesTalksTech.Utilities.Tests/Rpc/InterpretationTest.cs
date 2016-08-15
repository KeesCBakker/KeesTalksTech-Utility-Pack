﻿using KeesTalksTech.Utilities.Rpc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace KeesTalksTech.Utilities.UnitTests.Rpc
{
    [TestClass]
    public class InterpretationTest
    {
        [TestMethod]
        public void Interpretation_ExecutMethodWithConverter()
        {
            var json = @"{ ""method-name"": ""SetVariants"", ""colors"": [""red"", ""green"", ""blue""] }";

            var obj = new MyObject();

            var interpreter = Interpretation.Create<IMyObject>(obj, typeof(MyObjectExtensions));
            interpreter.RegisterConverter(new TryConverter((ParameterInfo info, string value, out object newValue) =>
            {
                if (info.Name == "colors")
                {
                    var colors = JsonConvert.DeserializeObject<string[]>(value);
                    newValue = colors.Select(c => Color.FromName(c)).ToArray();
                    return true;
                }

                newValue = null;
                return false;
            }));

            interpreter.Execute(json);

            var expected = new Color[] { Color.Red, Color.Green, Color.Blue };

            for(int i = 0;i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], obj.Variants[i]);
            }
        }

        [TestMethod]
        public void Interpretation_ExecutMethodWithIConvertable()
        {
            var json = @"{ ""method-name"": ""SetColor"", ""color"": ""red"" }";

            var obj = new MyObject();

            var interpreter = Interpretation.Create<IMyObject>(obj, typeof(MyObjectExtensions));
            interpreter.Execute(json);

            Assert.AreEqual(Color.Red, obj.Color);
        }

        [TestMethod]
        public void Interpretation_ExecuteArray()
        {
            var json = @"[{ ""method-name"": ""SetName"", ""name"": ""Kees C. Bakker"" },
                          { ""method-name"": ""ToString"" },
                          { ""method-name"": ""Greet"" }]";

            var obj = new MyObject();

            var interpreter = Interpretation.Create<IMyObject>(obj, typeof(MyObjectExtensions));
            interpreter.Execute(json);

            Assert.AreEqual("Kees C. Bakker", obj.Name);
        }

        [TestMethod]
        public void Interpretation_ExecuteMethodOnInterface()
        {
            var json = @"{ ""method-name"": ""SetName"", ""name"": ""Kees C. Bakker"" }";
            var obj = new MyObject();

            var interpreter = Interpretation.Create<IMyObject>(obj, typeof(MyObjectExtensions));
            interpreter.Execute(json);

            Assert.AreEqual("Kees C. Bakker", obj.Name);
        }

        [TestMethod]
        public void Interpretation_ExecuteMethodOnExtensionMethod()
        {
            var json = @"{ ""method-name"": ""SetName"", ""firstName"": ""Kees"", ""middleName"": ""C."", ""lastName"" : ""Bakker"" }";
            var obj = new MyObject();

            var interpreter = Interpretation.Create<IMyObject>(obj, typeof(MyObjectExtensions));
            interpreter.Execute(json);

            Assert.AreEqual("Kees C. Bakker", obj.Name);
        }

        [TestMethod]
        public void Interpretation_ExecuteMethodWithReturnType()
        {
            var json = @"{ ""method-name"": ""ToString"" }";
            var obj = new MyObject();
            obj.Name = "Haas";

            var interpreter = Interpretation.Create<IMyObject>(obj, typeof(MyObjectExtensions));
            var result = interpreter.Execute(json);

            Assert.AreEqual(obj.ToString(), result);
        }

        [TestMethod]
        public void Interpretation_ExecuteExtensionMethodWithReturnType()
        {
            var json = @"{ ""method-name"": ""Greet"" }";
            var obj = new MyObject();
            obj.Name = "Haas";

            var interpreter = Interpretation.Create<IMyObject>(obj, typeof(MyObjectExtensions));
            var result = interpreter.Execute(json);

            Assert.AreEqual(obj.Greet(), result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Can't execute method that's not on the interface.")]
        public void Interpretation_ExecuteClassMethod()
        {
            var json = @"{ ""method-name"": ""SetId"", ""id"": ""42"" }";
            var obj = new MyObject();
            obj.Name = "Haas";
            obj.Id = 7;

            var interpreter = Interpretation.Create<IMyObject>(obj, typeof(MyObjectExtensions));
            var result = interpreter.Execute(json);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Can't create interpreter for a non-interface.")]
        public void Interpretation_CannotCreateInterpreterOnClassType()
        {
            var obj = new MyObject();
            var interpreter = Interpretation.Create(obj, typeof(MyObjectExtensions));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Can't create interpreter for a non-interface.")]
        public void Interpretation_CannotCreateInterpreterWithNonExtensionType()
        {
            var obj = new MyObject();
            var interpreter = Interpretation.Create<IMyObject>(obj, typeof(MyObject));
        }
    }

    public interface IMyObject
    {
        void SetVariants(params Color[] colors);

        void SetColor(Color color);

        void SetName(string name);

        string ToString();
    }

    public class MyObject : IMyObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public Color[] Variants { get; set; }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetVariants(params Color[] colors)
        {
            this.Variants = colors;
        }

        public void SetColor(Color color)
        {
            Color = color;
        }

        public void SetId(int id)
        {
            this.Id = id;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public static class MyObjectExtensions
    {
        public static void SetName(this IMyObject obj, string firstName, string middleName, string lastName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var array = (new string[] { firstName, middleName, lastName }).Where(s => !String.IsNullOrWhiteSpace(s));
            var name = String.Join(" ", array);

            obj.SetName(name);
        }

        public static string Greet(this IMyObject obj)
        {
            return $"Hello {obj?.ToString()}!";
        }
    }
}
