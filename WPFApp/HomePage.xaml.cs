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
using ControllerModel.LanguagesHelper;
using System.Globalization;

namespace WPFApp
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private Frame _mainFrame;
        public HomePage(Frame mainFrame)
        {
            InitializeComponent();
            HomePageViewModel homePageViewModel = new HomePageViewModel();
            DataContext = homePageViewModel;
            _mainFrame = mainFrame;
        }

        private void ButtonLeave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonCreateJob_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new CreateJob(_mainFrame));
        }
        private void ButtonDeleteJob_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonManageJob_Click(object sender, RoutedEventArgs e)
        {

            _mainFrame.Navigate(new ManageJob());

        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {

            _mainFrame.Navigate(new Settings(_mainFrame));

        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void outputBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
