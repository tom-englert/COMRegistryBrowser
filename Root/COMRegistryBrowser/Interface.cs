using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace COMRegistryBrowser
{
    internal class Interface : RegistryEntry, IEquatable<Interface>
    {
        private string typeLibrary;
        private string typeLibVersion;
        private string proxyStub;

        public Interface(RegistryKey parentKey, string guid, Dictionary<string, TypeLibrary> typeLibraries)
            : base(guid)
        {
            using (var interfaceKey = parentKey.OpenSubKey(guid))
            {
                Name = interfaceKey.GetDefaultValue();

                using (var typeLibKey = interfaceKey.OpenSubKey(@"TypeLib"))
                {
                    if (typeLibKey != null)
                    {
                        typeLibrary = typeLibKey.GetDefaultValue();
                        typeLibKey.TryGetValue("Version", out typeLibVersion);
                    }
                }

                using (var proxyStubKey = interfaceKey.OpenSubKey(@"ProxyStubClsid32"))
                {
                    if (proxyStubKey != null)
                    {
                        proxyStub = proxyStubKey.GetDefaultValue();
                    }
                }
            }

            if ((typeLibrary != null) && (typeLibVersion != null))
            {
                TypeLibrary tlb;
                if (typeLibraries.TryGetValue(typeLibrary + typeLibVersion, out tlb))
                {
                    Exists = tlb.Exists;
                }
            }
        }

        public string TlbVersion
        {
            get { return typeLibVersion; }
        }

        public string TypeLibrary
        {
            get { return typeLibrary; }
        }

        public string ProxyStub
        {
            get { return proxyStub; }
        }

        #region IEquatable implementation

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="Interface"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="Interface"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="Interface"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Interface other)
        {
            return InternalEquals(this, other);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        public static bool operator ==(Interface left, Interface right)
        {
            return InternalEquals(left, right);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        public static bool operator !=(Interface left, Interface right)
        {
            return !InternalEquals(left, right);
        }

        #endregion
    }
}
