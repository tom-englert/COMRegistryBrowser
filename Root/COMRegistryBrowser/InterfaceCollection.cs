using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMRegistryBrowser
{
    internal class InterfaceCollection : ItemsCollection<Interface>
    {
        public InterfaceCollection(Browser browser)
            : base(browser, null)
        {
        }

        public InterfaceCollection(Browser browser, Predicate<Interface> filter)
            : base(browser, filter)
        {
        }

        internal override void GetDependencies(Interface item, HashSet<RegistryEntry> dependencies)
        {
            GetDependencies(Browser.ServerCollection, RelatedServerPredicate(item), dependencies);
            GetDependencies(Browser.TypeLibraryCollection, RelatedTypeLibraryPredicate(item), dependencies);
        }
    }
}
