using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ComBrowser
{
    internal class TypeLibraryCollection : ItemsCollection<TypeLibrary>
    {
        public TypeLibraryCollection(Browser browser)
        {
            ServerCollection = new ItemsCollection<Server>(ServerFilter);
            InterfaceCollection = new ItemsCollection<Interface>(InterfaceFilter);

            BindingOperations.SetBinding(ServerCollection, ItemsCollection<Server>.ItemsProperty, new Binding("ServerCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
            BindingOperations.SetBinding(InterfaceCollection, ItemsCollection<Interface>.ItemsProperty, new Binding("InterfaceCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
        }

        private bool ServerFilter(Server server)
        {
            if (server == null)
                return false;

            var current = CurrentItem;

            if (current == null)
                return false;

            return string.Equals(server.FullPath, current.FullPath, StringComparison.OrdinalIgnoreCase);
        }

        private bool InterfaceFilter(Interface intf)
        {
            if (intf == null)
                return false;

            var current = CurrentItem;

            if (current == null)
                return false;

            return string.Equals(current.Guid, intf.TypeLibrary, StringComparison.OrdinalIgnoreCase)
                && string.Equals(current.Version, intf.TlbVersion, StringComparison.OrdinalIgnoreCase);
        }

        public ItemsCollection<Server> ServerCollection
        {
            get;
            private set;
        }

        public ItemsCollection<Interface> InterfaceCollection
        {
            get;
            private set;
        }

        protected override void OnCurrentItemChanged(TypeLibrary oldValue, TypeLibrary newValue)
        {
            base.OnCurrentItemChanged(oldValue, newValue);

            ServerCollection.Refresh();
            InterfaceCollection.Refresh();
        }
    }
}
