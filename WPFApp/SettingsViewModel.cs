using ControllerModel.Jobs;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WPFApp
{
    public class SettingsViewModel : AbstractViewModel
    {
        public readonly JobManager _jobManager = new JobManager();
        private ExtensionItem _selectedExtension;
        private string _newExtensionName;
        private string _outputText;
        
        public ObservableCollection<ExtensionItem> Extensions { get; } = new ObservableCollection<ExtensionItem>();
   
        public SettingsViewModel()
        {
            NewExtensionName = ".";

            AddExtensionCommand = new CommandHandler(
                execute: AddExtension,
                canExecute: () => !string.IsNullOrWhiteSpace(NewExtensionName)
            );

            DeleteExtensionCommand = new CommandHandler(
                execute: DeleteExtension,
                canExecute: () => SelectedExtension != null
            );

            var existingExtensions = _jobManager.getListExtensionFilesCryptoSoft();
            foreach (var ext in existingExtensions)
            {
                Extensions.Add(new ExtensionItem { Name = ext });
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
        public string NewExtensionName
        {
            get => _newExtensionName;
            set
            {
                if (_newExtensionName != value)
                {
                    _newExtensionName = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested(); // met à jour l'état du bouton
                }
            }
        }

        public ExtensionItem SelectedExtension
        {
            get => _selectedExtension;
            set
            {
                if (_selectedExtension != value)
                {
                    _selectedExtension = value;
                    OnPropertyChanged();
                    DeleteExtensionCommand.RaiseCanExecuteChanged(); // ← nécessaire ici
                }
            }
        }

        public ICommand AddExtensionCommand { get; }
        private void AddExtension()
        {
            Extensions.Add(new ExtensionItem { Name = NewExtensionName });
            string[] extensionArray = Extensions.Select(e => e.Name).ToArray();
            _jobManager.UpdateExtensionFileCryptoSoft(extensionArray);
            OutputText = $"Extension '{NewExtensionName}' ajoutée.";
            NewExtensionName = string.Empty;
        }

        public CommandHandler DeleteExtensionCommand { get; }
        private void DeleteExtension()
        {
            try
            {
                if (SelectedExtension == null)
                {
                    OutputText = "Aucune extension sélectionnée.";
                    return;
                }
                string outputMessage = SelectedExtension.Name;
                Extensions.Remove(SelectedExtension);
                string[] extensionArray = Extensions.Select(e => e.Name).ToArray();
                _jobManager.UpdateExtensionFileCryptoSoft(extensionArray);
                OutputText = $"Extension '{outputMessage}' supprimée.";
            }
            catch (Exception ex)
            {
                OutputText = $"Erreur lors de la suppression : {ex.Message}";
            }
        }
    }

    public class ExtensionItem
    {
        public string Name { get; set; }
    }
}