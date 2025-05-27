using ControllerModel.Jobs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using WPFApp;
using System.Windows.Threading;

public class ManageAllJobViewModel : INotifyPropertyChanged
{
    private readonly JobManager _jobManager = new();

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

    // Propriétés de contrôle des boutons
    public bool CanStart => !JobManager.threadsByJob.Values.Any(v => v.ButtonStatus != 0);
    public bool CanPauseOrStop => JobManager.threadsByJob.Values.Any(v => v.ButtonStatus != 0);

    public ManageAllJobViewModel()
    {
        StartCommand = new RelayCommand(StartAllJobs, () => CanStart);
        PauseCommand = new RelayCommand(PauseAllJobs, () => CanPauseOrStop);
        ResumeCommand = new RelayCommand(ResumeAllJobs, () => CanPauseOrStop);
        StopCommand = new RelayCommand(StopAllJobs, () => CanPauseOrStop);

        // Optionnel : timer pour actualiser les états même sans interaction
        DispatcherTimer timer = new DispatcherTimer
        {
            Interval = System.TimeSpan.FromSeconds(1)
        };
        timer.Tick += (s, e) => RefreshButtonStates();
        timer.Start();
    }

    private void StartAllJobs()
    {
        _jobManager.LaunchBackup(0); // 0 = tous les jobs
        OutputString = "Tous les jobs ont démarré.";
        RefreshButtonStates();
    }

    private void PauseAllJobs()
    {
        foreach (var kvp in JobManager.threadsByJob)
        {
            kvp.Value.PauseEvent.Reset(); // Pause
        }
        OutputString = "Tous les jobs sont en pause.";
        RefreshButtonStates();
    }

    private void ResumeAllJobs()
    {
        foreach (var kvp in JobManager.threadsByJob)
        {
            kvp.Value.PauseEvent.Set(); // Resume
        }
        OutputString = "Tous les jobs ont repris.";
        RefreshButtonStates();
    }

    private void StopAllJobs()
    {
        foreach (var kvp in JobManager.threadsByJob)
        {
            kvp.Value.TokenSource.Cancel(); // Stop
        }
        OutputString = "Tous les jobs ont été arrêtés.";
        RefreshButtonStates();
    }

    public void RefreshButtonStates()
    {
        OnPropertyChanged(nameof(CanStart));
        OnPropertyChanged(nameof(CanPauseOrStop));

        // Notifie les RelayCommands pour qu'ils réévaluent CanExecute
        CommandManager.InvalidateRequerySuggested();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}


