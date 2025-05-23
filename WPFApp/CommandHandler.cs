using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}