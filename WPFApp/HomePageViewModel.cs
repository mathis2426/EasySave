using ControllerModel.Jobs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace WPFApp
{
    public class HomePageViewModel : AbstractViewModel
    {
        private readonly JobManager _jobManager = new JobManager();
        private string _outputText = string.Empty;
        private JobObj _selectedJob;

        public ObservableCollection<JobObj> JobsList { get; } = new ObservableCollection<JobObj>();

        public JobObj SelectedJob
        {
            get => _selectedJob;
            set
            {
                if (_selectedJob != value)
                {
                    _selectedJob = value;
                    OnPropertyChanged(nameof(SelectedJob));
                    // On actualise la commande si besoin
                    // CommandManager.InvalidateRequerySuggested();
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

        public ICommand DeleteJobCommand { get; }

        public HomePageViewModel()
        {
            // Charger la liste initiale des jobs depuis JobManager
            List<JobObj> jobsList = _jobManager.JobList;
            
            foreach (var job in jobsList)
            {
                OutputText = $"Nom de jobs : {job.Name}";
                JobsList.Add(job);
            }

            //DeleteJobCommand = new CommandHandler(DeleteJob, CanDeleteJob);
        }

        private bool CanDeleteJob()
        {
            return SelectedJob != null;
        }

        private void DeleteJob()
        {
            if (SelectedJob == null)
                return;

            int index = JobsList.IndexOf(SelectedJob);
            if (index >= 0)
            {
                _jobManager.JobDeletion(index);
                JobsList.Remove(SelectedJob);
                SelectedJob = null;
            }
        }
    }
}