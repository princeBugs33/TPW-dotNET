using System.Windows.Input;

namespace ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        public event EventHandler? CanExecuteChanged;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }
        
        public bool CanExecute(object? parameter)
        {
            return true;
        }
        
        public void Execute(object? parameter)
        {
            _execute();
        }
    }
}