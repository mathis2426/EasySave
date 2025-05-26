using ControllerModel.Jobs;
using ControllerModel.LanguagesHelper;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace WPFApp
{
    public class HomePageViewModel : AbstractViewModel
    {
        private readonly JobManager _jobManager = new JobManager();
        private readonly LanguageManager languageManager = new();
        private string _outputText = string.Empty;
        private JobObj _selectedJob;

        public ObservableCollection<JobObj> JobsList { get; } = new ObservableCollection<JobObj>();

        public HomePageViewModel()
        {
            // Charger la liste initiale des jobs depuis JobManager
            foreach (var job in _jobManager.JobList)
            {
                JobsList.Add(job);
            }

            // Initialiser la commande de suppression de job
            DeleteJobCommand = new CommandHandler(
                execute: DeleteJob,
                canExecute: () => SelectedJob != null
            );

            _selectedLanguage = languageManager.saveConfigObj.Language;
        }

        public JobObj SelectedJob
        {
            get => _selectedJob;
            set
            {
                if (_selectedJob != value)
                {
                    _selectedJob = value;
                    OnPropertyChanged(nameof(SelectedJob));
                    DeleteJobCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string OutputText
        {
            get => _outputText;
            set
            {
                if (_outputText != value)
                {
                    _outputText = value;
                    OnPropertyChanged();
                }
            }
        }

        public CommandHandler DeleteJobCommand { get; }
        private void DeleteJob()
        {
            if (SelectedJob == null)
                return;

            int index = JobsList.IndexOf(SelectedJob);
            if (index >= 0)
            {
                _jobManager.JobDeletion(index);
                OutputText = $"Job supprimé : {SelectedJob.Name}";
                JobsList.RemoveAt(index);
                SelectedJob = null;
            }
        }
        public CommandHandler DeleteExtensionCommand { get; }

        private void RefreshTranslations()
        {
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
                    OnPropertyChanged();
                    OutputText = $"{languageManager.Get("language_changed")} : {_selectedLanguage}";

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