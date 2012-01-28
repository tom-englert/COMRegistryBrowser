using System.Collections;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;

namespace COMRegistryBrowser
{
    /// <summary>
    /// Extensions for multi selectors like ListBox or DataGrid:
    /// <list type="bullet">
    /// <item>Support binding operations with SelectedItems property.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// SelectionBinding:
    /// <para/>
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
        /// <para/>
        /// <![CDATA[ 
        /// <ListBox ItemsSource="{Binding Path=Items}" src:MultiSelectorExtensions.SelectionBinding="{Binding Path=SelectedItems}"/>
        /// ]]>
        /// </example>
        public static readonly DependencyProperty SelectionBindingProperty =
            DependencyProperty.RegisterAttached("SelectionBinding", typeof(IList), typeof(MultiSelectorExtensions), new FrameworkPropertyMetadata(new object[0], FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectionBinding_Changed));

        [ContractVerification(false)] // Contracts get confused by dynamic variables.
        private static void SelectionBinding_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // The selector is the target of the binding, and the ViewModel property is the source.

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

                var bindingTarget = (dynamic)selector;

                selectionBindingIsUpdatingTarget = true;

                // Updating this direction is a rare case, usually happens only once.
                // Use a very simple approach to update the target - just clear the list and then add all selected again.
                bindingTarget.SelectedIndex = -1;

                var bindingSource = (IList)e.NewValue;

                if (bindingSource != null)
                {
                    if (bindingSource.Count == 1)
                    {
                        // Special handling, maybe listbox is in single selection mode, so this will work either.
                        bindingTarget.SelectedItem = bindingSource[0];
                    }
                    else
                    {
                        var selectedItems = (IList)bindingTarget.SelectedItems;

                        foreach (var item in bindingSource)
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
