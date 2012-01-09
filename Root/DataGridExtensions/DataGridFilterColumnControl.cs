using System.Windows;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Reflection;
using System;
using System.Windows.Data;
using System.Text.RegularExpressions;

namespace DataGridExtensions
{
    /// <summary>
    /// This class is the control hosting all information needed for filtering of one column.
    /// Filtering is enabled by simply adding this control to the control template of the DataGridColumnHeader.
    /// </summary>
    public sealed class DataGridFilterColumnControl : Control, INotifyPropertyChanged
    {
        private static BooleanToVisibilityConverter BooleanToVisibilityConverter = new BooleanToVisibilityConverter();

        /// <summary>
        /// The column header of the column we are filtering. This control must be a child element of the column header.
        /// </summary>
        private DataGridColumnHeader columnHeader;
        /// <summary>
        /// The filter we belong to.
        /// </summary>
        private DataGridFilterHost filterHost;
        /// <summary>
        /// The active filter for this column.
        /// </summary>
        private IContentFilter activeFilter;


        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridFilterColumnControl"/> class.
        /// </summary>
        public DataGridFilterColumnControl()
        {
            Loaded += self_Loaded;
            Unloaded += self_Loaded;

            this.DataContext = this;
        }

        void self_Loaded(object sender, RoutedEventArgs e)
        {
            // Find the ancestor column header and data grid controls.
            columnHeader = this.FindAncestorOrSelf<DataGridColumnHeader>();
            if (columnHeader == null)
                throw new InvalidOperationException("DataGridFilterColumnControl must be a child element of a DataGridColumnHeader.");

            var dataGrid = columnHeader.FindAncestorOrSelf<DataGrid>();
            if (dataGrid == null)
                throw new InvalidOperationException("DataGridColumnHeader must be a child element of a DataGrid");

            // Must set an empty template here, so we reliably get the "OnTemplateChanged" also when the columns attached property is null 
            // - otherwise we won't be able to assing the default templates.
            Template = new ControlTemplate();

            // Load our IsFilterVisible and Template properties from the corresponding properties attached to the
            // DataGridColumnHeader.Column property. Use binding since columnHeader.Column is still null at this point.
            var isFilterVisiblePropertyPath = new PropertyPath("Column.(0)", DataGridFilterColumn.IsFilterVisibleProperty);
            BindingOperations.SetBinding(this, VisibilityProperty, new Binding() { Path = isFilterVisiblePropertyPath, Source = columnHeader, Mode = BindingMode.OneWay, Converter = BooleanToVisibilityConverter });

            var templatePropertyPath = new PropertyPath("Column.(0)", DataGridFilterColumn.TemplateProperty);
            BindingOperations.SetBinding(this, TemplateProperty, new Binding() { Path = templatePropertyPath, Source = columnHeader, Mode = BindingMode.OneWay });

            // Find our host and attach oursef.
            filterHost = dataGrid.GetFilter();
            filterHost.AddColumn(this);
        }

        void self_Unloaded(object sender, RoutedEventArgs e)
        {
            // Detach from host.
            filterHost.RemoveColumn(this);
            // Clear all bindings generatend during load. 
            BindingOperations.ClearBinding(this, VisibilityProperty);
            BindingOperations.ClearBinding(this, TemplateProperty);
        }

        #region Filter dependency property

        /// <summary>
        /// The user provided filter (IFilter) or content (usually a string) used to filter this column. 
        /// If the filter object implements IFilter, it will be used directly as the filter,
        /// else the filter object will be passed to the content filter.
        /// </summary>
        public object Filter
        {
            get { return (object)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }
        /// <summary>
        /// Identifies the Filter dependency property
        /// </summary>
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(object), typeof(DataGridFilterColumnControl), new UIPropertyMetadata(null, new PropertyChangedCallback((sender, e) => ((DataGridFilterColumnControl)sender).Filter_Changed(e.NewValue))));

        private void Filter_Changed(object newValue)
        {
            // Update the effective filter. If the filter is provided as content, the content filter will be recreated when needed.
            activeFilter = newValue as IContentFilter;
            // Notify the filter to update the view.
            filterHost.FilterChanged();
        }

        #endregion

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);

            // Try to find a default template if no explict template is set.
            if ((newTemplate == null) && (columnHeader.Column != null))
            {
                Template = this.TryFindResource(columnHeader.Column.GetType()) as ControlTemplate;
            }
        }

        /// <summary>
        /// Returns all distinct visible (filtered) values of this column as string. 
        /// This can be used to e.g. feed the ItemsSource of an AutoCompleteBox to give a hint to the user what to enter.
        /// </summary>
        public IEnumerable<string> Values
        {
            get
            {
                return InternalValues().Distinct();
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this column has some filter condition to evaluate or not. 
        /// If there is no filter condition we don't need to invoke this filter.
        /// </summary>
        internal bool IsFiltered
        {
            get
            {
                return (Filter != null) && !string.IsNullOrWhiteSpace(Filter.ToString()) && (this.columnHeader.Column != null);
            }
        }

        /// <summary>
        /// Returns true if the given item matches the filter condition for this column.
        /// </summary>
        internal bool Matches(object item)
        {
            if (Filter == null)
                return true;

            if (activeFilter == null)
            {
                activeFilter = filterHost.CreateContentFilter(Filter);
            }

            var propertyValue = GetCellContent(item);

            if (propertyValue == null)
                return false;

            return activeFilter.IsMatch(propertyValue);
        }

        /// <summary>
        /// Notification of the filter that the content of the values might have changed. 
        /// </summary>
        internal void ValuesUpdated()
        {
            // We simply raise a change event for the Values propety and create the output on the fly;
            // if there is no binding to the values property we don't waste resources to compute a list that is never used.
            OnPropertyChanged("Values");
        }

        /// <summary>
        /// Identifies the CellValue dependency property, a private helper property used to evaluate the property path for the list items.
        /// </summary>
        private static readonly DependencyProperty CellValueProperty =
            DependencyProperty.Register("CellValue", typeof(object), typeof(DataGridFilterColumnControl));

        /// <summary>
        /// Examines the property path and returns the objects value for this column. 
        /// Filtering is applied on the SortMemberPath, this is the path used to create the binding.
        /// </summary>
        private object GetCellContent(object item)
        {
            var column = columnHeader.Column;
            if (column == null)
                return null;

            var propertyPath = column.SortMemberPath;

            // Since already the name "SortMemberPath" implies that this might be not only a simple property name but a full property path
            // we use binding for evaluation; this will properly handle even complex property paths like e.g. "SubItems[0].Name"
            BindingOperations.SetBinding(this, CellValueProperty, new Binding(propertyPath) { Source = item });
            var propertyValue = GetValue(CellValueProperty);
            BindingOperations.ClearBinding(this, CellValueProperty);

            return propertyValue;
        }

        /// <summary>
        /// Gets the cell content of all list items for this column.
        /// </summary>
        private IEnumerable<string> InternalValues()
        {
            var dataGrid = columnHeader.FindAncestorOrSelf<DataGrid>();
            var items = dataGrid.Items;

            foreach (var item in items)
            {
                var propertyValue = GetCellContent(item);

                if (propertyValue != null)
                {
                    yield return propertyValue.ToString();
                }
            }
        }

        #region INotifyPropertyChanged Members

        private void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}