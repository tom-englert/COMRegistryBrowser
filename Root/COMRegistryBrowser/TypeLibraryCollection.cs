using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMRegistryBrowser
{
    internal class TypeLibraryCollection : ItemsCollection<TypeLibrary>
    {
        public TypeLibraryCollection(Browser browser)
            : base(browser, null)
        {
        }

        public TypeLibraryCollection(Browser browser, Predicate<TypeLibrary> filter)
            : base(browser, filter)
        {
        }

        internal override void GetDependencies(TypeLibrary item, HashSet<RegistryEntry> dependencies)
        {
            GetDependencies(Browser.ServerCollection, RelatedServerPredicate(item), dependencies);
            GetDependencies(Browser.InterfaceCollection, RelatedInterfacePredicate(item), dependencies);
        }
    }
}
