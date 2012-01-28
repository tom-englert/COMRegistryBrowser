using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace COMRegistryBrowser
{
    internal class MasterServerCollection : ServerCollection
    {
        private Predicate<Interface> interfaceFilterPredicate = (_) => false;
        private Predicate<TypeLibrary> typeLibraryPredicate = (_) => false;

        public MasterServerCollection(Browser browser)
            : base(browser)
        {
            InterfaceCollection = new InterfaceCollection(browser, InterfaceFilter);
            TypeLibraryCollection = new TypeLibraryCollection(browser, TypeLibraryFilter);

            BindingOperations.SetBinding(InterfaceCollection, InterfaceCollection.ItemsProperty, new Binding("InterfaceCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
            BindingOperations.SetBinding(TypeLibraryCollection, TypeLibraryCollection.ItemsProperty, new Binding("TypeLibraryCollection.Items") { Source = browser, Mode = BindingMode.OneWay });
        }

        private bool InterfaceFilter(Interface intf)
        {
            return interfaceFilterPredicate(intf);
        }

        private bool TypeLibraryFilter(TypeLibrary typeLibrary)
        {
            return typeLibraryPredicate(typeLibrary);
        }

        public InterfaceCollection InterfaceCollection
        {
            get;
            private set;
        }

        public TypeLibraryCollection TypeLibraryCollection
        {
            get;
            private set;
        }

        protected override void OnCurrentItemChanged(Server oldValue, Server newValue)
        {
            base.OnCurrentItemChanged(oldValue, newValue);

            interfaceFilterPredicate = RelatedInterfacePredicate(newValue);
            typeLibraryPredicate = RelatedTypeLibraryPredicate(newValue);

            InterfaceCollection.Refresh();
            TypeLibraryCollection.Refresh();
        }
    }
}
