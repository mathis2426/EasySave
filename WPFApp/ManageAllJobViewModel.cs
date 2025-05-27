using ControllerModel.Jobs;
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
    public class ManageAllJobViewModel : INotifyPropertyChanged
    {
        private readonly JobManager _jobManager = new();
        public ObservableCollection<JobViewModel> Jobs { get; set; } = new();

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

        public string PriorityFile => string.Join("\t", languageManager.saveConfigObj.ExtensionPriorityFile); 


        public ManageAllJobViewModel() // constructor
        {
            
            StartCommand = new CommandHandler(() => StartJob(), CanStart);
            StopCommand = new CommandHandler(() => StopJob(), CanPauseOrStop);
            PauseCommand = new CommandHandler(() => PauseJob(), CanPauseOrStop);
            ResumeCommand = new CommandHandler(() => ResumeJob(), CanResume);  
        
        }

        private void OnProgressChanged(double progress)
        {
            OutputString = $"Changement de la barre de progress {progress}";
            ProgressValue = progress;
            ProgressValue = JobManager.threadsByJob[_job.Id].progressBarPercent;
            this.OnPropertyChanged(nameof(ProgressValue));
        }


            
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
        public ICommand StartCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand ResumeCommand { get; }
        public ICommand StopCommand { get; }

        private string _outputString;
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

        public ManageAllJobViewModel()
            int result = Controller.LaunchBackup(JobID);
            OutputString = $"Demarrage du job {JobID}";

        }

        private void PauseJob() 
        {
            // Convertir chaque JobObj en JobViewModel
            foreach (var job in _jobManager.JobList)

            if (JobManager.threadsByJob.TryGetValue(JobID, out var threadInfo))
            {
                Jobs.Add(new JobViewModel(job));
                threadInfo.PauseEvent.Reset();
                OutputString = $"Pause du job {JobID}";
            }

            StartCommand = new RelayCommand(StartAllJobs);
            PauseCommand = new RelayCommand(PauseAllJobs);
            ResumeCommand = new RelayCommand(ResumeAllJobs);
            StopCommand = new RelayCommand(StopAllJobs);
        }

        private void StartAllJobs()
        {
            _jobManager.LaunchBackup(0); // 0 = tous les jobs
            OutputString = "Tous les jobs ont démarré.";
        }

        private void PauseAllJobs()
        {
            foreach (var kvp in JobManager.threadsByJob)
            {
                kvp.Value.PauseEvent.Reset(); // Pause
            }
            OutputString = "Tous les jobs sont en pause.";
        }

        private void ResumeAllJobs()
        {
            foreach (var kvp in JobManager.threadsByJob)
            {
                kvp.Value.PauseEvent.Set(); // Resume
            }
            OutputString = "Tous les jobs ont repris.";
        }

        private void StopAllJobs()
        {
            foreach (var kvp in JobManager.threadsByJob)
            {
                kvp.Value.TokenSource.Cancel(); // Stop
            }
            OutputString = "Tous les jobs ont été arrêtés.";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

   
}
