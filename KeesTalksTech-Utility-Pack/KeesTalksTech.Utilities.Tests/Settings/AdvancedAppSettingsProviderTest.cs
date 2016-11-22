﻿using KeesTalksTech.Utilities.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace KeesTalksTech.Utilities.UnitTests.Settings
{
    class OuterSettings
    {
        public string OuterProperty { get; set; }

        public InnerSettings InnerSettings { get; set; }

        public string NullProperty { get; set; }
    }

    class InnerSettings
    {
        public string InnerProperty { get; set; }
    }


    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class AdvancedAppSettingsProviderTest
    {
        [TestMethod]
        public void AppSettingsProvider_Create_AdvancedObjectSettings()
        {
            var prefix = typeof(OuterSettings).FullName.Replace("+", ".");

            ConfigurationManager.AppSettings[prefix + ".OuterProperty"] = "Kees C. Bakker";
            ConfigurationManager.AppSettings[prefix + ".InnerSettings.InnerProperty"] = "1337!42";

            var s = AppSettingsProvider.Create<OuterSettings>();

            Assert.AreEqual(s.OuterProperty, "Kees C. Bakker");
            Assert.IsNotNull(s.InnerSettings);
            Assert.AreEqual(s.InnerSettings.InnerProperty, "1337!42");
            Assert.IsNull(s.NullProperty);
        }
    }
}
