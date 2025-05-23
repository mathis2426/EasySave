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
        private int _inputJobID;

        public JobManager Controller = new();

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand ResumeCommand { get; }


        public ViewModelManageJob() // constructeur 
        {
            StartCommand = new CommandHandler(() => StartJob(), CanStart); // Elle est liée à la méthode ConvertToUpper(), mais seulement si CanConvert() retourne true (exemple : input non vide).
            StopCommand = new CommandHandler(() => StopJob(), CanStop);
            ResumeCommand = new CommandHandler(() => ResumeJob(), CanResume);
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

        public string StateString
        {
            get => _stateString;
            set
            {
                _stateString = value;
                OnPropertyChanged(nameof(StateString));
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
        private void StartJob() //appel au model
        {
            int result = Controller.LaunchBackup(JobID);
            OutputString = $"Demarrage du job {result}";
        }

        private void StopJob() //appel au model
        {
            int result = Controller.LaunchBackup(JobID);/////////////: à faiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiire
            OutputString = $"Demarrage du job {result}";
        }

        private void ResumeJob() //appel au model
        { 
            int result = Controller.LaunchBackup(JobID);/////////////: à faiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiire
            OutputString = $"Demarrage du job {result}";
        }

        private bool CanStart() //valide si le bouton est cliquable, si le input est vide, le bouton est grisé
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
        private bool CanStop() //valide si le bouton est cliquable, si le input est vide, le bouton est grisé
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
        private bool CanResume() //valide si le bouton est cliquable, si le input est vide, le bouton est grisé
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
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) // appelé quand une propriété change
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    public class CommandHandler : ICommand //implémentation de ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public CommandHandler(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }
        public bool CanExecute(object? parameter) => _canExecute();
        public void Execute(object? parameter) => _execute();

        public event EventHandler? CanExecuteChanged;
    }
   
}
