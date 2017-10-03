using DotNet.Revit.Hook.DataStructure;
using DotNet.Revit.Hook.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNet.Revit.Hook.Achieve
{
    public abstract class MouseHookBase : HookBase
    {
        protected MouseHookBase(IntPtr processId)
            : base(processId, HookType.WH_MOUSE_LL)
        {

        }

        protected MouseHookBase(int threadId)
            : base(threadId, HookType.WH_MOUSE)
        {

        }

        /// <summary>
        /// 鼠标双击
        /// </summary>
        public event HookHandler<MouseEventArgs> MouseDoubleClick;

        /// <summary>
        /// 鼠标移动
        /// </summary>
        public event HookHandler<MouseEventArgs> MouseMove;

        /// <summary>
        /// 鼠标按下
        /// </summary>
        public event HookHandler<MouseEventArgs> MouseDown;

        /// <summary>
        /// 鼠标弹起
        /// </summary>
        public event HookHandler<MouseEventArgs> MouseUp;

        protected override int HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return HookHelper.CallNextHookEx(m_HookId, nCode, wParam, lParam);
            }

            var mouseMsg = (MouseMessage)wParam.ToInt32();
            var mouseHookStruct = lParam.ToStruct<MOUSEHOOKSTRUCT>();

            var button = this.GetMouseButtons(mouseMsg);

            switch (mouseMsg)
            {
                case MouseMessage.WM_LBUTTONDOWN:
                case MouseMessage.WM_RBUTTONDOWN:
                case MouseMessage.WM_MBUTTONDOWN:

                    return this.OnRaiseMouseDown(button, 1, mouseHookStruct.pt.X, mouseHookStruct.pt.Y, mouseHookStruct.mouseData);

                case MouseMessage.WM_LBUTTONUP:
                case MouseMessage.WM_MBUTTONUP:
                case MouseMessage.WM_RBUTTONUP:

                    return this.OnRaiseMouseUp(button, 1, mouseHookStruct.pt.X, mouseHookStruct.pt.Y, mouseHookStruct.mouseData);

                case MouseMessage.WM_LBUTTONDBLCLK:
                case MouseMessage.WM_RBUTTONDBLCLK:
                case MouseMessage.WM_MBUTTONDBLCLK:

                    return this.OnRaiseMouseDoubleClick(button, 2, mouseHookStruct.pt.X, mouseHookStruct.pt.Y, mouseHookStruct.mouseData);

                case MouseMessage.WM_MOUSEMOVE:

                    return this.OnRaiseMouseMove(MouseButtons.None, 0, mouseHookStruct.pt.X, mouseHookStruct.pt.Y, mouseHookStruct.mouseData);
                default:
                    return HookHelper.CallNextHookEx(m_HookId, nCode, wParam, lParam);
            }
        }

        private MouseButtons GetMouseButtons(MouseMessage mouseMsg)
        {
            MouseButtons result = MouseButtons.None;
            switch (mouseMsg)
            {
                case MouseMessage.WM_LBUTTONDBLCLK:
                case MouseMessage.WM_LBUTTONDOWN:
                case MouseMessage.WM_LBUTTONUP:
                    result = MouseButtons.Left;
                    break;
                case MouseMessage.WM_MBUTTONDBLCLK:
                case MouseMessage.WM_MBUTTONDOWN:
                case MouseMessage.WM_MBUTTONUP:
                    result = MouseButtons.Middle;
                    break;
                case MouseMessage.WM_RBUTTONDBLCLK:
                case MouseMessage.WM_RBUTTONDOWN:
                case MouseMessage.WM_RBUTTONUP:
                    result = MouseButtons.Right;
                    break;
            }
            return result;
        }

        private int OnRaiseMouseDoubleClick(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (this.MouseDoubleClick != null)
            {
                return this.MouseDoubleClick(this, new MouseEventArgs(button, clicks, x, y, delta));
            }
            return 0;
        }

        private int OnRaiseMouseDown(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (this.MouseDown != null)
            {
                return this.MouseDown(this, new MouseEventArgs(button, clicks, x, y, delta));
            }
            return 0;
        }

        private int OnRaiseMouseUp(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (this.MouseUp != null)
            {
                return this.MouseUp(this, new MouseEventArgs(button, clicks, x, y, delta));
            }
            return 0;
        }

        private int OnRaiseMouseMove(MouseButtons button, int clicks, int x, int y, int delta)
        {
            if (this.MouseMove != null)
            {
                return this.MouseMove(this, new MouseEventArgs(button, clicks, x, y, delta));
            }
            return 0;
        }
    }
}
