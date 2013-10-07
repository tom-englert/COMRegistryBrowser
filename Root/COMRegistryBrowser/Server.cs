namespace COMRegistryBrowser
{
    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.Win32;

    internal class Server : RegistryEntry, IEquatable<Server>
    {
        private const string _rootKeyName = "CLSID";

        private readonly string _fullPath;
        private readonly string _fileName;
        private readonly string _assembly;
        private readonly string _threadingModel;
        private readonly string _progId;
        private readonly string _versionIndependentProgId;

        public Server(RegistryKey parentKey, string guid)
            : base(guid)
        {
            using (var serverKey = parentKey.OpenSubKey(guid))
            {
                Name = serverKey.GetDefaultValue();
                _fullPath = serverKey.GetDefaultValue(@"InprocServer32") ?? serverKey.GetDefaultValue(@"LocalServer32");
                _assembly = serverKey.GetSubKeyValue(@"InprocServer32", @"Assembly");
                _threadingModel = serverKey.GetSubKeyValue(@"InprocServer32", @"ThreadingModel");
                _progId = serverKey.GetDefaultValue(@"ProgID");
                _versionIndependentProgId = serverKey.GetDefaultValue(@"VersionIndependentProgID");
            }

            if (_fullPath != null)
            {
                _fullPath = _fullPath.GetFullFilePath();
                Exists = File.Exists(_fullPath);

                if (!Exists && _fullPath.Contains(' '))
                {
                    var temp = _fullPath.Split(' ').First().GetFullFilePath();

                    if (File.Exists(temp))
                    {
                        _fullPath = temp;
                        Exists = true;
                    }
                }

                _fileName = Path.GetFileName(_fullPath);
            }
            else if (_assembly != null)
            {
                try
                {
                    System.Reflection.Assembly.ReflectionOnlyLoad(_assembly);
                    Exists = true;
                }
                catch
                {
                }
            }
        }

        internal static Server[] GetServers(RegistryKey classesRootKey)
        {
            using (var clsidKey = classesRootKey.OpenSubKey(_rootKeyName))
            {
                Guid tempGuid;

                return clsidKey.GetSubKeyNames()
                    .Where(guid => System.Guid.TryParse(guid, out tempGuid))
                    .Select(guid => new Server(clsidKey, guid))
                    .ToArray();
            }
        }

        protected override string RootKeyName
        {
            get { return _rootKeyName; }
        }

        public string ThreadingModel
        {
            get { return _threadingModel; }
        }

        public string ProgId
        {
            get { return _progId; }
        }

        public string FullPath
        {
            get { return _fullPath; }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public string Assembly
        {
            get { return _assembly; }
        }

        public string VersionIndependentProgId
        {
            get
            {
                return _versionIndependentProgId;
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
