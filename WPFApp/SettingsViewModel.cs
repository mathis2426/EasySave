using ControllerModel.Jobs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace WPFApp
{
    public class SettingsViewModel : AbstractViewModel
    {
        public readonly JobManager _jobManager = new JobManager();

        private ExtensionItem _selectedExtension;
        private ExtensionItem _selectedPriorityExtension;

        private string _newExtensionName;
        private string _newPriorityExtension;

        private string _outputText;

        public ObservableCollection<ExtensionItem> Extensions { get; } = new ObservableCollection<ExtensionItem>();
        public ObservableCollection<ExtensionItem> PriorityExtensions { get; } = new ObservableCollection<ExtensionItem>();

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

            AddPriorityExtensionCommand = new CommandHandler(
                execute: AddPriorityExtension,
                canExecute: () => !string.IsNullOrWhiteSpace(NewPriorityExtension)
            );

            DeletePriorityExtensionCommand = new CommandHandler(
                execute: DeletePriorityExtension,
                canExecute: () => SelectedPriorityExtension != null
            );

            var existingExtensions = _jobManager.getListExtensionFilesCryptoSoft();
            foreach (var ext in existingExtensions)
            {
                Extensions.Add(new ExtensionItem { Name = ext });
            }

            var existingPriorityExtensions = _jobManager.getListExtensionPriorityFiles();
            foreach (var ext in existingPriorityExtensions)
            {
                PriorityExtensions.Add(new ExtensionItem { Name = ext });
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
                    AddExtensionCommand?.RaiseCanExecuteChanged();
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
                    DeleteExtensionCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        public string NewPriorityExtension
        {
            get => _newPriorityExtension;
            set
            {
                if (_newPriorityExtension != value)
                {
                    _newPriorityExtension = value;
                    OnPropertyChanged();
                    AddPriorityExtensionCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        public ExtensionItem SelectedPriorityExtension
        {
            get => _selectedPriorityExtension;
            set
            {
                if (_selectedPriorityExtension != value)
                {
                    _selectedPriorityExtension = value;
                    OnPropertyChanged();
                    DeletePriorityExtensionCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        public CommandHandler AddExtensionCommand { get; }
        private void AddExtension()
        {
            try
            {
                Extensions.Add(new ExtensionItem { Name = NewExtensionName });
                string[] extensionArray = Extensions.Select(e => e.Name).ToArray();
                _jobManager.UpdateExtensionFileCryptoSoft(extensionArray);
                OutputText = $"Extension '{NewExtensionName}' ajoutée.";
                NewExtensionName = string.Empty;
            }
            catch (Exception ex)
            {
                OutputText = $"Erreur lors de l'ajout de l'extension : {ex.Message}";
            }
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

        public CommandHandler AddPriorityExtensionCommand { get; }
        private void AddPriorityExtension()
        {
            try
            {
                PriorityExtensions.Add(new ExtensionItem { Name = NewPriorityExtension });
                string[] priorityArray = PriorityExtensions.Select(e => e.Name).ToArray();
                _jobManager.UpdateExtensionPriorityFile(priorityArray);
                OutputText = $"Extension prioritaire '{NewPriorityExtension}' ajoutée.";
                NewPriorityExtension = string.Empty;
            }
            catch (Exception ex)
            {
                OutputText = $"Erreur lors de l'ajout de l'extension prioritaire : {ex.Message}";
            }
        }

        public CommandHandler DeletePriorityExtensionCommand { get; }
        private void DeletePriorityExtension()
        {
            try
            {
                if (SelectedPriorityExtension == null)
                {
                    OutputText = "Aucune extension prioritaire sélectionnée.";
                    return;
                }

                string outputMessage = SelectedPriorityExtension.Name;
                PriorityExtensions.Remove(SelectedPriorityExtension);
                string[] priorityArray = PriorityExtensions.Select(e => e.Name).ToArray();
                _jobManager.UpdateExtensionPriorityFile(priorityArray);
                OutputText = $"Extension prioritaire '{outputMessage}' supprimée.";
            }
            catch (Exception ex)
            {
                OutputText = $"Erreur lors de la suppression prioritaire : {ex.Message}";
            }
        }
    }

    public class ExtensionItem
    {
        public string Name { get; set; }
    }
}
