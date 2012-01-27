using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace ComBrowser
{
    internal class ServerCollection : ItemsCollection<Server>
    {
        public ServerCollection(Browser browser)
        {
            InterfaceCollection = new ItemsCollection<Interface>(InterfaceFilter);
            TypeLibraryCollection = new ItemsCollection<TypeLibrary>(TypeLibraryFilter);

            BindingOperations.SetBinding(InterfaceCollection, ItemsCollection<Interface>.ItemsProperty, new Binding("InterfaceCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
            BindingOperations.SetBinding(TypeLibraryCollection, ItemsCollection<TypeLibrary>.ItemsProperty, new Binding("TypeLibraryCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
        }

        private bool InterfaceFilter(Interface intf)
        {
            if (intf == null)
                return false;

            var current = CurrentItem;

            if (current == null)
                return false;

            return string.Equals(intf.ProxyStub, current.Guid, StringComparison.OrdinalIgnoreCase);
        }

        private bool TypeLibraryFilter(TypeLibrary typeLibrary)
        {
            if (typeLibrary == null)
                return false;

            var current = CurrentItem;

            if (current == null)
                return false;

            return string.Equals(typeLibrary.FullPath, current.FullPath, StringComparison.OrdinalIgnoreCase);
        }

        public ItemsCollection<Interface> InterfaceCollection
        {
            get;
            private set;
        }

        public ItemsCollection<TypeLibrary> TypeLibraryCollection
        {
            get;
            private set;
        }

        protected override void OnCurrentItemChanged(Server oldValue, Server newValue)
        {
            base.OnCurrentItemChanged(oldValue, newValue);

            InterfaceCollection.Refresh();
            TypeLibraryCollection.Refresh();
        }
    }
}
