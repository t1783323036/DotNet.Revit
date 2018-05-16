using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace DotNet.RevitUI.MVVM.Extension
{
    public static class WindowExtension
    {
        /// <summary>
        /// 非模态显示窗体，并指定此窗体的宿主窗体.
        /// </summary>
        /// <param name="main">The main.</param>
        /// <param name="parent">The parent.</param>
        public static void Show(this Window main, IntPtr parent)
        {
            if (parent == IntPtr.Zero)
            {
                parent = Process.GetCurrentProcess().MainWindowHandle;
            }

            var mainHelper = new System.Windows.Interop.WindowInteropHelper(main)
            {
                Owner = parent
            };

            // self-adaption
            if (Math.Abs(PrimaryScreen.ScaleX - 1.0) >= 1e-7)
            {
                main.Width = main.Width * PrimaryScreen.ScaleX;
                main.Height = main.Height * PrimaryScreen.ScaleX;
            }

            main.Show();
        }

        /// <summary>
        /// 模态显示窗体，并指定此窗体的宿主窗体.
        /// </summary>
        public static void ShowDialog(this Window main, IntPtr parent)
        {
            if (parent == IntPtr.Zero)
            {
                parent = Process.GetCurrentProcess().MainWindowHandle;
            }

            var mainHelper = new System.Windows.Interop.WindowInteropHelper(main)
            {
                Owner = parent
            };

            // self-adaption
            if (Math.Abs(PrimaryScreen.ScaleX - 1.0) >= 1e-7)
            {
                main.Width = main.Width * PrimaryScreen.ScaleX;
                main.Height = main.Height * PrimaryScreen.ScaleX;
            }

            main.ShowDialog();
        }

        /// <summary>
        /// 获取WPF窗口句柄.
        /// </summary>
        /// <param name="main">The main.</param>
        /// <returns></returns>
        public static IntPtr GetHandle(this Window main)
        {
            var helper = new System.Windows.Interop.WindowInteropHelper(main);

#if !NET35
            if(helper.Handle==IntPtr.Zero)
            {
               return helper.EnsureHandle();
            }
#endif
            return helper.Handle;
        }
    }

     class PrimaryScreen
    {
        #region Win32 API  
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        #endregion

        #region DeviceCaps常量  
        const int HORZRES = 8;
        const int VERTRES = 10;
        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;
        #endregion

        #region 属性  
        /// <summary>  
        /// 获取屏幕分辨率当前物理大小  
        /// </summary>  
        public static Size WorkingArea
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                var size = new Size
                {
                    Width = GetDeviceCaps(hdc, HORZRES),
                    Height = GetDeviceCaps(hdc, VERTRES)
                };

                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }

        /// <summary>  
        /// 当前系统DPI_X 大小 一般为96  
        /// </summary>  
        public static int DpiX
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                var DpiX = GetDeviceCaps(hdc, LOGPIXELSX);

                ReleaseDC(IntPtr.Zero, hdc);
                return DpiX;
            }
        }

        /// <summary>  
        /// 当前系统DPI_Y 大小 一般为96  
        /// </summary>  
        public static int DpiY
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                var DpiX = GetDeviceCaps(hdc, LOGPIXELSY);

                ReleaseDC(IntPtr.Zero, hdc);
                return DpiX;
            }
        }

        /// <summary>  
        /// 获取真实设置的桌面分辨率大小  
        /// </summary>  
        public static Size DESKTOP
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                var size = new Size();
                size.Width = GetDeviceCaps(hdc, DESKTOPHORZRES);
                size.Height = GetDeviceCaps(hdc, DESKTOPVERTRES);

                ReleaseDC(IntPtr.Zero, hdc);
                return size;
            }
        }

        /// <summary>  
        /// 获取宽度缩放百分比  
        /// </summary>  
        public static float ScaleX
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                var t = GetDeviceCaps(hdc, DESKTOPHORZRES);
                var d = GetDeviceCaps(hdc, HORZRES);
                var ScaleX = (float)GetDeviceCaps(hdc, DESKTOPHORZRES) / (float)GetDeviceCaps(hdc, HORZRES);

                ReleaseDC(IntPtr.Zero, hdc);
                return ScaleX;
            }
        }

        /// <summary>  
        /// 获取高度缩放百分比  
        /// </summary>  
        public static float ScaleY
        {
            get
            {
                var hdc = GetDC(IntPtr.Zero);
                var scaleY = (float)GetDeviceCaps(hdc, DESKTOPVERTRES) / (float)GetDeviceCaps(hdc, VERTRES);

                ReleaseDC(IntPtr.Zero, hdc);
                return scaleY;
            }
        }
        #endregion
    }
}
