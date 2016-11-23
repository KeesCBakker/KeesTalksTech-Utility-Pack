namespace KeesTalksTech.Utilities.Settings
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Reflection;

    /// <summary>
    /// Provides app settings from the .config to the application. Can be used to automatically fill
    /// settings objects using the convention: {namespace}.{class-name}.{field-name}
    /// </summary>
    public static class AppSettingsProvider
    {
        /// <summary>
        /// Creates the object and fills it with values from the configuration. 
        /// The public settable instance properties with the corresponding 
        /// configuration values. Properties decorated with [Required] will
        /// throw an error if they aren't set by the configuration.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>An instance with filled properties.</returns>
        public static T Create<T>() where T : new()
        {
            var config = new T();
            Fill(config);
            return config;
        }

        /// <summary>
        /// Fills the public settable instance properties with the corresponding configuration values. 
        /// Properties decorated with [Required] will throw an error if they aren't set by the configuration.
        /// </summary>
        /// <param name="obj">The object. Required.</param>
        public static void Fill(object obj)
        {
            Fill(obj, String.Empty);
        }

        /// <summary>
        /// Fills the public settable instance properties with the corresponding configuration values. 
        /// Properties decorated with [Required] will throw an error if they aren't set by the configuration.
        /// </summary>
        /// <param name="obj">The object. Required.</param>
        private static void Fill(object obj, string baseSettingName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var required = property.GetCustomAttribute<RequiredAttribute>() != null;
                var settingName = ResolveSettingName(obj, baseSettingName, property.Name);
                var value = GetValue(settingName, required);

                if (!String.IsNullOrEmpty(value))
                {
                    object convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(obj, convertedValue);
                    continue;
                }

                var emptyConstructor = property.PropertyType.GetConstructor(new Type[0]);
                if(emptyConstructor != null)
                {
                    //create value
                    var propertyValue = emptyConstructor.Invoke(new object[0]);

                    //fill the property
                    var propertySettingName = ResolveSettingName(obj, baseSettingName, property.Name);
                    Fill(propertyValue, propertySettingName);

                    //assign value
                    property.SetValue(obj, propertyValue);
                }
            }
        }

        /// <summary>
        /// Gets the value of the setting from the configuration.
        /// </summary>
        /// <param name="key">The name of the setting.</param>
        /// <param name="required">If <c>true</c> an exception will be thrown if the setting isn't present.</param>
        /// <returns>The value.</returns>
        public static string GetValue(string key, bool required = true)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            string value = ConfigurationManager.AppSettings[key];

            //check escaped value - removed first dollar
            if(value != null && value.StartsWith("$$") && value.EndsWith("$"))
            {
                return value.Substring(1);
            }

            //check reused value with $settingName$.
            if(value != null && value.Length > 1 && value.StartsWith("$") && value.EndsWith("$"))
            {
                return GetValue(value.Substring(1, value.Length - 2), required);
            }

            if (required && String.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Required configuration value for '" + key + "' is missing. Add it to config.");
            }

            return value;
        }

        /// <summary>
        /// Gets the value for the field of an object from the configuration. 
        /// The classname of the object is used to construct the setting name.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="field">The name of the field.</param>
        /// <param name="required">If <c>true</c> an exception will be thrown if the setting isn't present.</param>
        /// <returns>The value.</returns>
        public static string GetValue(object obj, string field, bool required = true)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            string settingName = ResolveSettingName(obj, null, field);
            return GetValue(settingName, required);
        }

        /// <summary>
        /// Gets the value for the field of an object from the configuration. 
        /// The classname of the object is used to construct the setting name.
        /// </summary>
        /// <param name="baseSettingName">The base name of the setting.</param>
        /// <param name="field">The name of the field.</param>
        /// <param name="required">If <c>true</c> an exception will be thrown if the setting isn't present.</param>
        /// <returns>The value.</returns>
        private static string GetValue(string baseSettingName, string field, bool required = true)
        {
            string settingName = ResolveSettingName(null, baseSettingName, field);
            return GetValue(settingName, required);
        }

        /// <summary>
        /// Resolves the name of the setting.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="baseSettingName">The base name of the settings.</param>
        /// <param name="field">The name of the field (property name).</param>
        /// <returns>The setting name.</returns>
        private static string ResolveSettingName(object obj, string baseSettingName, string field)
        {
            string setting = "";

            if (!String.IsNullOrEmpty(baseSettingName))
            {
                setting += baseSettingName;
            }
            else
            {
                setting += obj.GetType().FullName;
            }

            setting += "." + field;
            setting = setting.Replace("+", ".");

            return setting;
        }
    }
}