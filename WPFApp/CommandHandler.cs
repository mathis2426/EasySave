using System;
using System.Windows;
using System.Windows.Input;


public class CommandHandler : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public CommandHandler(Action execute, Func<bool> canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute ?? (() => true);
    }

    public bool CanExecute(object parameter) => _canExecute();

    public void Execute(object parameter) => _execute();

    public void RaiseCanExecuteChanged()
    {
        // Appelle via le Dispatcher si on n’est pas dans le thread UI
        if (System.Windows.Application.Current?.Dispatcher?.CheckAccess() == false)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
                CommandManager.InvalidateRequerySuggested());
        }
        else
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
