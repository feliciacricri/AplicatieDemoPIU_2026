using System;
using System.Windows.Input;

namespace NivelUIWPF
{
    public class RelayCommand : ICommand
    {
        private readonly Action executa;
        private readonly Func<bool> poateExecuta;

        public RelayCommand(Action executa, Func<bool> poateExecuta = null)
        {
            this.executa = executa ?? throw new ArgumentNullException(nameof(executa));
            this.poateExecuta = poateExecuta;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return poateExecuta == null || poateExecuta();
        }

        public void Execute(object parameter)
        {
            executa();
        }
    }
}
