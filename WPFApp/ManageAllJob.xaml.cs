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

namespace WPFApp
{
    /// <summary>
    /// Logique d'interaction pour ManageAllJob.xaml
    /// </summary>
    public partial class ManageAllJob : Page
    {

        ManageAllJobViewModel ManageAllJobViewModel;
        public ManageAllJob()
        {
            InitializeComponent();
            ManageAllJobViewModel = new ManageAllJobViewModel();
            DataContext = ManageAllJobViewModel;
        }

        private void ButtonLeave_ClickManageAllJob(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}
