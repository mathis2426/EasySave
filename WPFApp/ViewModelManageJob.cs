using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        private int _stateString;
        private string _nameString;
        private string _inputFileString;
        private string _outputFileString;
        private string _typeBackupString;

        private LanguageManager languageManager = new();



        private int _inputJobID;
        private double _progressValue;
        private JobObj _job;
        public JobManager Controller = new();

        public CommandHandler StartCommand { get; }
        public CommandHandler StopCommand { get; }
        public CommandHandler PauseCommand { get; }
        public CommandHandler ResumeCommand { get; }

        public string JobType => _job.Type.ToString();
        public string JobName => _job.Name;
        public string PriorityFile => string.Join("\t", languageManager.saveConfigObj.ExtensionPriorityFile); 


        public ViewModelManageJob(JobObj Job) // constructor
        {
            _job = Job;
            StartCommand = new CommandHandler(() => StartJob(), CanStart);
            StopCommand = new CommandHandler(() => StopJob(), CanPauseOrStop);
            PauseCommand = new CommandHandler(() => PauseJob(), CanPauseOrStop);
            ResumeCommand = new CommandHandler(() => ResumeJob(), CanResume);
            _progressValue = 0;
            _inputFileString = _job.SourcePath;
            _outputFileString = _job.TargetPath;
            _inputJobID = _job.Id;
            _selectedLanguage = languageManager.saveConfigObj.Language;
            _nameString = _job.Name;
            _typeBackupString = _job.Type.ToString();




            if (JobManager.threadsByJob.TryGetValue(_job.Id, out var jobData))
            {
                _stateString = jobData.ButtonStatus;
                _progressValue = jobData.progressBarPercent;
            }
            else
            {
                _stateString = 0;
                _progressValue = 0;
            }

            if (!ExecuteBackup.ProgressDelegatesByJobId.ContainsKey(_job.Id))
                ExecuteBackup.ProgressDelegatesByJobId[_job.Id] = null;

            ExecuteBackup.ProgressDelegatesByJobId[_job.Id] += OnProgressChanged;

            if (!ExecuteBackup.StatusDelegatesByJobId.ContainsKey(_job.Id))
                ExecuteBackup.StatusDelegatesByJobId[_job.Id] = null;

            ExecuteBackup.StatusDelegatesByJobId[_job.Id] += OnStatusChanged;
        
        }

        private void OnProgressChanged(double progress)
        {
            OutputString = $"Changement de la barre de progress {progress}";
            ProgressValue = progress;
            ProgressValue = JobManager.threadsByJob[_job.Id].progressBarPercent;
            this.OnPropertyChanged(nameof(ProgressValue));
        }

        public string Stop => languageManager.Get("Stop");
        public string Pause => languageManager.Get("Pause");
        public string Resume => languageManager.Get("Resume");
        public string Start => languageManager.Get("Start");
        public string Type => languageManager.Get("Type");
        public string NameLabel => languageManager.Get("Name");

        private void OnStatusChanged(int status)
        {
            StateString = status;
            StateString = JobManager.threadsByJob[_job.Id].ButtonStatus;
            this.OnPropertyChanged(nameof(StateString));
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

        public int StateString
        {
            get => _stateString;
            set
            {
                _stateString = value;
                OnPropertyChanged(nameof(StateString));
                StartCommand?.RaiseCanExecuteChanged();
                StopCommand?.RaiseCanExecuteChanged();
                StartCommand?.RaiseCanExecuteChanged();
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
            
            int result = Controller.LaunchBackup(JobID);
            OutputString = $"Demarrage du job {JobID}";

        }

        private void PauseJob() 
        {

            if (JobManager.threadsByJob.TryGetValue(JobID, out var threadInfo))
            {
                threadInfo.PauseEvent.Reset();
                OutputString = $"Pause du job {JobID}";
            }
            
        }

        private void StopJob()
        {

            if (JobManager.threadsByJob.TryGetValue(JobID, out var threadInfo))
            {
                threadInfo.TokenSource.Cancel();
                OutputString = $"Arrêt du job {JobID}"; ;
            }

        }

        private void ResumeJob() 
        {
            if (JobManager.threadsByJob.TryGetValue(JobID, out var threadInfo))
            {
                threadInfo.PauseEvent.Set(); 
                OutputString = $"Redémarrage du job : {JobID}";
            }
        }

        private bool CanStart()
        {
            Debug.WriteLine("CanStart called with state: " + StateString);
            if (StateString != 1 && StateString != 2)
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }
        private bool CanPauseOrStop() 
        {

            if (StateString == 1)
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

            if (StateString == 2)
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
        public string StopJobLabel => languageManager.Get("stop_job");

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
