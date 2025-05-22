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

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ButtonLeave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonAddJob_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonDeleteJob_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonManageJob_Click(object sender, RoutedEventArgs e)
        {

            MainFrame.Navigate(new ManageJob());

        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}