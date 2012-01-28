using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Reflection;
using System.Reactive.Linq;
using System.Reactive;
using System.Windows.Controls.Primitives;
using DataGridExtensions;

namespace COMRegistryBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Browser ViewModel
        {
            get
            {
                return (Browser)DataContext;
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;

            Observable.FromEventPattern(dataGrid, "LoadingRow")
                .Throttle(TimeSpan.FromSeconds(0.2))
                .ObserveOnDispatcher()
                .Subscribe(eventPattern => DataGrid_RowLoadingFinished((DataGrid)eventPattern.Sender));

            Observable.FromEventPattern(dataGrid, "Sorting")
                .ObserveOnDispatcher()
                .Subscribe(eventPattern => DataGrid_Sorted((DataGrid)eventPattern.Sender));

            dataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;

            var selectedItem = dataGrid.SelectedItem;

            if (selectedItem != null)
            {
                // Workaround a bug in the data grid:
                // setting the selected item before the data grid is loaded has no visual effect!
                dataGrid.SelectedIndex = -1;
                dataGrid.SelectedItem = selectedItem;
            }
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Exists")
            {
                e.Column.DisplayIndex = 0;
            }
            else if (e.PropertyName == "Guid")
            {
                e.Column.DisplayIndex = 1;
            }
            else if (e.PropertyName == "Name")
            {
                e.Column.DisplayIndex = 2;
            }
        }

        void DataGrid_RowLoadingFinished(DataGrid dataGrid)
        {
            foreach (var col in dataGrid.Columns)
            {
                col.Width = col.ActualWidth;
            }
        }

        void DataGrid_Sorted(DataGrid dataGrid)
        {
            if (dataGrid.CurrentItem != null)
            {
                dataGrid.ScrollIntoView(dataGrid.CurrentItem);
            }
        }

        private void BeginInvoke(Action action)
        {
            Dispatcher.BeginInvoke(action);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.F5)
            {
                ViewModel.BeginRefresh();
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((DependencyObject)e.OriginalSource).FindAncestor<DataGridRow>() == null)
            {
                // Click on scroll bar or background: ignore this event.
                return;
            }

            var sourceGrid = (DataGrid)sender;
            var itemsSource = (ICollectionView)sourceGrid.ItemsSource;
            var sourceCollection = itemsSource.SourceCollection;
            var currentItem = itemsSource.CurrentItem;

            DataGrid targetGrid;

            if (sourceCollection is Interface[])
            {
                targetGrid = interfacesGrid;
            }
            else if (sourceCollection is Server[])
            {
                targetGrid = serversGrid;
            }
            else if (sourceCollection is TypeLibrary[])
            {
                targetGrid = typeLibsGrid;
            }
            else
            {
                return;
            }

            if (sourceGrid == targetGrid)
            {
                return;
            }

            targetGrid.GetFilter().Clear();

            BeginInvoke(delegate
            {
                targetGrid.FindAncestor<TabItem>().IsSelected = true;
                targetGrid.ScrollIntoView(currentItem);
                targetGrid.SelectedItem = currentItem;
            });
        }
    }
}
