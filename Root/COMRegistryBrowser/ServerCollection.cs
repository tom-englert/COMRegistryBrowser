using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMRegistryBrowser
{
    internal class ServerCollection : ItemsCollection<Server>
    {
        public ServerCollection(Browser browser)
            : base(browser, null)
        {
        }

        public ServerCollection(Browser browser, Predicate<Server> filter)
            : base(browser, filter)
        {
        }

        internal override void GetDependencies(Server item, HashSet<RegistryEntry> dependencies)
        {
            GetDependencies(Browser.InterfaceCollection, RelatedInterfacePredicate(item), dependencies);
            GetDependencies(Browser.TypeLibraryCollection, RelatedTypeLibraryPredicate(item), dependencies);
        }
    }
}
