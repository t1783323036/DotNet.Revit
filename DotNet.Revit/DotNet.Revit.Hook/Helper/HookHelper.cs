using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNet.Revit.Hook.Helper
{
    static class HookHelper
    {
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetCurrentThreadId();

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CallNextHookEx(int idHook, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetCurrentProcessId();

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        public static int HiWord(IntPtr dword)
        {
            return dword.ToInt32() >> 16 & 65535;
        }

        public static int LoWord(IntPtr dword)
        {
            return dword.ToInt32() & 65535;
        }

        public static T ToStruct<T>(this IntPtr lParam) where T : struct
        {
            return (T)Marshal.PtrToStructure(lParam, typeof(T));
        }
    }


    public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    public delegate int HookHandler<TEvent>(object sender, TEvent e) where TEvent : EventArgs;
}
