using ControllerModel.Jobs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WPFApp;

public class ManageAllJobViewModel : INotifyPropertyChanged
{
    private readonly JobManager _jobManager = new();
    //public ObservableCollection<JobViewModel> Jobs { get; set; } = new();

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

    public ManageAllJobViewModel()
    {
        // Convertir chaque JobObj en JobViewModel
        foreach (var job in _jobManager.JobList)
        {
            //Jobs.Add(new JobViewModel(job));
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