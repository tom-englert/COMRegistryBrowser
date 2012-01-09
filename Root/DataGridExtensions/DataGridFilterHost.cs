﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Media;

namespace DataGridExtensions
{
    /// <summary>
    /// This class hosts all filter columns and handles the filter changes on the data grid level.
    /// This class will be attached to the DataGrid.
    /// </summary>
    public sealed class DataGridFilterHost
    {
        /// <summary>
        /// The data grid this filter is attached to.
        /// </summary>
        private readonly DataGrid dataGrid;
        /// <summary>
        /// Filter information about each column.
        /// </summary>
        private readonly List<DataGridFilterColumnControl> filterColumns = new List<DataGridFilterColumnControl>();
        /// <summary>
        /// Timer to defer evaluation of the filter until user has stopped typing.
        /// </summary>
        private readonly DispatcherTimer deferFilterEvaluationTimer;

        /// <summary>
        /// Create a new filter for the given data grid.
        /// </summary>
        /// <param name="dataGrid">The data grid to filter.</param>
        internal DataGridFilterHost(DataGrid dataGrid)
        {
            if (dataGrid == null)
                throw new ArgumentNullException("dataGrid");

            this.dataGrid = dataGrid;
            this.deferFilterEvaluationTimer = new DispatcherTimer(TimeSpan.FromSeconds(0.3), DispatcherPriority.Input, (_, __) => EvaluateFilter(), Dispatcher.CurrentDispatcher);
        }

        /// <summary>
        /// Clear all existing filter conditions.
        /// </summary>
        public void Clear()
        {
            filterColumns.ForEach(filter => filter.Filter = null);
            EvaluateFilter();
        }

        /// <summary>
        /// When any filter condition has changed restart the evaluation timer to defer 
        /// the evaluation until the user has stopped typing.
        /// </summary>
        internal void FilterChanged()
        {
            deferFilterEvaluationTimer.Restart();
        }

        /// <summary>
        /// Adds a new collumn.
        /// </summary>
        /// <param name="filterColumn"></param>
        internal void AddColumn(DataGridFilterColumnControl filterColumn)
        {
            filterColumns.Add(filterColumn);
        }

        /// <summary>
        /// Removes an unloaded column.
        /// </summary>
        internal void RemoveColumn(DataGridFilterColumnControl filterColumn)
        {
            filterColumns.Remove(filterColumn);
        }

        /// <summary>
        /// Creates a new content filter.
        /// </summary>
        internal IContentFilter CreateContentFilter(object content)
        {
            return DataGridFilter.GetContentFilterFactory(dataGrid).Create(content);
        }

        /// <summary>
        /// Evaluates the current filters and applies the filtering to the collection view of the items control.
        /// </summary>
        private void EvaluateFilter()
        {
            deferFilterEvaluationTimer.Stop();

            // Get the collection view of the grids ItemsSource
            var itemsSource = dataGrid.ItemsSource;
            if (itemsSource == null)
                return;
            var collectionView = CollectionViewSource.GetDefaultView(itemsSource);

            // Collect all active filters of all known columns.
            var filters = filterColumns.Where(column => column.IsFiltered).ToArray();

            // Apply filter to collection view
            if (!filters.Any())
            {
                collectionView.Filter = null;
            }
            else
            {
                collectionView.Filter = (item) => filters.All(filter => filter.Matches(item));
            }

            // Notify all filters about the change of the collection view.
            filterColumns.ForEach(filter => filter.ValuesUpdated());
        }
    }
}
