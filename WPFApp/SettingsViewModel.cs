using ControllerModel.Jobs;
using ControllerModel.LanguagesHelper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;

namespace WPFApp
{
    public class SettingsViewModel : AbstractViewModel
    {
        private readonly JobManager _jobManager = new();
        private ExtensionItem _selectedExtension;
        private readonly LanguageManager languageManager = new();
        private ExtensionItem _selectedPriorityExtension;

        private string _newExtensionName;
        private string _newPriorityExtension;

        private string _outputText;

        public ObservableCollection<ExtensionItem> Extensions { get; } = new ObservableCollection<ExtensionItem>();

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

            //BlockingApp = _jobManager.GetBlockingApp();
            _selectedLanguage = languageManager.saveConfigObj.Language;

            //SizeFile = _jobManager.GetLargeFileThreshold();

            //SizeMax =

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

        public string ExtensionsName => languageManager.Get("ExtensionsName");
        public string BlockingApplication => languageManager.Get("BlockingApplication");
        public string Exit => languageManager.Get("Exit");
        public string Language => languageManager.Get("Language");
        public string AddExtensionName => languageManager.Get("AddExtension");
        public string DeleteExtensionName => languageManager.Get("DeleteExtension");
        public string PriorityExtension => languageManager.Get("PriorityExtension");
        public string AddPriority => languageManager.Get("AddPriority");
        public string DeletePriority => languageManager.Get("DeletePriority");
        public string MaxSizeFile => languageManager.Get("MaxSizeFile");


        private void RefreshTranslations()
        {
            OnPropertyChanged(nameof(ExtensionsName));
            OnPropertyChanged(nameof(BlockingApplication));
            OnPropertyChanged(nameof(Exit));
            OnPropertyChanged(nameof(Language));
            OnPropertyChanged(nameof(AddExtensionName));
            OnPropertyChanged(nameof(DeleteExtensionName));
            OnPropertyChanged(nameof(PriorityExtension));
            OnPropertyChanged(nameof(AddPriority));
            OnPropertyChanged(nameof(DeletePriority));
            OnPropertyChanged(nameof(MaxSizeFile));

        }
        public ObservableCollection<ExtensionItem> PriorityExtensions { get; } = new ObservableCollection<ExtensionItem>();

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
        private string _blockingApp;
        public string BlockingApp
        {
            get => _blockingApp;
            set
            {
                if (_blockingApp != value)
                {
                    _blockingApp = value;
                    OnPropertyChanged();
                }

                _jobManager.SetBlockingApp(_blockingApp);
                OnPropertyChanged();
            }
        }

        private int _sizeFile;
        public int SizeFile
        {
            get => _sizeFile;
            set
            {
                if (_sizeFile != value)
                {
                    _sizeFile = value;
                    OnPropertyChanged();
                }

                _jobManager.SetLargeFileThreshold(_sizeFile);
                OnPropertyChanged();
            }
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    
                    languageManager.SetLanguage(_selectedLanguage);
                    RefreshTranslations();
                    OnPropertyChanged();
                    OutputText = $"{ languageManager.Get("language_changed")} : {_selectedLanguage}";

                }
            }
        }

        public ObservableCollection<string> AvailableLanguages { get; } = new ObservableCollection<string>
        {
            "fr",
            "en-US"
        };
    }

    public class ExtensionItem
    {
        public string Name { get; set; }
    }
}
