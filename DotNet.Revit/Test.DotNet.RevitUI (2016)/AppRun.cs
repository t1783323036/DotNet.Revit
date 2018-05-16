using Autodesk.Revit.UI;
using DotNet.RevitUI;
using DotNet.RevitUI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.DotNet.RevitUI.View;

namespace Test.DotNet.RevitUI
{
    /// <summary>
    /// This is app run class .
    /// </summary>
    public class AppRun : RevitApp
    {
        protected override Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        protected override Result OnStartup(UIControlledApplication application)
        {
            RibbonRegisterHelper.Register<RibbonView>();

            return Result.Succeeded;
        }
    }
}
