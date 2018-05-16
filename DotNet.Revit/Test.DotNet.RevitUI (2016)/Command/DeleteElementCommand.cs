using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using DotNet.RevitUI;
using Autodesk.Revit.Attributes;

namespace Test.DotNet.RevitUI.Command
{
    /// <summary>
    /// 删除元素
    /// </summary>
    /// <remarks>测试基于ribbon自动化绑定命令下的事务开启测试</remarks>
    /// <remarks>如果基于addinmanager工具测试此命令，则需要声明特性，否则不需要声明此特性</remarks>

    [Transaction(TransactionMode.Manual)]
    public class DeleteElementCommand : RevitCommand
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
            var uidoc = commandData.Application.ActiveUIDocument;

            var doc = uidoc.Document;

            while (true)
            {
                try
                {
                    var reference = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "请选择要删除的元素 :");

                    doc.Invoke(m =>
                    {
                        try
                        {
                            doc.Delete(reference.ElementId);
                        }
                        catch (Exception ex)
                        {
                            Autodesk.Revit.UI.TaskDialog.Show("错误", ex.Message);
                        }
                    });
                }
                catch (Exception ex)
                {
                    return Result.Cancelled;
                }
            }
        }
    }
}
