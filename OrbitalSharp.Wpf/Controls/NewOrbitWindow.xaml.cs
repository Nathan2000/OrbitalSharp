using OrbitalSharp.Wpf.Classes;
using OrbitalSharp.Wpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OrbitalSharp.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for NewOrbitFormControl.xaml
    /// </summary>
    public partial class NewOrbitWindow : Window
    {
        public NewOrbitWindow()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
