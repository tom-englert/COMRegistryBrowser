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
using System.Windows.Shapes;
using System.Collections;

namespace COMRegistryBrowser
{
    /// <summary>
    /// Interaction logic for UnregisterDialog.xaml
    /// </summary>
    public partial class UnregisterDialog : Window
    {
        public UnregisterDialog()
        {
            InitializeComponent();
        }

        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        /// <summary>
        /// Identifies the Items dependency property
        /// </summary>
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable), typeof(UnregisterDialog));

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
       
    }
}
