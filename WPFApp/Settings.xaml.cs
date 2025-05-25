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
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class Settings : Page
    {
        private Frame _mainFrame;
        public Settings(Frame mainFrame)
        {
            InitializeComponent();
            SettingsViewModel settingsViewModel = new SettingsViewModel();
            DataContext = settingsViewModel;
        }
    }
}
