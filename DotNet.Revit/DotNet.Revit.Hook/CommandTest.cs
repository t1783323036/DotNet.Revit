using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;

namespace DotNet.Revit.Hook
{
    [Transaction(TransactionMode.Manual)]
    public class MouseHookTest : IExternalCommand
    {
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {



            return Result.Succeeded;
        }
    }
}
