using System;
using System.Configuration;
using System.Linq;

namespace SeeGit.Models
{
    public class Settings
    {
        private readonly Configuration _config;
        public Settings()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (!_config.HasFile)
                throw new ConfigurationErrorsException("Config file not found.");
        }

        /// <summary>
        /// Changes configuration modifications to file
        /// </summary>
        public void Save()
        {
            _config.Save(ConfigurationSaveMode.Full);
        }

        /// <summary>
        /// Returns the value associated with the given key or
        /// returns the passed value if key does not exist.
        /// </summary>
        /// <typeparam name="T">Type of default value and return type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultVal">Return value if key does not exist</param>
        /// <param name="createVal">If the key does not exist, add the key-default value pair to configuration</param>
        /// <returns></returns>
        public T GetSetting<T>(string key, T defaultVal, bool createVal = false)
        {
            var pair = _config.AppSettings.Settings[key];
            if (pair == null)
            {
                if (createVal)
                    _config.AppSettings.Settings.Add(
                        new KeyValueConfigurationElement(key, defaultVal.ToString()));

                return defaultVal;
            }

            return (T) Convert.ChangeType(pair.Value, typeof (T));
        }

        /// <summary>
        /// Modifies the value of an existing key or creates a new one.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public void SetSetting(string key, string value)
        {
            if (_config.AppSettings.Settings.AllKeys.Contains(key))
                _config.AppSettings.Settings[key].Value = value;
            else
                _config.AppSettings.Settings.Add(new KeyValueConfigurationElement(key, value));
        }

        /// <summary>
        /// Removes a setting from the configuration.
        /// Does nothing if key does not exist.
        /// </summary>
        /// <param name="key">Key</param>
        public void RemoveSetting(string key)
        {
            _config.AppSettings.Settings.Remove(key);
        }
    }
}