using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.RevitUI.MVVM
{
    /// <summary>
    /// 不带参数通用命令类.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />

    public class RelayCommand : System.Windows.Input.ICommand
    {
        private readonly Action m_Execute;

        private readonly Func<bool> m_CanExecute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (m_CanExecute != null)
                    System.Windows.Input.CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (m_CanExecute != null)
                    System.Windows.Input.CommandManager.RequerySuggested -= value;
            }
        }

        public RelayCommand(Action execute)
            : this(execute, null)
        {

        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            m_Execute = execute ?? throw new ArgumentNullException("execute");

            m_CanExecute = canExecute;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return m_CanExecute == null || m_CanExecute();
        }

        public void Execute(object parameter)
        {
            m_Execute();
        }
    }
}
