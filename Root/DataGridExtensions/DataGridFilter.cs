using System;
using System.Windows;
using System.Windows.Controls;

namespace DataGridExtensions
{
    /// <summary>
    /// Defines the attached properties that can be defined on the data grid level.
    /// </summary>
    public static class DataGridFilter
    {
        #region IsFilterEnabled attached property

        public static bool GetIsFilterEnabled(DataGrid obj)
        {
            return (bool)obj.GetValue(IsFilterEnabledProperty);
        }
        public static void SetIsFilterEnabled(DataGrid obj, bool value)
        {
            obj.SetValue(IsFilterEnabledProperty, value);
        }
        /// <summary>
        /// Identifies the IsFilterEnabled dependency property
        /// </summary>
        public static readonly DependencyProperty IsFilterEnabledProperty =
            DependencyProperty.RegisterAttached("IsFilterEnabled", typeof(bool), typeof(DataGridFilter), new UIPropertyMetadata(false, IsFilterEnabled_Changed));

        private static void IsFilterEnabled_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                dataGrid.GetFilter();
            }
        }

        #endregion

        #region Filter attached property

        /// <summary>
        /// Filter attached property to attach the DataGridFilterHost instance to the owning DataGrid. 
        /// This property is only used internal by code and is not accessible from XAML.
        /// </summary>
        public static DataGridFilterHost GetFilter(this DataGrid dataGrid)
        {
            if (dataGrid == null)
                throw new ArgumentNullException("dataGrid");

            var value = (DataGridFilterHost)dataGrid.GetValue(FilterProperty);
            if (value == null)
            {
                value = new DataGridFilterHost(dataGrid);
                dataGrid.SetValue(FilterProperty, value);
            }
            return value;
        }
        /// <summary>
        /// Identifies the Filters dependency property
        /// </summary>
        private static readonly DependencyProperty FilterProperty =
            DependencyProperty.RegisterAttached("Filter", typeof(DataGridFilterHost), typeof(DataGridFilter));

        #endregion

        #region ContentFilterFactory attached property

        private static readonly IContentFilterFactory defaultContentFilterFactory = new SimpleContentFilterFactory(StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// Gets the content filter factory for the data grid filter.
        /// </summary>
        public static IContentFilterFactory GetContentFilterFactory(this DataGrid dataGrid)
        {
            if (dataGrid == null)
                throw new ArgumentNullException("dataGrid");
            return (IContentFilterFactory)dataGrid.GetValue(ContentFilterFactoryProperty);
        }
        /// <summary>
        /// Sets the content filter factory for the data grid filter.
        /// </summary>
        public static void SetContentFilterFactory(this DataGrid dataGrid, IContentFilterFactory value)
        {
            if (dataGrid == null)
                throw new ArgumentNullException("dataGrid");
            dataGrid.SetValue(ContentFilterFactoryProperty, value);
        }
        /// <summary>
        /// Identifies the ContentFilterFactory dependency property
        /// </summary>
        public static readonly DependencyProperty ContentFilterFactoryProperty =
            DependencyProperty.RegisterAttached("ContentFilterFactory", typeof(IContentFilterFactory), typeof(DataGridFilter), new UIPropertyMetadata(defaultContentFilterFactory, null, ContentFilterFactory_CoerceValue));

        private static object ContentFilterFactory_CoerceValue(DependencyObject sender, object value)
        {
            // Ensure non-null content filter.
            return value ?? defaultContentFilterFactory;
        }

        #endregion
    }
}
