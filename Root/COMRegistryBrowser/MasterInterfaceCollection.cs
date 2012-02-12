using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace COMRegistryBrowser
{
    internal class MasterInterfaceCollection : InterfaceCollection
    {
        private Predicate<Server> serverFilterPredicate = (_) => false;
        private Predicate<TypeLibrary> typeLibraryFilterPredicate = (_) => false;

        public MasterInterfaceCollection(Browser browser)
            : base(browser)
        {
            ServerCollection = new ServerCollection(browser, ServerFilter);
            TypeLibraryCollection = new TypeLibraryCollection(browser, TypeLibraryFilter);

            BindingOperations.SetBinding(ServerCollection, ServerCollection.ItemsProperty, new Binding("ServerCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
            BindingOperations.SetBinding(TypeLibraryCollection, TypeLibraryCollection.ItemsProperty, new Binding("TypeLibraryCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
        }

        private bool ServerFilter(Server server)
        {
            return serverFilterPredicate(server);
        }

        private bool TypeLibraryFilter(TypeLibrary typeLibrary)
        {
            return typeLibraryFilterPredicate(typeLibrary);
        }

        public ServerCollection ServerCollection
        {
            get;
            private set;
        }

        public TypeLibraryCollection TypeLibraryCollection
        {
            get;
            private set;
        }

        protected override void OnSelectedItemsChanged(IEnumerable<Interface> newValue)
        {
            base.OnSelectedItemsChanged(newValue);

            serverFilterPredicate = (server) => newValue.Select(item => RelatedServerPredicate(item)).Any(pred => pred(server));
            typeLibraryFilterPredicate = (typeLibrary) => newValue.Select(item => RelatedTypeLibraryPredicate(item)).Any(pred => pred(typeLibrary));

            ServerCollection.Refresh();
            TypeLibraryCollection.Refresh();
        }
    }
}
