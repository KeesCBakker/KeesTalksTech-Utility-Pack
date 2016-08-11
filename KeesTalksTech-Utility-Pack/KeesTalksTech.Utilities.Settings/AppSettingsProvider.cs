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
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var required = property.GetCustomAttribute<RequiredAttribute>() != null;
                var value = GetValue(obj, property.Name, required);

                if (!String.IsNullOrEmpty(value))
                {
                    object convertedValue = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(obj, convertedValue);
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

            string settingName = obj.GetType().FullName + "." + field;

            //fix inner classes
            settingName = settingName.Replace("+", ".");

            return GetValue(settingName, required);
        }
    }
}