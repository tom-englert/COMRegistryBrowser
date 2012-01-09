using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ComBrowser
{
    public static class FrameworkExtensions
    {
        public static T FindAncestor<T>(this DependencyObject item) where T : class
        {
            while (item != null)
            {
                var target = item as T;
                if (target != null)
                    return target;

                item = LogicalTreeHelper.GetParent(item) ?? VisualTreeHelper.GetParent(item);
            }

            return null;
        }
    }
}
