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
		[TestCategory("UnitTest")]
        public void AppSettingsProvider_Create_ReusedSetting()
		{
            var prefix = typeof(NetworkSettings).FullName.Replace("+", ".");

			ConfigurationManager.AppSettings[prefix + ".IP"] = "127.0.0.1";
			ConfigurationManager.AppSettings[prefix + ".Port"] = "$DefaultPort$";
			ConfigurationManager.AppSettings["DefaultPort"] = "8080";

			var s = AppSettingsProvider.Create<NetworkSettings>();

			Assert.AreEqual(s.IP, "127.0.0.1");
			Assert.AreEqual(s.Port, 8080);
		}

        public class EscapedSetting
        {
            public string EscapedValue { get; set; }
        }

        [TestMethod]
		[TestCategory("UnitTest")]
        public void AppSettingsProvider_Create_EscapedSettings()
        {
            var prefix = typeof(EscapedSetting).FullName.Replace("+", ".");

            ConfigurationManager.AppSettings[prefix + ".EscapedValue"] = "$$MyDollarEscapedPassword$";

            var s = AppSettingsProvider.Create<EscapedSetting>();

            Assert.AreEqual(s.EscapedValue, "$MyDollarEscapedPassword$");
        }

    }
}
