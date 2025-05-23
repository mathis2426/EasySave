using ControllerModel.Jobs;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WPFApp
{
    public class SettingsViewModel : AbstractViewModel
    {
        private readonly JobManager _jobManager = new JobManager();
        private ExtensionItem _selectedExtension;
        private string _newExtensionName;
        private string _outputText;
        
        public ObservableCollection<ExtensionItem> Extensions { get; } = new ObservableCollection<ExtensionItem>();
        
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
                    CommandManager.InvalidateRequerySuggested(); // met à jour l'état du bouton "Supprimer"
                }
            }
        }

        public SettingsViewModel()
        {
            AddExtensionCommand = new CommandHandler(
                execute: AddExtension,
                canExecute: () => !string.IsNullOrWhiteSpace(NewExtensionName)
            );

            DeleteExtensionCommand = new CommandHandler(
                execute: DeleteExtension,
                canExecute: () => SelectedExtension != null
            );
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

        public ICommand DeleteExtensionCommand { get; }
        private void DeleteExtension()
        {
            if (SelectedExtension == null)
                return;

            Extensions.Remove(SelectedExtension);
            string[] extensionArray = Extensions.Select(e => e.Name).ToArray();
            _jobManager.UpdateExtensionFileCryptoSoft(extensionArray);
            OutputText = $"Extension '{SelectedExtension.Name}' supprimée.";
        }

    }

    public class ExtensionItem
    {
        public string Name { get; set; }
    }
}
