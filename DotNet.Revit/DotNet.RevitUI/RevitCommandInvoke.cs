using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.Attributes;
using System.Threading.Tasks;

namespace DotNet.RevitUI.Helper
{
    /// <summary>
    /// Revit命令帮助.
    /// </summary>

    [Transaction(TransactionMode.Manual)]
    class RevitCommandInvoke : IExternalCommand
    {
        public static Queue<ExecuteHandler> ExecuteHandlers { get; set; }

        public static Queue<InvokeHandler> InvokeHandlers { get; set; }

        static RevitCommandInvoke()
        {
            ExecuteHandlers = new Queue<ExecuteHandler>();

            InvokeHandlers = new Queue<InvokeHandler>();
        }

        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            while (ExecuteHandlers.Count > 0)
            {
                var first = ExecuteHandlers.Dequeue();

                first(commandData, ref message, elements);
            }

            while (InvokeHandlers.Count > 0)
            {
                var first = InvokeHandlers.Dequeue();

                first(commandData.Application);
            }

            return Result.Succeeded;
        }
    }

    delegate Result ExecuteHandler(ExternalCommandData commandData, ref string message, ElementSet elements);

    delegate void InvokeHandler(UIApplication uiApp);
}
