using Autodesk.Revit.UI;
using Autodesk.Windows;
using DotNet.RevitUI.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.RevitUI
{
    /// <summary>
    /// Revit入口App.
    /// </summary>
    public abstract class RevitApp : IExternalApplication
    {
        private static string WorkPath = Path.GetDirectoryName(typeof(RevitApp).Assembly.Location);

        private static UIApplication m_Application;

        public static UIApplication Application
        {
            get
            {
                return m_Application;
            }
        }

        /// <summary>
        /// Revit关闭后执行.
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        protected abstract Result OnShutdown(UIControlledApplication application);

        /// <summary>
        /// Revit启动时执行.
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        protected abstract Result OnStartup(UIControlledApplication application);

        Result IExternalApplication.OnShutdown(UIControlledApplication application)
        {
            return this.OnShutdown(application);
        }

        Result IExternalApplication.OnStartup(UIControlledApplication application)
        {
            this.OnBefore(application);

            var result = this.OnStartup(application);

            this.OnAfter(application);

            return result;
        }

        void OnBefore(UIControlledApplication application)
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;

            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod;

            if (m_Application == null)
            {
                m_Application = (UIApplication)application.GetType().InvokeMember("getUIApplication", flags, Type.DefaultBinder, application, null);
            }

            if (RevitCommandHelper.Instance == null)
            {
                RevitCommandHelper.Instance = new RevitCommandHelper();
            }
        
            RevitCommandHelper.Instance.RegisterCommand<RevitCommandInvoke>();
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);

            var file = System.IO.Path.Combine(WorkPath, string.Format("{0}.dll", assemblyName.Name));

            if (System.IO.File.Exists(file))
            {
                return Assembly.LoadFrom(file);
            }

            return args.RequestingAssembly;
        }

        void OnAfter(UIControlledApplication application)
        {

        }
    }
}
