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
using System.IO;

namespace WPFApp
{
    /// <summary>
    /// Logique d'interaction pour CreateJob.xaml
    /// </summary>
    public partial class CreateJob : Page
    {
        private Frame _mainFrame;

        private JobManager _jobManager;
        public CreateJob(Frame mainFrame, JobManager jobManager)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _jobManager = jobManager;

            TypeComboBox.ItemsSource = Enum.GetValues(typeof(JobType));
            TypeComboBox.SelectedIndex = 0;
        }

        public bool AreFieldsFilled(System.Windows.Controls.TextBox jobNameTextBox, System.Windows.Controls.TextBox sourcePathTextBox, System.Windows.Controls.TextBox targetPathTextBox)
        {
            if (string.IsNullOrWhiteSpace(jobNameTextBox.Text))
            {
                System.Windows.MessageBox.Show("Le champ 'Nom du job' est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                jobNameTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(sourcePathTextBox.Text))
            {
                System.Windows.MessageBox.Show("Le champ 'Chemin source du job' est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                sourcePathTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(targetPathTextBox.Text))
            {
                System.Windows.MessageBox.Show("Le champ 'Chemin de destination du job' est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                targetPathTextBox.Focus();
                return false;
            }

            return true;
        }

        public bool ArePathsValid(System.Windows.Controls.TextBox sourcePathTextBox, System.Windows.Controls.TextBox targetPathTextBox)
        {
            if (!IsValidExistingDirectory(sourcePathTextBox.Text))
            {
                System.Windows.MessageBox.Show("Le chemin source n'est pas valide ou n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                sourcePathTextBox.Focus();
                return false;
            }

            if (!IsValidExistingDirectory(targetPathTextBox.Text))
            {
                System.Windows.MessageBox.Show("Le chemin de destination n'est pas valide ou n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                targetPathTextBox.Focus();
                return false;
            }

            return true;
        }

        // Méthode auxiliaire
        private bool IsValidExistingDirectory(string path)
        {
            try
            {
                string fullPath = System.IO.Path.GetFullPath(path);
                return Directory.Exists(fullPath);
            }
            catch
            {
                return false;
            }
        }

        private void ButtonValidate_ClickJobCreation(object sender, RoutedEventArgs e)
        {
            string name = JobName.Text;
            string source = SourcePath.Text;
            string target = TargetPath.Text;
            JobType selectedType = (JobType)TypeComboBox.SelectedItem;

            if (!AreFieldsFilled(JobName, SourcePath, TargetPath))
                return;

            if (!ArePathsValid(SourcePath, TargetPath))
                return;

            _jobManager.JobCreation(name, source, target, selectedType);

            _mainFrame.Navigate(new HomePage(_mainFrame, _jobManager));
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
