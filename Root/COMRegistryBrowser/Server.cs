using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace ComBrowser
{
    internal class Server : RegistryEntry, IEquatable<Server>
    {
        private string fullPath;
        private string fileName;
        private string assembly;

        public Server(RegistryKey parentKey, string guid)
            : base(guid)
        {
            using (var serverKey = parentKey.OpenSubKey(guid))
            {
                Name = serverKey.GetDefaultValue();
                fullPath = serverKey.GetDefaultValue(@"InprocServer32") ?? serverKey.GetDefaultValue(@"LocalServer32");
                assembly = serverKey.GetSubKeyValue(@"InprocServer32", @"Assembly");
            }

            if (fullPath != null)
            {
                fullPath = fullPath.GetFullFilePath();
                Exists = File.Exists(this.fullPath);

                if (!Exists && fullPath.Contains(' '))
                {
                    var temp = fullPath.Split(' ').First().GetFullFilePath();

                    if (File.Exists(temp))
                    {
                        fullPath = temp;
                        Exists = true;
                    }
                }

                this.fileName = Path.GetFileName(fullPath);
            }
            else if (assembly != null)
            {
                try
                {
                    System.Reflection.Assembly.ReflectionOnlyLoad(assembly);
                    Exists = true;
                }
                catch
                {
                }
            }
        }

        public string FullPath
        {
            get { return this.fullPath; }
        }

        public string FileName
        {
            get { return this.fileName; }
        }

        public string Assembly
        {
            get { return assembly; }
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
        /// Determines whether the specified <see cref="Server"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="Server"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="Server"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Server other)
        {
            return InternalEquals(this, other);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        public static bool operator ==(Server left, Server right)
        {
            return InternalEquals(left, right);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        public static bool operator !=(Server left, Server right)
        {
            return !InternalEquals(left, right);
        }

        #endregion
    }
}
