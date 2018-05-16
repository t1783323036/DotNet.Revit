using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autodesk.Revit.DB;
using DotNet.RevitUI.Helper;

namespace DotNet.RevitUI
{
    /// <summary>
    /// Revit command interface.
    /// </summary>
    public abstract class RevitCommand : ICommand, IExternalCommand
    {
        private static UIApplication m_Application;

        public static UIApplication Application
        {
            get
            {
                return m_Application;
            }
        }

        /// <summary>
        /// 当前激活文档.
        /// </summary>
        public Document ActiveDocument { get; private set; }

        /// <summary>
        /// 当前激活UI文档.
        /// </summary>
        public UIDocument ActiveUIDocument { get; private set; }

        /// <summary>
        /// Occurs when [can execute changed].
        /// </summary>
        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Riibon控件是否可执行.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>

        bool ICommand.CanExecute(object parameter)
        {
            var p = parameter == null ? null : parameter as Autodesk.Windows.RibbonItem;

            if (p == null)
                return CanExecute(p);

            p.Tag = RevitApp.Application;

            var flag = this.CanExecute(p);

            if (flag)
            {
                p.IsEnabled = true;
            }
            else
            {
                p.IsEnabled = false;
            }

            if (RevitApp.Application == null || p is Autodesk.Windows.RibbonItem item)
            {
                return flag;
            }

            return flag;
        }

        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Initialize(commandData);

            m_Application = commandData.Application;

            return this.Invoke(commandData, ref message, elements);
        }

        void ICommand.Execute(object parameter)
        {
            RevitCommandInvoke.ExecuteHandlers.Enqueue(((IExternalCommand)this).Execute);
            RevitCommandHelper.Instance.Execute<RevitCommandInvoke>();
        }

        void Initialize(ExternalCommandData commandData)
        {
            if (RevitCommandHelper.Instance == null)
            {
                RevitCommandHelper.Instance = new RevitCommandHelper();
            }

            RevitCommandHelper.Instance.RegisterCommand<RevitCommandInvoke>();

            if (commandData != null && commandData.Application != null)
            {
                this.ActiveUIDocument = commandData.Application.ActiveUIDocument;

                if (this.ActiveUIDocument != null)
                {
                    this.ActiveDocument = this.ActiveUIDocument.Document;
                }
            }
        }

        /// <summary>
        /// 此命令是否能执行.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected abstract bool CanExecute(Autodesk.Windows.RibbonItem parameter);

        /// <summary>
        /// 在revit上下文件环境执行命令.
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        protected abstract Result Invoke(ExternalCommandData commandData, ref string message, ElementSet elements);
    }
}
