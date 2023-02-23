using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Actuelice1.clases.vista
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("hola");
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            //if (comboboxSecciones.SelectedIndex > -1)
            //{
            //    ComboBoxItem cbi = (ComboBoxItem)comboboxSecciones.SelectedItem;
            //    StackPanel sp = (StackPanel)cbi.Content;
            //    TextBlock tb = (TextBlock)sp.Children[1];
            //    MessageBox.Show(tb.Text);
            //}
            //ComboBoxItem cbi= (ComboBoxItem)comboboxSecciones.SelectedItem;
            //StackPanel sp = (StackPanel)cbi.Content;
            //TextBlock tb = (TextBlock)sp.Children[1];
            //MessageBox.Show(tb.Text);

            //MessageBox.Show(comboboxSecciones.SelectedIndex+"");
        }
    }
}
