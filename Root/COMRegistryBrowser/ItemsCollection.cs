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
    internal abstract class ItemsCollection<T> : DependencyObject where T : RegistryEntry
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

        public T CurrentItem
        {
            get { return (T)GetValue(CurrentItemProperty); }
            set { SetValue(CurrentItemProperty, value); }
        }
        /// <summary>
        /// Identifies the CurrentItem dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentItemProperty =
            DependencyProperty.Register("CurrentItem", typeof(T), typeof(ItemsCollection<T>), new UIPropertyMetadata(new PropertyChangedCallback((sender, e) => ((ItemsCollection<T>)sender).CurrentItemChanged((T)e.OldValue, (T)e.NewValue))));

        private void CurrentItemChanged(T oldValue, T newValue)
        {
            OnCurrentItemChanged(oldValue, newValue);
        }

        protected virtual void OnCurrentItemChanged(T oldValue, T newValue)
        {
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
            DependencyProperty.Register("ItemsView", typeof(ICollectionView), typeof(ItemsCollection<T>), new UIPropertyMetadata(new PropertyChangedCallback((sender, e) => ((ItemsCollection<T>)sender).ItemsViewChanged((ICollectionView)e.OldValue, (ICollectionView)e.NewValue))));

        private void ItemsViewChanged(ICollectionView oldValue, ICollectionView newValue)
        {
            if (oldValue != null)
            {
                oldValue.CurrentChanged -= ItemsView_CurrentChanged;
            }

            if (newValue != null)
            {
                newValue.Filter = ActiveFilter;
                newValue.CurrentChanged += ItemsView_CurrentChanged;
                newValue.MoveCurrentTo(CurrentItem);
            }
        }

        private void ItemsView_CurrentChanged(object sender, EventArgs e)
        {
            CurrentItem = (T)ItemsView.CurrentItem;
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
            get;
            set;
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
            var itemsToRemove = new HashSet<RegistryEntry>(selectedItems.Cast<RegistryEntry>());

            foreach (var item in selectedItems)
            {
                GetDependencies(item, itemsToRemove);
            }

            var itemsToRemoveText = string.Join("\n", itemsToRemove.Select(item => item.ToString()));

            MessageBox.Show("Do you want to remove this entries?\n\n" + itemsToRemoveText);
        }

        private static bool AreRelated(Server server, Interface intf)
        {
            if ((server == null) || (intf == null))
                return false;

            return string.Equals(intf.ProxyStub, server.Guid, StringComparison.OrdinalIgnoreCase);
        }

        private static bool AreRelated(Server server, TypeLibrary typeLibrary)
        {
            if ((typeLibrary == null) || (server == null))
                return false;

            return string.Equals(typeLibrary.FullPath, server.FullPath, StringComparison.OrdinalIgnoreCase);
        }

        private static bool AreRelated(TypeLibrary typeLibrary, Interface intf)
        {
            if ((intf == null) || (typeLibrary == null))
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
    }
}
