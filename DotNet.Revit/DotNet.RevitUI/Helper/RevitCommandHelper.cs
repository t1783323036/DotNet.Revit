using Autodesk.Revit.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFrameworkServices;
using System.Reflection;
using Autodesk.Revit.DB;
using System.Windows.Threading;

namespace DotNet.RevitUI.Helper
{
    /// <summary>
    /// Revit command helper
    /// </summary>
    public class RevitCommandHelper
    {
        private static RevitCommandHelper m_Instance;

        private ConcurrentDictionary<string, string> m_CommandIds;

        public static RevitCommandHelper Instance
        {
            get
            {
                return m_Instance;
            }

            internal set
            {
                m_Instance = value;
            }
        }

        internal RevitCommandHelper()
        {
            m_Instance = this;

            m_CommandIds = new ConcurrentDictionary<string, string>();
        }

        /// <summary>
        /// 指定一个基于IExternalCommand全名称.执行此命令.
        /// </summary>
        public bool Execute(string className)
        {
            if (!m_CommandIds.ContainsKey(className))
            {
                return false;
            }

            var commnadId = m_CommandIds[className];

            string text2 = string.Format("CustomCtrl_%{0}%{1}", "ZhongHao.He", commnadId);

            ExternalCommandHelper.executeExternalCommand(text2);

            return true;
        }

        /// <summary>
        /// 指定一个Revit命令接口，执行此命令.
        /// </summary>
        /// <remarks>在执行此命令之前，应确保命令已被加入.</remarks>
        public bool Execute<T>()
            where T : IExternalCommand
        {
            return this.Execute(typeof(T).FullName);
        }

        /// <summary>
        /// 指定Revit命令Id，调用内部命令.
        /// </summary>
        public bool Invoke(string cmdId)
        {
            if (ExternalCommandHelper.CanExecute(cmdId))
            {
                ExternalCommandHelper.executeExternalCommand(cmdId);
                return true;
            }
            else if (CommandHandlerService.canExecute(cmdId))
            {
                CommandHandlerService.invokeCommandHandler(cmdId);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 切回Revit上下文件环境调用命令.
        /// </summary>
        /// <param name="uiApp"></param>
        public void Invoke(Action<UIApplication> uiApp)
        {
            if (uiApp == null)
            {
                throw new NullReferenceException();
            }

            if (Dispatcher.CurrentDispatcher != Autodesk.Windows.ComponentManager.Ribbon.Dispatcher)
            {
                Autodesk.Windows.ComponentManager.Ribbon.Dispatcher.Invoke(new Action(() =>
                {
                    RevitCommandInvoke.InvokeHandlers.Enqueue(new InvokeHandler(uiApp));
                    this.Execute<RevitCommandInvoke>();

                }));
            }
            else
            {
                RevitCommandInvoke.InvokeHandlers.Enqueue(new InvokeHandler(uiApp));
                this.Execute<RevitCommandInvoke>();
            }
        }

        /// <summary>
        /// 注册一个命令.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public bool RegisterCommand(string assemblyPath, string className)
        {
            if (m_CommandIds.ContainsKey(className))
            {
                return false;
            }

            var guid = Guid.NewGuid().ToString();
            var data = new PushButtonData(guid, guid, assemblyPath, className);
            var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.InvokeMethod;

            var btn = typeof(PushButtonData).InvokeMember("createPushButton",
                flags,
                Type.DefaultBinder,
                null,
                new object[] { data, false, "ZhongHao.He" });

            btn.GetType().InvokeMember("getRibbonButton", flags, Type.DefaultBinder, btn, null);

            return m_CommandIds.TryAdd(className, guid);
        }

        /// <summary>
        /// 注册一个命令.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool RegisterCommand<T>()
            where T : IExternalCommand
        {
            return RegisterCommand(typeof(T).Assembly.Location, typeof(T).FullName);
        }
    }
}
