using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControllerModel.Jobs;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly JobManager _jobManager;
        public MainWindow()
        {
            InitializeComponent();
            _jobManager = new JobManager();
            MainFrame.Navigate(new HomePage(MainFrame, _jobManager));
        }
    }
}