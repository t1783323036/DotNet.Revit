using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;

namespace DotNet.Revit.Hook
{
    /// <summary>
    /// 鼠标双击元素拦截事件.
    /// </summary>
    /// <seealso cref="Autodesk.Revit.UI.IExternalCommand" />
    [Transaction(TransactionMode.Manual)]
    public class MouseHookTest : IExternalCommand
    {
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (ElementMonitor.Instance == null)
            {
                ElementMonitor.Register(commandData.Application);
            }

            ElementMonitor.Instance.DoubleClickElement += OnRaiseDoubleClickElement;

            return Result.Succeeded;
        }

        private int OnRaiseDoubleClickElement(object sender, DoubleClickElementEventArgs e)
        {
            if (e.Element == null)
            {
                return 0;
            }

            System.Windows.Forms.MessageBox.Show(string.Format("双击击元素Id： {0}", e.Element.Id));

            return 1;

        }
    }
}
