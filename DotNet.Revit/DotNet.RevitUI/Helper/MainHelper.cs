using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using UIFrameworkServices;

namespace DotNet.RevitUI.Helper
{
    /// <summary>
    /// Revit UI helper.
    /// </summary>

    public static class MainHelper
    {
        const int MDI_ID = 0x0000E900;

        const int StatusBar_ID = 0x0000E801;

        const int OptionsBar_ID = 0x000EC801;

        [DllImport("user32")]
        static extern IntPtr GetDlgItem(IntPtr hWndParent, int cid);

        /// <summary>
        /// Revit 主窗口句柄.
        /// </summary>
        public static IntPtr RevitHandle = ComponentManager.ApplicationWindow;

        /// <summary>
        /// Revit MDI句柄.
        /// </summary>
        public static IntPtr RevitMDIHandle = GetDlgItem(ComponentManager.ApplicationWindow, MDI_ID);

        /// <summary>
        /// Revit StatusBar句柄.
        /// </summary>
        public static IntPtr RevitStatusBarHandle = GetDlgItem(ComponentManager.ApplicationWindow, StatusBar_ID);

        /// <summary>
        /// Revit OptionsBar句柄
        /// </summary>
        public static IntPtr RevitOptionsBarHandle = GetDlgItem(ComponentManager.ApplicationWindow, OptionsBar_ID);

        /// <summary>
        /// Revit MDI rect.
        /// </summary>
        public static Rect RevitMDIRect = FloatingService.getMDIClientRect();
    }
}