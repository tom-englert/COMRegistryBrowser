using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections;

namespace COMRegistryBrowser
{
    internal abstract class ItemsCollection<T> : DependencyObject, IItemsCollection where T : RegistryEntry
    {
        private readonly CollectionViewSource itemsViewSource = new CollectionViewSource();
        private readonly Browser browser;
        private readonly Predicate<T> filter;

        protected ItemsCollection(Browser browser, Predicate<T> filter)
        {
            this.browser = browser;
            this.filter = filter;

            BindingOperations.SetBinding(itemsViewSource, CollectionViewSource.SourceProperty, new Binding("Items") { Source = this, Mode = BindingMode.OneWay });
            BindingOperations.SetBinding(this, ItemsViewProperty, new Binding() { Source = itemsViewSource, Mode = BindingMode.OneWay });

        }

        public IList<T> Items
        {
            get { return (IList<T>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        /// <summary>
        /// Identifies the Items dependency property
        /// </summary>
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IList<T>), typeof(ItemsCollection<T>));

        public ICollectionView ItemsView
        {
            get { return (ICollectionView)GetValue(ItemsViewProperty); }
            set { SetValue(ItemsViewProperty, value); }
        }
        /// <summary>
        /// Identifies the ItemsView dependency property
        /// </summary>
        public static readonly DependencyProperty ItemsViewProperty =
            DependencyProperty.Register("ItemsView", typeof(ICollectionView), typeof(ItemsCollection<T>), new UIPropertyMetadata((sender, e) => ((ItemsCollection<T>)sender).ItemsViewChanged((ICollectionView)e.NewValue)));

        private void ItemsViewChanged(ICollectionView newValue)
        {
            if (newValue != null)
            {
                newValue.Filter = ActiveFilter;
            }
        }

        private Predicate<object> ActiveFilter
        {
            get
            {
                if (filter == null)
                    return null;

                return (item) => this.filter((T)item);
            }
        }

        public void Refresh()
        {
            var collectionView = this.ItemsView;

            if (collectionView != null)
            {
                collectionView.Refresh();
            }
        }

        protected Browser Browser
        {
            get
            {
                return browser;
            }
        }

        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
        /// <summary>
        /// Identifies the SelectedItems dependency property
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IList), typeof(ItemsCollection<T>), new UIPropertyMetadata(null, new PropertyChangedCallback((sender, e) => ((ItemsCollection<T>)sender).SelectedItems_Changed((IList)e.NewValue))));

        private void SelectedItems_Changed(IList newValue)
        {
            OnSelectedItemsChanged(newValue.Cast<T>());
        }

        protected virtual void OnSelectedItemsChanged(IEnumerable<T> newValue)
        {
        }

        public ICommand UnregisterCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteCallback = delegate
                    {
                        return (SelectedItems != null) && (SelectedItems.Count > 0);
                    },
                    ExecuteCallback = delegate
                    {
                        UnregisterSelected();
                    }
                };
            }
        }

        protected static void GetDependencies<Q>(ItemsCollection<Q> itemCollection, Predicate<Q> filter, HashSet<RegistryEntry> dependencies) where Q : RegistryEntry
        {
            var items = itemCollection.Items;
            var dependentItems = items.Where(item => filter(item)).ToArray();

            foreach (var dependentItem in dependentItems)
            {
                if (!dependencies.Contains(dependentItem))
                {
                    dependencies.Add(dependentItem);
                    itemCollection.GetDependencies(dependentItem, dependencies);
                }
            }
        }

        internal abstract void GetDependencies(T item, HashSet<RegistryEntry> dependencies);

        private void UnregisterSelected()
        {
            var selectedItems = SelectedItems.Cast<T>();
            var itemsToRemove = new HashSet<RegistryEntry>(selectedItems);

            foreach (var item in selectedItems)
            {
                GetDependencies(item, itemsToRemove);
            }

            var dialog = new UnregisterDialog() 
            { 
                Items = itemsToRemove.OrderBy(item => item.ToString()).ToArray(),
            };

            if ((bool)dialog.ShowDialog())
            {
                Browser.RemoveEntries(itemsToRemove);
            }
        }

        private static bool AreRelated(Server server, Interface intf)
        {
            if ((server == null) || (intf == null))
                return false;

            if (string.IsNullOrEmpty(server.Guid))
                return false;

            return string.Equals(intf.ProxyStub, server.Guid, StringComparison.OrdinalIgnoreCase);
        }

        private static bool AreRelated(Server server, TypeLibrary typeLibrary)
        {
            if ((typeLibrary == null) || (server == null))
                return false;

            if (string.IsNullOrEmpty(server.FullPath))
                return false;

            return string.Equals(typeLibrary.FullPath, server.FullPath, StringComparison.OrdinalIgnoreCase);
        }

        private static bool AreRelated(TypeLibrary typeLibrary, Interface intf)
        {
            if ((intf == null) || (typeLibrary == null))
                return false;

            if (string.IsNullOrEmpty(typeLibrary.Guid))
                return false;

            return string.Equals(typeLibrary.Guid, intf.TypeLibrary, StringComparison.OrdinalIgnoreCase)
                && string.Equals(typeLibrary.Version, intf.TlbVersion, StringComparison.OrdinalIgnoreCase);
        }

        protected static Predicate<Interface> RelatedInterfacePredicate(Server server)
        {
            return delegate(Interface intf)
            {
                return AreRelated(server, intf);
            };
        }

        protected static Predicate<Interface> RelatedInterfacePredicate(TypeLibrary typeLibrary)
        {
            return delegate(Interface intf)
            {
                return AreRelated(typeLibrary, intf);
            };
        }

        protected static Predicate<Server> RelatedServerPredicate(Interface intf)
        {
            return delegate(Server server)
            {
                // PSOAInterface: The system proxy, does type library marshalling for most interfaces.
                // This is an implict dependency, don't list it in the explict dependencies.
                if (string.Equals(server.Guid, @"{00020424-0000-0000-C000-000000000046}", StringComparison.OrdinalIgnoreCase))
                    return false;

                return AreRelated(server, intf);
            };
        }

        protected static Predicate<Server> RelatedServerPredicate(TypeLibrary typeLibrary)
        {
            return delegate(Server server)
            {
                return AreRelated(server, typeLibrary);
            };
        }

        protected static Predicate<TypeLibrary> RelatedTypeLibraryPredicate(Server server)
        {
            return delegate(TypeLibrary typeLibrary)
            {
                return AreRelated(server, typeLibrary);
            };
        }

        protected static Predicate<TypeLibrary> RelatedTypeLibraryPredicate(Interface intf)
        {
            return delegate(TypeLibrary typeLibrary)
            {
                return AreRelated(typeLibrary, intf);
            };
        }

        #region IItemsCollection Members

        bool IItemsCollection.Remove(RegistryEntry item)
        {
            var i = item as T;

            if (i == null)
                return false;

            return Items.Remove(i);
        }

        #endregion
    }
}
