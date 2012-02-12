using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Collections;

namespace COMRegistryBrowser
{
    internal class MasterServerCollection : ServerCollection
    {
        private Predicate<Interface> interfaceFilterPredicate = (_) => false;
        private Predicate<TypeLibrary> typeLibraryFilterPredicate = (_) => false;

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
            return typeLibraryFilterPredicate(typeLibrary);
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

        protected override void OnSelectedItemsChanged(IEnumerable<Server> newValue)
        {
            base.OnSelectedItemsChanged(newValue);

            interfaceFilterPredicate = (intf) => newValue.Select(item => RelatedInterfacePredicate(item)).Any(pred => pred(intf));
            typeLibraryFilterPredicate = (typeLibrary) => newValue.Select(item => RelatedTypeLibraryPredicate(item)).Any(pred => pred(typeLibrary));

            InterfaceCollection.Refresh();
            TypeLibraryCollection.Refresh();
        }
    }
}
