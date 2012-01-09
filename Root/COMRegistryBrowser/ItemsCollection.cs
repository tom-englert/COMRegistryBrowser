using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;

namespace ComBrowser
{
    internal class ItemsCollection<T> : DependencyObject where T : class
    {
        private readonly CollectionViewSource itemsViewSource = new CollectionViewSource();

        public ItemsCollection()
        {
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

            //var collectionView = ItemsView;

            //if (collectionView != null)
            //{
            //    collectionView.MoveCurrentTo(newValue);
            //}
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
            DependencyProperty.Register("Items", typeof(IList<T>), typeof(ItemsCollection<T>), new UIPropertyMetadata(new PropertyChangedCallback((sender, e) => ((ItemsCollection<T>)sender).ItemsChanged((IList<T>)e.OldValue, (IList<T>)e.NewValue))));

        private void ItemsChanged(IList<T> oldValue, IList<T> newValue)
        {
        }

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
                if (Filter == null)
                    return null;

                return (item) => this.Filter((T)item);
            }
        }

        public Predicate<T> Filter
        {
            get { return (Predicate<T>)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }
        /// <summary>
        /// Identifies the Filter dependency property
        /// </summary>
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(Predicate<T>), typeof(ItemsCollection<T>), new UIPropertyMetadata(new PropertyChangedCallback((sender, e) => ((ItemsCollection<T>)sender).Filter_Changed((Predicate<T>)e.NewValue))));

        private void Filter_Changed(Predicate<T> newValue)
        {
            var collectionView = this.ItemsView;

            if (collectionView != null)
            {
                collectionView.Filter = ActiveFilter;
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
    }
}
