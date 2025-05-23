using ControllerModel.Jobs;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Forms = System.Windows.Forms;
using ControllerModel.LanguagesHelper;
using System.Globalization;


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

            TypeComboBox.ItemsSource = Enum.GetValues(typeof(JobType));
            TypeComboBox.SelectedIndex = 0;
        }
        

        private void ButtonValidate_ClickJobCreation(object sender, RoutedEventArgs e)
        {
            string name = JobName.Text;
            string source = SourcePath.Text;
            string target = TargetPath.Text;
            JobType selectedType = (JobType)TypeComboBox.SelectedItem;

            BackupJob backupJob = new BackupJob();
            JobObj job = backupJob.CreateJob(name, source, target, selectedType);

            _mainFrame.Navigate(new HomePage(_mainFrame));
        }

        private void ButtonLeave_ClickJobCreation(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ButtonPath_ClickJobCreation(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn)
            {
                if (btn.Name == "ButtonSourcePath")
                {
                    using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                    {
                        dialog.Description = "Sélectionnez un dossier source";
                        dialog.ShowNewFolderButton = false;

                        var result = dialog.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                        {
                            SourcePath.Text = dialog.SelectedPath;  // Met à jour la TextBox SourcePath
                        }
                    }
                }
                else if (btn.Name == "ButtonTargetPath")
                {
                    using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                    {
                        dialog.Description = "Sélectionnez un dossier cible";
                        dialog.ShowNewFolderButton = true;

                        var result = dialog.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                        {
                            TargetPath.Text = dialog.SelectedPath;  // Met à jour la TextBox TargetPath
                        }
                    }
                }
            }
        }

    }
}
