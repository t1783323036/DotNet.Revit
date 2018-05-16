using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;

namespace DotNet.RevitUI.MVVM
{
    /// <summary>
    /// 委托式revit命令
    /// </summary>
    public class RevitRelayCommand : RevitCommand
    {
        private readonly Func<ExternalCommandData, Result> m_Execute;

        private readonly Func<bool> m_CanExecute;

        public RevitRelayCommand(Func<ExternalCommandData, Result> execute, Func<bool> canExecute)
        {
            m_Execute = execute ?? throw new ArgumentNullException("execute");

            m_CanExecute = canExecute;
        }

        public RevitRelayCommand(Func<ExternalCommandData, Result> execute)
            : this(execute, null)
        {

        }

        protected override bool CanExecute(Autodesk.Windows.RibbonItem parameter)
        {
            return m_CanExecute == null || m_CanExecute();
        }

        protected override Result Invoke(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return m_Execute(commandData);
        }
    }
}
