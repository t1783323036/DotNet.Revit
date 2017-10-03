using DotNet.Revit.Hook.DataStructure;
using DotNet.Revit.Hook.Helper;
using DotNet.Revit.Hook.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Revit.Hook.Achieve
{
    public abstract class HookBase : IHook
    {
        private static Dictionary<int, IHook> m_Hooks;
        private IntPtr m_ProcessId;
        private int m_ThreadId;
        private HookType m_HookType;
        private HookProc m_HookProc;

        protected internal int m_HookId;

        static HookBase()
        {
            m_Hooks = new Dictionary<int, IHook>();
        }

        private HookBase(HookType hookType)
        {
            m_HookType = hookType;
            m_HookProc = HookProc;
        }

        protected HookBase(IntPtr processId, HookType hookType)
            : this(hookType)
        {
            m_ProcessId = processId;
            if (m_ProcessId == IntPtr.Zero)
            {
                m_ProcessId = HookHelper.GetCurrentProcessId();
            }
        }

        protected HookBase(int threadId, HookType hookType)
            : this(hookType)
        {
            m_ThreadId = threadId;
            if (m_ThreadId == 0)
            {
                m_ThreadId = HookHelper.GetCurrentThreadId();
            }
        }

        public void Install()
        {
            if (m_ThreadId != 0)
            {
                m_HookId = HookHelper.SetWindowsHookEx(m_HookType, m_HookProc, IntPtr.Zero, m_ThreadId);
            }
            else
            {
                if (m_ProcessId == IntPtr.Zero)
                {
                    return;
                }
                m_HookId = HookHelper.SetWindowsHookEx(m_HookType, m_HookProc, m_ProcessId, 0);
            }

            if (m_HookId == 0)
            {
                return;
            }

            if (!m_Hooks.ContainsKey(m_HookId))
            {
                m_Hooks.Add(m_HookId, this);
            }
        }

        public void Uninstall()
        {
            if (m_HookId == 0)
            {
                return;
            }

            var flag = HookHelper.UnhookWindowsHookEx(m_HookId);
            if (flag)
            {
                if (m_Hooks.Remove(m_HookId))
                {
                    m_HookId = 0;
                }
            }
        }

        protected abstract int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    }

}
