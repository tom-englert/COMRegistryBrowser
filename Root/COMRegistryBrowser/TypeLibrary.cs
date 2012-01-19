using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

namespace ComBrowser
{
    internal class TypeLibrary : RegistryEntry, IEquatable<TypeLibrary>
    {
        private string version;
        private string fullPath;
        private string fileName;

        public TypeLibrary(string guid, string version, string name, string path)
            : base(guid, name)
        {
            this.version = version;

            if (!string.IsNullOrEmpty(path))
            {
                fullPath = path.GetFullFilePath();
                fileName = Path.GetFileName(fullPath);

                Exists = File.Exists(fullPath);

                int resourceId;
                if (!Exists && int.TryParse(fileName, out resourceId))
                {
                    fullPath = Path.GetDirectoryName(fullPath);
                    this.fileName = Path.GetFileName(fullPath);
                    Exists = File.Exists(fullPath);
                }
            }
        }

        public static IEnumerable<TypeLibrary> GetTypeLibrarys(RegistryKey parentKey, string guid)
        {
            using (var typeLibKey = parentKey.OpenSubKey(guid))
            {
                var subKeyNames = GetSubKeyNames(typeLibKey);

                foreach (var versionName in subKeyNames)
                {
                    using (var versionKey = typeLibKey.OpenSubKey(versionName))
                    {
                        if (versionKey != null)
                        {
                            using (var fileKey32 = versionKey.OpenSubKey(@"0\win32"))
                            {
                                if (fileKey32 != null)
                                {
                                    yield return new TypeLibrary(guid, versionName, versionKey.GetDefaultValue(), fileKey32.GetDefaultValue());
                                }
                                else
                                {
                                    using (var fileKey64 = versionKey.OpenSubKey(@"0\win64"))
                                    {
                                        if (fileKey64 != null)
                                        {
                                            yield return new TypeLibrary(guid, versionName, versionKey.GetDefaultValue(), fileKey64.GetDefaultValue());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static string[] GetSubKeyNames(RegistryKey typeLibKey)
        {
            try
            {
                return typeLibKey.GetSubKeyNames();
            }
            catch
            {
                return new string[0];
            }
        }

        public string Version
        {
            get { return this.version; }
        }

        public string FileName
        {
            get { return this.fileName; }
        }

        public string FullPath
        {
            get { return this.fullPath; }
        }

        public static string GetKey(string guid, string version)
        {
            return guid + (version ?? string.Empty);
        }

        internal string Key
        {
            get
            {
                return GetKey(Guid, version);
            }
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
        /// Determines whether the specified <see cref="TypeLibrary"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="TypeLibrary"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="TypeLibrary"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(TypeLibrary other)
        {
            return InternalEquals(this, other);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        public static bool operator ==(TypeLibrary left, TypeLibrary right)
        {
            return InternalEquals(left, right);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        public static bool operator !=(TypeLibrary left, TypeLibrary right)
        {
            return !InternalEquals(left, right);
        }

        #endregion
    }
}
