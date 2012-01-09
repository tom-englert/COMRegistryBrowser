using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ComBrowser
{
    internal class InterfaceCollection : ItemsCollection<Interface>
    {
        public InterfaceCollection(Browser browser)
        {
            ServerCollection = new ItemsCollection<Server>();
            TypeLibraryCollection = new ItemsCollection<TypeLibrary>();

            ServerCollection.Filter = ServerFilter;
            TypeLibraryCollection.Filter = TypeLibraryFilter;

            BindingOperations.SetBinding(ServerCollection, ItemsCollection<Server>.ItemsProperty, new Binding("ServerCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
            BindingOperations.SetBinding(TypeLibraryCollection, ItemsCollection<TypeLibrary>.ItemsProperty, new Binding("TypeLibraryCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
        }

        private bool ServerFilter(Server server)
        {
            if (server == null)
                return false;

            var current = CurrentItem;

            if (current == null)
                return false;

            return string.Equals(server.Guid, current.ProxyStub, StringComparison.OrdinalIgnoreCase);
        }

        private bool TypeLibraryFilter(TypeLibrary typeLibrary)
        {
            if (typeLibrary == null)
                return false;

            var current = CurrentItem;

            if (current == null)
                return false;

            return string.Equals(typeLibrary.Guid, current.TypeLibrary, StringComparison.OrdinalIgnoreCase)
                && string.Equals(typeLibrary.Version, current.TlbVersion, StringComparison.OrdinalIgnoreCase);
        }

        public ItemsCollection<Server> ServerCollection
        {
            get;
            private set;
        }

        public ItemsCollection<TypeLibrary> TypeLibraryCollection
        {
            get;
            private set;
        }

        protected override void OnCurrentItemChanged(Interface oldValue, Interface newValue)
        {
            base.OnCurrentItemChanged(oldValue, newValue);

            ServerCollection.Refresh();
            TypeLibraryCollection.Refresh();
        }
    }
}
