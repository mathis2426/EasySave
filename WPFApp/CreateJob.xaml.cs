using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Logique d'interaction pour CreateJob.xaml
    /// </summary>
    public partial class CreateJob : Page
    {
        private Frame _mainFrame;
        public CreateJob(Frame mainFrame)
        {
            InitializeComponent();
           _mainFrame = mainFrame;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ButtonValidate_ClickJobCreation(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void ButtonLeave_ClickJobCreation(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new HomePage(_mainFrame));
        }
        
        private void ButtonSourcePath_ClickJobCreation(object source, RoutedEventArgs e)
        {
            Process.Start("explorer");
        }

        private void ButtonTargetPath_ClickJobCreation(object target, RoutedEventArgs e) 
        {
            Process.Start("explorer");
        }
    }
}
