using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNet.Revit.Hook.DataStructure
{
    /// <summary>
    /// 全局鼠标消息结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEHOOKSTRUCT
    {
        /// <summary>
        /// 屏幕坐标
        /// </summary>
        public POINT pt;

        /// <summary>
        /// 如果消息是WM_MOUSEWHEEL，则此成员的高位字是wheel delta。保留低位字。正值表示车轮向前旋转，远离用户; 负值表示车轮向后旋转，朝向用户。 
        /// XBUTTON1 == 0x0001 如果按下或释放第一个X按钮。
        /// XBUTTON2 == 0x0002 如果按下或释放第一个X按钮。
        /// </summary>
        public int mouseData;

        public int flags;

        public uint wHitTestCode;

        public uint dwExtraInfo;
    }
}
