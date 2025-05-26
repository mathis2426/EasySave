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
using ControllerModel.Jobs;

namespace WPFApp
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private Frame _mainFrame;
        private JobManager _jobManager;
        public HomePage(Frame mainFrame, JobManager jobManager)
        {
            InitializeComponent();
            HomePageViewModel homePageViewModel = new HomePageViewModel(jobManager);
            DataContext = homePageViewModel;
            _mainFrame = mainFrame;
            _jobManager = jobManager;
        }

        private void ButtonLeave_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void ButtonCreateJob_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new CreateJob(_mainFrame, _jobManager));
        }
        private void ButtonDeleteJob_Click(object sender, RoutedEventArgs e)
        {
            //_mainFrame.Navigate(new DeleteJob(_mainFrame));
        }
        private void ButtonManageJob_Click(object sender, RoutedEventArgs e)
        {
            var selectedJob = ((HomePageViewModel)DataContext).SelectedJob;

            if (selectedJob != null)
            {
                _mainFrame.Navigate(new ManageJob(selectedJob));
            }


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
