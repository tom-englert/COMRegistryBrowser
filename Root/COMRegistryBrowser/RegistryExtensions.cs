using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace ComBrowser
{
    internal static class RegistryExtensions
    {
        public static string GetDefaultValue(this RegistryKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            var value = key.GetValue(null);

            if (value != null)
            {
                return value.ToString();
            }

            return null;
        }

        public static string GetDefaultValue(this RegistryKey key, string subKeyName)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            using (var subKey = key.OpenSubKey(subKeyName))
            {
                if (subKey != null)
                {
                    return subKey.GetDefaultValue();
                }
            }

            return null;
        }

        public static bool TryGetValue<T>(this RegistryKey key, string valueName, out T value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            var rawValue = key.GetValue(valueName);
            if ((rawValue != null) && typeof(T).IsAssignableFrom(rawValue.GetType()))
            {
                value = (T)rawValue;
                return true;
            }

            value = default(T);
            return false;
        }

        public static string GetSubKeyValue(this RegistryKey key, string subKeyName, string valueName)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            using (var subKey = key.OpenSubKey(subKeyName))
            {
                if (subKey != null)
                {
                    var value = subKey.GetValue(valueName);

                    if (value != null)
                    {
                        return value.ToString();
                    }
                }
            }

            return null;
        }
    }
}
