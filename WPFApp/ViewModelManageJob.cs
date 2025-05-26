using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private string _stateString;
        private string _nameString;
        private string _inputFileString;
        private string _outputFileString;
        private string _typeBackupString;
        

        private int _inputJobID;
        private double _progressValue;
        private JobObj _job;
        public JobManager Controller = new();

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand ResumeCommand { get; }


        public ViewModelManageJob(JobObj Job) // constructor
        {
            _job = Job;
            StartCommand = new CommandHandler(() => StartJob(), CanStart);
            StopCommand = new CommandHandler(() => StopJob(), CanStop);
            ResumeCommand = new CommandHandler(() => ResumeJob(), CanResume);
            _progressValue = 72.8;
            _nameString = "Job name : " + _job.Name;
            _inputFileString = _job.SourcePath;
            _outputFileString = _job.TargetPath;
            _typeBackupString = "Job type : " + _job.Type.ToString();

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
            int result = Controller.LaunchBackup(JobID);
            OutputString = $"Demarrage du job {result}";
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
                return(true);
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
