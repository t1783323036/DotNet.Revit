using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using DotNet.RevitUI;
using DotNet.RevitUI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.DotNet.RevitUI.View;
using DotNet.RevitUI.MVVM.Extension;

namespace Test.DotNet.RevitUI.Command
{
    /// <summary>
    /// 分段墙体
    /// </summary>
    /// <remarks>1 . 测试基于WPF MVVM 的事务开启</remarks>
    /// <remarks>2 . 测试模态窗体立即调用命令</remarks>

    public class DeleteMatchElementCommand : RevitCommand
    {
        protected override bool CanExecute(Autodesk.Windows.RibbonItem parameter)
        {
            if (parameter == null)
            {
                return true;
            }

            if (parameter.Tag is UIApplication uiApp)
            {
                return uiApp.ActiveUIDocument != null;
            }

            return true;
        }

        protected override Result Invoke(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var rect = MainHelper.RevitMDIRect;

            var main = new MainDeleteMatchElement()
            {
                Top = rect.Top + 5,
                Left = rect.Left
            };

            main.ShowDialog(MainHelper.RevitHandle);

            return Result.Succeeded;
        }
    }
}
