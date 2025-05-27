using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ControllerModel.Jobs;

namespace WPFApp
{
    public class ViewModelManageJob : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _inputString;
        private string _outputString;
        private int _stateString;
        private string _nameString;
        private string _inputFileString;
        private string _outputFileString;
        private string _typeBackupString;
        

        private int _inputJobID;
        private double _progressValue;
        private JobObj _job;
        public JobManager Controller = new();

        public CommandHandler StartCommand { get; }
        public CommandHandler StopCommand { get; }
        public CommandHandler PauseCommand { get; }
        public CommandHandler ResumeCommand { get; }


        public ViewModelManageJob(JobObj Job) // constructor
        {
            _job = Job;
            StartCommand = new CommandHandler(() => StartJob(), CanStart);
            StopCommand = new CommandHandler(() => StopJob(), CanPauseOrStop);
            PauseCommand = new CommandHandler(() => PauseJob(), CanPauseOrStop);
            ResumeCommand = new CommandHandler(() => ResumeJob(), CanResume);
            _progressValue = 0;
            _nameString = "Job name : " + _job.Name;
            _inputFileString = _job.SourcePath;
            _outputFileString = _job.TargetPath;
            _typeBackupString = "Job type : " + _job.Type.ToString();
            _inputJobID = _job.Id;

            

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
                StartCommand?.RaiseCanExecuteChanged();
                StopCommand?.RaiseCanExecuteChanged();
                StartCommand?.RaiseCanExecuteChanged();
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
                return(true);
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

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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


    }
    
   
}
