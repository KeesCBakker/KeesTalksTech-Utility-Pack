#if (DEBUG)
namespace KeesTalksTech.Utiltities.Settings
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
		public void AppSettingsProvider_Create_ObjectSettings()
		{
			ConfigurationManager.AppSettings["KeesTalksTech.Utiltities.Settings.MyTestSettings.UserName"] = "Kees C. Bakker";
			ConfigurationManager.AppSettings["KeesTalksTech.Utiltities.Settings.MyTestSettings.Password"] = "1337!42";

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
		public void AppSettingsProvider_Create_InnerClassObjectSettings()
		{
			ConfigurationManager.AppSettings["KeesTalksTech.Utiltities.Settings.AppSettingsProviderTest.MyInnerTestSettings.UserName"] = "Kees C. Bakker";
			ConfigurationManager.AppSettings["KeesTalksTech.Utiltities.Settings.AppSettingsProviderTest.MyInnerTestSettings.Password"] = "1337!42";

			var s = AppSettingsProvider.Create<MyInnerTestSettings>();

			Assert.AreEqual(s.UserName, "Kees C. Bakker");
			Assert.AreEqual(s.Password, "1337!42");
		}
	}
}
#endif