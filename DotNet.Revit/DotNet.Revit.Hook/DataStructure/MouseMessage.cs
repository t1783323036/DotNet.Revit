using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Revit.Hook.DataStructure
{
    [Flags]
    public enum MouseMessage
    {
        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        WM_LBUTTONDOWN = 0x0201,

        /// <summary>
        /// 鼠标左键双击
        /// </summary>
        WM_LBUTTONDBLCLK = 0x0203,

        /// <summary>
        /// 鼠标左键弹起
        /// </summary>
        WM_LBUTTONUP = 0x0202,

        /// <summary>
        /// 鼠标右键单击
        /// </summary>
        WM_RBUTTONDOWN = 0x0204,

        /// <summary>
        /// 鼠标右键双击
        /// </summary>
        WM_RBUTTONDBLCLK = 0x0206,

        /// <summary>
        /// 鼠标右键弹起
        /// </summary>
        WM_RBUTTONUP = 0x0205,

        /// <summary>
        /// 鼠标中键双击
        /// </summary>
        WM_MBUTTONDBLCLK = 0x0209,
        /// <summary>
        /// 鼠标中键单击
        /// </summary>
        WM_MBUTTONDOWN = 0x0207,
        /// <summary>
        /// 鼠标中键弹起
        /// </summary>
        WM_MBUTTONUP = 0x0208,

        /// <summary>
        /// 鼠标侧键双击时触发.
        /// </summary>
        WM_XBUTTONDBLCLK = 0x020D,

        /// <summary>
        /// 鼠标侧键单击时触发
        /// </summary>
        WM_XBUTTONDOWN = 0x020B,

        /// <summary>
        /// 鼠标侧键弹起时触发.
        /// </summary>
        WM_XBUTTONUP = 0x020C,

        /// <summary>
        /// 鼠标移动
        /// </summary>
        WM_MOUSEMOVE = 0x0200,

        /// <summary>
        /// 鼠标滚动
        /// </summary>
        WM_MOUSEWHEEL = 0x020A,

        /// <summary>
        /// 鼠标滚动
        /// </summary>
        WM_MOUSEHWHEEL = 0x020E,

        /// <summary>
        /// 正在失去鼠标捕获的窗口.
        /// </summary>
        WM_CAPTURECHANGED = 0x0215,

        /// <summary>
        /// 当鼠标在非激活窗体按下时.
        /// </summary>
        WM_MOUSEACTIVATE = 0x0021,

        /// <summary>
        /// 当光标悬停在窗口的客户端区域上达到在先前调用TrackMouseEvent中指定的时间段时，发布到窗口。
        /// </summary>
        WM_MOUSEHOVER = 0x02A1,
    }
}
