using ControllerModel.Jobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFApp
{
    public class JobViewModel : INotifyPropertyChanged
    {
        private double _progressValue;
        public JobObj Job { get; }

        public string NameString => Job.Name;
        public string TypeBackupString => Job.Type.ToString();

        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }

        public ICommand PauseCommand { get; }
        public ICommand ResumeCommand { get; }
        public ICommand StopCommand { get; }

        public JobViewModel(JobObj job)
        {
            Job = job;

            PauseCommand = new RelayCommand(() =>
            {
                if (JobManager.threadsByJob.TryGetValue(Job.Id, out var jobInfo))
                    jobInfo.PauseEvent.Reset(); // pause
            });

            ResumeCommand = new RelayCommand(() =>
            {
                if (JobManager.threadsByJob.TryGetValue(Job.Id, out var jobInfo))
                    jobInfo.PauseEvent.Set(); // resume
            });

            StopCommand = new RelayCommand(() =>
            {
                if (JobManager.threadsByJob.TryGetValue(Job.Id, out var jobInfo))
                {
                    jobInfo.TokenSource.Cancel(); // stop
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}
