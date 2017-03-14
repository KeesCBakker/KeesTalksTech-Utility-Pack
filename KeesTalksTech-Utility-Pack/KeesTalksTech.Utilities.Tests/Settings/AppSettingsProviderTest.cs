namespace KeesTalksTech.Utilities.Settings
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Configuration;

    class MyTestSettings
	{
		public string UserName { get; set; }

		public string Password { get; set; }
	}

	[TestClass]
	public class AppSettingsProviderTest
	{
		[TestMethod]
		[TestCategory("UnitTest")]
        public void AppSettingsProvider_Create_ObjectSettings()
		{
            var prefix = typeof(MyTestSettings).FullName.Replace("+", ".");

			ConfigurationManager.AppSettings[prefix + ".UserName"] = "Kees C. Bakker";
			ConfigurationManager.AppSettings[prefix + ".Password"] = "1337!42";

			var s = AppSettingsProvider.Create<MyTestSettings>();

			Assert.AreEqual(s.UserName, "Kees C. Bakker");
			Assert.AreEqual(s.Password, "1337!42");
		}

		class MyInnerTestSettings
		{
			public string UserName { get; set; }

			public string Password { get; set; }
		}

		[TestMethod]
        [TestCategory("UnitTest")]
        public void AppSettingsProvider_Create_InnerClassObjectSettings()
		{
            var prefix = typeof(MyInnerTestSettings).FullName.Replace("+", ".");

            ConfigurationManager.AppSettings[prefix + ".UserName"] = "Kees C. Bakker";
            ConfigurationManager.AppSettings[prefix + ".Password"] = "1337!42";

            var s = AppSettingsProvider.Create<MyInnerTestSettings>();

			Assert.AreEqual(s.UserName, "Kees C. Bakker");
			Assert.AreEqual(s.Password, "1337!42");
		}
	}
}
