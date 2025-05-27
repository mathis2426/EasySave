using ControllerModel.Jobs;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ControllerModel.LanguagesHelper;
using System.Windows.Controls;


namespace WPFApp
{
    public class CreateJobViewModel : AbstractViewModel
    {
        private readonly JobManager _jobManager;
        private readonly Frame _mainFrame;
        private readonly LanguageManager languageManager = new();

        public CreateJobViewModel(JobManager jobManager, Frame mainFrame)
        {
            _jobManager = jobManager;
            _mainFrame = mainFrame;
            JobTypes = new ObservableCollection<JobType>((JobType[])Enum.GetValues(typeof(JobType)));
            SelectedJobType = JobType.Full;

            BrowseSourceCommand = new RelayCommand(BrowseSource);
            BrowseTargetCommand = new RelayCommand(BrowseTarget);
            ValidateCommand = new RelayCommand(Validate);
            ExitCommand = new RelayCommand(Exit);
            _selectedLanguage = languageManager.saveConfigObj.Language;
        }

        public ObservableCollection<JobType> JobTypes { get; }

        private string _jobName;
        public string JobName
        {
            get => _jobName;
            set { _jobName = value; OnPropertyChanged(nameof(JobName)); }
        }

        private string _sourcePath;
        public string SourcePath
        {
            get => _sourcePath;
            set { _sourcePath = value; OnPropertyChanged(nameof(SourcePath)); }
        }

        private string _targetPath;
        public string TargetPath
        {
            get => _targetPath;
            set { _targetPath = value; OnPropertyChanged(nameof(TargetPath)); }
        }

        private JobType _selectedJobType;
        public JobType SelectedJobType
        {
            get => _selectedJobType;
            set { _selectedJobType = value; OnPropertyChanged(nameof(SelectedJobType)); }
        }

        public ICommand BrowseSourceCommand { get; }
        public ICommand BrowseTargetCommand { get; }
        public ICommand ValidateCommand { get; }
        public ICommand ExitCommand { get; }

        private void BrowseSource()
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Sélectionnez un dossier source"
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SourcePath = dialog.SelectedPath;
            }
        }

        private void BrowseTarget()
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Sélectionnez un dossier de destination"
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TargetPath = dialog.SelectedPath;
            }
        }

        private void Validate()
        {
            if (!AreFieldsFilled() || !ArePathsValid() || !ArePathsDifferent())
                return;

            _jobManager.JobCreation(JobName.Trim(), SourcePath.Trim(), TargetPath.Trim(), SelectedJobType);
            System.Windows.MessageBox.Show("Job created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _mainFrame.Navigate(new HomePage(_mainFrame, _jobManager));
        }

        private void Exit()
        {
            _mainFrame.GoBack();
        }

        private bool AreFieldsFilled()
        {
            if (string.IsNullOrWhiteSpace(JobName))
            {
                System.Windows.MessageBox.Show("Le champ 'Nom du job' est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(SourcePath))
            {
                System.Windows.MessageBox.Show("Le champ 'Chemin source' est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(TargetPath))
            {
                System.Windows.MessageBox.Show("Le champ 'Chemin de destination' est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private bool ArePathsValid()
        {
            if (!Directory.Exists(SourcePath))
            {
                System.Windows.MessageBox.Show("Le chemin source n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!Directory.Exists(TargetPath))
            {
                System.Windows.MessageBox.Show("Le chemin de destination n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private bool ArePathsDifferent()
        {
            if (string.Equals(SourcePath.Trim(), TargetPath.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                System.Windows.MessageBox.Show("Les chemins source et destination doivent être différents.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        public string ExitLabel => languageManager.Get("Exit");
        public string Browse => languageManager.Get("Browse");
        public string JobNameLabel => languageManager.Get("job_name");
        public string SourcePathLabel => languageManager.Get("select_source");
        public string TargetPathLabel => languageManager.Get("select_target");
        public string BackupTypeLabel => languageManager.Get("BackupType");
        public string Language => languageManager.Get("Language");
        public string ValidateLabel => languageManager.Get("ValidateLabel");

        private void RefreshTranslations()
        {
            OnPropertyChanged(nameof(ExitLabel));
            OnPropertyChanged(nameof(Browse));
            OnPropertyChanged(nameof(JobNameLabel));
            OnPropertyChanged(nameof(SourcePathLabel));
            OnPropertyChanged(nameof(TargetPathLabel));
            OnPropertyChanged(nameof(BackupTypeLabel));
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(ValidateLabel));
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                { 
                    _selectedLanguage = value;

                    languageManager.SetLanguage(_selectedLanguage);
                    RefreshTranslations();
                    OnPropertyChanged(nameof(SelectedLanguage));

                }
            }
        }

        public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>
        {
            "fr",
            "en-US"
        };
    }
}
