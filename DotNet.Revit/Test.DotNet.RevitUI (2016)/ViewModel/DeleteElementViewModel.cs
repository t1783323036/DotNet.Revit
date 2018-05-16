using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using DotNet.RevitUI;
using DotNet.RevitUI.MVVM;
using Test.DotNet.RevitUI.View;

namespace Test.DotNet.RevitUI.ViewModel
{
    public class DeleteElementViewModel : ObservableObject
    {
        private int _elementId;

        public int ElementId
        {
            get
            {
                return _elementId;
            }

            set
            {
                _elementId = value;

                this.RaisePropertyChanged(nameof(ElementId));
            }
        }

        /// <summary>
        /// Revit动态委托命令
        /// </summary>
        /// <remarks>也可以直接绑定RevitCommand</remarks>
        public RevitRelayCommand OK { get; private set; }

        public RelayCommand Cancel { get; private set; }

        public DeleteElementViewModel()
        {
            this.ElementId = 0;

            this.OK = new RevitRelayCommand(OnOK, () => ElementId > 0);

            this.Cancel = new RelayCommand(OnCancel);
        }

        private void OnCancel()
        {
            Messenger.Default.Send(this, MainDeleteMatchElement.ClosedToken);
        }

        private Result OnOK(ExternalCommandData arg)
        {
            var uidoc = arg.Application.ActiveUIDocument;

            var doc = uidoc.Document;

            if (doc.GetElement(new Autodesk.Revit.DB.ElementId(ElementId)) is Autodesk.Revit.DB.Element elem)
            {
                doc.Invoke(m => 
                {
                    try
                    {
                        doc.Delete(elem.Id);
                    }
                    catch (Exception ex)
                    {
                        Autodesk.Revit.UI.TaskDialog.Show("错误", ex.Message);
                    }
                });

                Autodesk.Revit.UI.TaskDialog.Show("提示", "删除元素完成");

                return Result.Succeeded;
            }

            Autodesk.Revit.UI.TaskDialog.Show("警告", "输入元素ID不合法，或者未在当前文档找到此元素ID");

            return Result.Succeeded;
        }
    }
}
