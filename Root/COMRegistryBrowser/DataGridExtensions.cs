using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Controls;

namespace ComBrowser
{
    internal static class DataGridExtensions
    {
        private static MethodInfo performSortMethod = typeof(DataGrid).GetMethod("PerformSort", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void SortBy(this DataGrid dataGrid, string propertyName)
        {
            performSortMethod.Invoke(dataGrid, new[] { dataGrid.Columns.First(col => col.SortMemberPath == propertyName) });
        }
    }
}
