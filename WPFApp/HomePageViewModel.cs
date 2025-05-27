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

        public ObservableCollection<JobObj> JobsList { get; set; } = new ObservableCollection<JobObj>();

        public HomePageViewModel(JobManager jobManager)
        {
            _jobManager = jobManager;
            // Load initial job list from JobManager

            foreach (var job in _jobManager.JobList)
            {
                JobsList.Add(job);
            }


            DeleteJobCommand = new CommandHandler(
                execute: DeleteJob,
                canExecute: () => SelectedJob != null
            );
            ManageJobCommand = new CommandHandler(
                execute: () => ManageJob(),
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
                    ManageJobCommand.RaiseCanExecuteChanged();
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

        public string Exit => languageManager.Get("Exit");
        public string Language => languageManager.Get("Language");
        public string CreateJob => languageManager.Get("CreateJob");
        public string DeleteJobName => languageManager.Get("DeleteJob");
        public string ManageJobName => languageManager.Get("ManageJob");
        public string ID => languageManager.Get("ID");
        public string Name => languageManager.Get("Name");
        public string SourcePath => languageManager.Get("SourcePath");
        public string TargetPath => languageManager.Get("TargetPath");
        public string Type => languageManager.Get("Type");



        private void RefreshTranslations()
        {
            OnPropertyChanged(nameof(Exit));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(ID));
            OnPropertyChanged(nameof(SourcePath));
            OnPropertyChanged(nameof(TargetPath));
            OnPropertyChanged(nameof(Type));
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(CreateJob));
            OnPropertyChanged(nameof(DeleteJobName));
            OnPropertyChanged(nameof(ManageJobName));
            OnPropertyChanged(nameof(DeleteExtensionCommand));
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
        public CommandHandler ManageJobCommand { get; }
        private void ManageJob() { }
    }
}