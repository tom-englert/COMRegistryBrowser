using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace COMRegistryBrowser
{
    internal class MasterTypeLibraryCollection : TypeLibraryCollection
    {
        private Predicate<Interface> interfaceFilterPredicate = (_) => false;
        private Predicate<Server> serverFilterPredicate = (_) => false;

        public MasterTypeLibraryCollection(Browser browser)
            : base(browser)
        {
            ServerCollection = new ServerCollection(browser, ServerFilter);
            InterfaceCollection = new InterfaceCollection(browser, InterfaceFilter);

            BindingOperations.SetBinding(ServerCollection, ServerCollection.ItemsProperty, new Binding("ServerCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
            BindingOperations.SetBinding(InterfaceCollection, InterfaceCollection.ItemsProperty, new Binding("InterfaceCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
        }

        private bool ServerFilter(Server server)
        {
            return serverFilterPredicate(server);
        }

        private bool InterfaceFilter(Interface intf)
        {
            return interfaceFilterPredicate(intf);
        }

        public ServerCollection ServerCollection
        {
            get;
            private set;
        }

        public InterfaceCollection InterfaceCollection
        {
            get;
            private set;
        }

        protected override void OnCurrentItemChanged(TypeLibrary oldValue, TypeLibrary newValue)
        {
            base.OnCurrentItemChanged(oldValue, newValue);

            serverFilterPredicate = RelatedServerPredicate(newValue);
            interfaceFilterPredicate = RelatedInterfacePredicate(newValue);

            ServerCollection.Refresh();
            InterfaceCollection.Refresh();
        }
    }
}
