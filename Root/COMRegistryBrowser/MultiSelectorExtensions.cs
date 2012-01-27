using System.Collections;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;

namespace AVL.Core.WPF
{
    /// <summary>
    /// Extensions for multi selectors like ListBox or DataGrid:
    /// <list type="bullet">
    /// <item>Support binding operations with SelectedItems property.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Since there is no common interface for ListBox and DataGrid, the SelectionBinding is implemented via reflection/dynamics, so it will
    /// work on any FrameworkElement that has the SelectedItems, SelectedItem and SelectedItemIndex properties and the SelectionChanged event.
    /// </remarks>
    public static class MultiSelectorExtensions
    {
        // Simple recursion blocking. Change events should appear only on the UI thread, so a static bool will do the job.
        private static bool selectionBindingIsUpdatingTarget;
        private static bool selectionBindingIsUpdatingSource;

        /// <summary>
        /// Gets the value of the <see cref="SelectionBindingProperty"/> property.
        /// </summary>
        /// <param name="obj">The object to attach to.</param>
        /// <returns>The current selection.</returns>
        public static IList GetSelectionBinding(DependencyObject obj)
        {
            Contract.Requires(obj != null);
            return (IList)obj.GetValue(SelectionBindingProperty);
        }
        /// <summary>
        /// Sets the value of the <see cref="SelectionBindingProperty"/> property.
        /// </summary>
        /// <param name="obj">The object to attach to.</param>
        /// <param name="value">The new selection.</param>
        public static void SetSelectionBinding(DependencyObject obj, IList value)
        {
            Contract.Requires(obj != null);
            obj.SetValue(SelectionBindingProperty, value);
        }
        /// <summary>
        /// Identifies the SelectionBinding dependency property.
        /// <para/>
        /// Attach this property to a ListBox or DataGrid to bind the selectors SelectedItems property to the view models SelectedItems property.
        /// </summary>
        /// <example>
        /// If your view model has two properties "AnyList Items { get; }" and "IList SelectedItems { get; set; }" your XAML looks like this:
        /// <![CDATA[ 
        /// <ListBox ItemsSource="{Binding Path=Items}" core:MultiSelectorExtensions.SelectionBinding="{Binding Path=SelectedItems}"/>
        /// ]]>
        /// </example>
        public static readonly DependencyProperty SelectionBindingProperty =
            DependencyProperty.RegisterAttached("SelectionBinding", typeof(IList), typeof(MultiSelectorExtensions), new FrameworkPropertyMetadata(new object[0], FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectionBinding_Changed));

        [ContractVerification(false)] // Contracts get confused by dynamic variables.
        private static void SelectionBinding_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Contract.Requires(d != null);
            Contract.Requires(d is FrameworkElement);

            if (!selectionBindingIsUpdatingSource)
            {
                var selector = (FrameworkElement)d;

                var eventInfo = selector.GetType().GetEvent("SelectionChanged");
                if (eventInfo != null)
                {
                    // Simply remove and add again, so we don't need to track if we have already attached the event.
                    eventInfo.RemoveEventHandler(selector, (SelectionChangedEventHandler)selector_SelectionChanged);
                    eventInfo.AddEventHandler(selector, (SelectionChangedEventHandler)selector_SelectionChanged);
                }

                var dynamicSelector = (dynamic)selector;

                selectionBindingIsUpdatingTarget = true;

                dynamicSelector.SelectedIndex = -1;

                var target = (IList)e.NewValue;

                if (target != null)
                {
                    if (target.Count == 1)
                    {
                        // Special handling, maybe listbox is in single selection mode, so this will work either.
                        dynamicSelector.SelectedItem = target[0];
                    }
                    else
                    {
                        var selectedItems = (IList)dynamicSelector.SelectedItems;

                        foreach (var item in target)
                        {
                            selectedItems.Add(item);
                        }
                    }
                }

                selectionBindingIsUpdatingTarget = false;
            }
        }

        [ContractVerification(false)] // Contracts get confused by dynamic variables.
        private static void selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!selectionBindingIsUpdatingTarget)
            {
                dynamic selector = sender;

                if (selector != null)
                {
                    selectionBindingIsUpdatingSource = true;
                    selector.SetValue(SelectionBindingProperty, selector.SelectedItems);
                    selectionBindingIsUpdatingSource = false;
                }
            }
        }
    }
}
