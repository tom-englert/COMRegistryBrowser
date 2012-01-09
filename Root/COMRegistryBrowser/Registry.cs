using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace ComBrowser
{
    internal sealed class Registry : IDisposable
    {
        public Registry()
        {
            ClassesRoot = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default);
            CLSID = ClassesRoot.OpenSubKey("CLSID");
            Interface = ClassesRoot.OpenSubKey("Interface");
            TypeLib = ClassesRoot.OpenSubKey("TypeLib");
        }

        public RegistryKey ClassesRoot { get; private set; }
        public RegistryKey CLSID { get; private set; }
        public RegistryKey Interface { get; private set; }
        public RegistryKey TypeLib { get; private set; }

        #region IDisposable Members

        public void Dispose()
        {
            ClassesRoot.Dispose();
            CLSID.Dispose();
            Interface.Dispose();
            TypeLib.Dispose();
        }

        #endregion
    }
}
