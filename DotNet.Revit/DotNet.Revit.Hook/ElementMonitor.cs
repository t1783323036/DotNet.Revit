using Autodesk.Revit.UI;
using DotNet.Revit.Hook.Achieve;
using DotNet.Revit.Hook.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNet.Revit.Hook
{
    /// <summary>
    /// 元素监控.
    /// </summary>
    public class ElementMonitor
    {
        private static ElementMonitor m_Instance;
        private MouseHook m_MouseHook;
        private bool m_IsMonitor;
        private UIApplication m_UIApplication;

        private ElementMonitor(UIApplication uiApp)
        {
            m_Instance = this;
            m_UIApplication = uiApp;

            m_MouseHook = new MouseHook();
            m_MouseHook.Install();

            m_MouseHook.MouseDoubleClick += OnRaiseMouseDoubleClick;
        }

        /// <summary>
        /// 静态实例，可在入口类判断此实例是否为null，防止重复注册.
        /// </summary>
        public static ElementMonitor Instance
        {
            get
            {
                return m_Instance;
            }
        }

        /// <summary>
        /// 当鼠标双击元素时触发此事件.
        /// </summary>
        public event HookHandler<DoubleClickElementEventArgs> DoubleClickElement;

        /// <summary>
        /// 注册元素监控，并指定是否立即监控.
        /// </summary>
        public static void Register(UIApplication uiApp, bool immediatelyMonitor = true)
        {
            if (uiApp == null)
            {
                throw new ArgumentNullException(nameof(uiApp));
            }

            new ElementMonitor(uiApp)
            {
                m_IsMonitor = immediatelyMonitor
            };
        }

        /// <summary>
        /// 注册元素监控，并指定是否立即监控.
        /// </summary>
        public static void Register(UIControlledApplication uiControllApp, bool immediatelyMonitor = true)
        {
            if (uiControllApp == null)
            {
                throw new ArgumentNullException(nameof(uiControllApp));
            }

            var flag = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod;

            var uiApp = (UIApplication)uiControllApp.GetType().InvokeMember("getUIApplication", flag, Type.DefaultBinder, uiControllApp, null);

            Register(uiApp, immediatelyMonitor);
        }

        /// <summary>
        /// 返回1，则拦截鼠标消息，返回0则传递给真正消息接收者.
        /// </summary>
        private int OnRaiseMouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!m_IsMonitor || e.Button != MouseButtons.Left || e.Clicks != 2)
            {
                return 0;
            }

            var uiDoc = m_UIApplication.ActiveUIDocument;

            if (uiDoc == null)
            {
                return 0;
            }

            var elemIds = uiDoc.Selection.GetElementIds();

            if (elemIds.Count == 1)
            {
                var elem = uiDoc.Document.GetElement(elemIds.First());

                if (elem == null)
                {
                    return 0;
                }

                if (this.DoubleClickElement == null)
                {
                    return 0;
                }

                return this.DoubleClickElement(this, new DoubleClickElementEventArgs(elem));
            }

            return 0;
        }
    }

}
