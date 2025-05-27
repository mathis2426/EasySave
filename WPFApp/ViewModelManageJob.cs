using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using ControllerModel.Jobs;
using ControllerModel.LanguagesHelper;

namespace WPFApp
{
    public class ViewModelManageJob : AbstractViewModel
    {

        private string _inputString;
        private string _outputString;
        private string _stateString;
        private string _nameString;
        private string _inputFileString;
        private string _outputFileString;
        private string _typeBackupString;

        private readonly LanguageManager languageManager = new();



        private int _inputJobID;
        private double _progressValue;
        private JobObj _job;
        public JobManager Controller = new();

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand ResumeCommand { get; }

        public string JobType => _job.Type.ToString();
        public string JobName => _job.Name;


        public ViewModelManageJob(JobObj Job) // constructor
        {
            _job = Job;
            StartCommand = new CommandHandler(() => StartJob(), CanStart);
            StopCommand = new CommandHandler(() => StopJob(), CanStop);
            ResumeCommand = new CommandHandler(() => ResumeJob(), CanResume);
            _progressValue = 72.8;
            _inputFileString = _job.SourcePath;
            _outputFileString = _job.TargetPath;
            _inputJobID = _job.Id;

            _selectedLanguage = languageManager.saveConfigObj.Language;

        }
        public string InputString
        {
            get => _inputString; // getter
            set // setter
            {
                _inputString = value;
                OnPropertyChanged(nameof(InputString));
            }
        }

        public string OutputString
        {
            get => _outputString;
            set
            {
                _outputString = value;
                OnPropertyChanged(nameof(OutputString));
            }
        }

        public string NameString
        {
            get => _nameString;
            set
            {
                _nameString = value;
                OnPropertyChanged(nameof(NameString));
            }
        }
        public string InputFileString
        {
            get => _inputFileString;
            set
            {
                _inputFileString = value;
                OnPropertyChanged(nameof(InputFileString));
            }
        }
        public string OutputFileString
        {
            get => _outputFileString;
            set
            {
                _outputFileString = value;
                OnPropertyChanged(nameof(OutputFileString));
            }
        }

        public string StateString
        {
            get => _stateString;
            set
            {
                _stateString = value;
                OnPropertyChanged(nameof(StateString));
            }
        }
        public string TypeBackupString
        {
            get => _typeBackupString;
            set
            {
                _typeBackupString = value;
                OnPropertyChanged(nameof(TypeBackupString));
            }
        }

        public int JobID
        {
            get => _inputJobID; // getter
            set // setter
            {
                _inputJobID = value;
                OnPropertyChanged(nameof(JobID));
            }
        }
        private void StartJob()
        {
            OutputString = $"Demarrage du job {JobID}";
            int result = Controller.LaunchBackup(JobID);

        }

        private void StopJob()
        {

            int result = Controller.LaunchBackup(JobID);/////////////: à faiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiire
            OutputString = $"Demarrage du job {result}";
        }

        private void ResumeJob()
        {
            int result = Controller.LaunchBackup(JobID);/////////////: à faiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiire
            OutputString = $"Demarrage du job {result}";
        }

        private bool CanStart()
        {

            if (StateString != "Active" && StateString != "Stopped")
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }
        private bool CanStop()
        {

            if (StateString == "Active")
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }
        private bool CanResume()
        {

            if (StateString == "Stopped")
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }


        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }

        public string JobNameLabel => languageManager.Get("job_name");
        public string StartJobLabel => languageManager.Get("start_job");
        public string PauseJobLabel => languageManager.Get("pause_job");
        public string ResumeJobLabel => languageManager.Get("resume_job");
        public string ExitLabel => languageManager.Get("Exit");
        public string InputFileLabel => languageManager.Get("job_source");
        public string OutputFileLabel => languageManager.Get("job_target");
        public string PriorityFileLabel => languageManager.Get("priority_files");
        public string JobTypeLabel => languageManager.Get("job_type");
        public string Language => languageManager.Get("language");
        public string PercentageCompleted => languageManager.Get("percentage_completed");

        private void RefreshTranslations()
        {
            OnPropertyChanged(nameof(JobName));
            OnPropertyChanged(nameof(StartJobLabel));
            OnPropertyChanged(nameof(PauseJobLabel));
            OnPropertyChanged(nameof(ResumeJobLabel));
            OnPropertyChanged(nameof(ExitLabel));
            OnPropertyChanged(nameof(InputFileLabel));
            OnPropertyChanged(nameof(OutputFileLabel));
            OnPropertyChanged(nameof(PriorityFileLabel));
            OnPropertyChanged(nameof(JobTypeLabel));
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(PercentageCompleted));

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
