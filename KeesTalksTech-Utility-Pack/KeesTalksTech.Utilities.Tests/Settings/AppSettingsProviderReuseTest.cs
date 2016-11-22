namespace KeesTalksTech.Utilities.Settings
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Configuration;

	[TestClass]
	public class AppSettingsProviderReuseTest
    {
        class NetworkSettings
        {
            public string IP { get; set; }

            public int Port { get; set; }
        }

        [TestMethod]
		public void AppSettingsProvider_Create_ObjectSettings()
		{
            var prefix = typeof(NetworkSettings).FullName.Replace("+", ".");

			ConfigurationManager.AppSettings[prefix + ".IP"] = "127.0.0.1";
			ConfigurationManager.AppSettings[prefix + ".Port"] = "$DefaultPort$";
			ConfigurationManager.AppSettings["DefaultPort"] = "8080";

			var s = AppSettingsProvider.Create<NetworkSettings>();

			Assert.AreEqual(s.IP, "127.0.0.1");
			Assert.AreEqual(s.Port, 8080);
		}

		class MyInnerTestSettings
		{
			public string UserName { get; set; }

			public string Password { get; set; }
		}

		[TestMethod]
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
