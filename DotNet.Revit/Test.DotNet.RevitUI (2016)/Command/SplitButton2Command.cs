using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DotNet.RevitUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DotNet.RevitUI.Command
{

    public class SplitButton2Command : RevitCommand
    {
        protected override bool CanExecute(Autodesk.Windows.RibbonItem parameter)
        {
            return true;
        }

        protected override Result Invoke(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.UI.TaskDialog.Show("提示", "测试 Split Button2 Command 有效点击 ！");

            return Result.Succeeded;
        }
    }
}
