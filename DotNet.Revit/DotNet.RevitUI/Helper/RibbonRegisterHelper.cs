
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DotNet.RevitUI.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public static class RibbonRegisterHelper
    {
        /// <summary>
        /// 基于指定泛型自动注册Ribbon下的Tab.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Register<T>()
            where T : FrameworkElement, new()
        {
            var frameworkElement = new T();

            var flag = RibbonRegisterHelper.Register(frameworkElement);

            if (flag)
            {
                return frameworkElement;
            }

            return default(T);
        }

        /// <summary>
        /// 基于指定控件自动注册Ribbon下的Tab.
        /// </summary>
        /// <param name="frameworkElement">The framework element.</param>
        /// <returns></returns>
        public static bool Register(FrameworkElement frameworkElement)
        {
            var mainRibbon = Autodesk.Windows.ComponentManager.Ribbon;

            if (mainRibbon == null)
            {
                return false;
            }

            var ribbons = new List<KeyValuePair<RibbonControl, object>>();

            if (frameworkElement is RibbonControl ribbon)
            {
                ribbons.Add(new KeyValuePair<RibbonControl, object>(ribbon, ribbon.DataContext));
            }
            else
            {
                var temps = new List<RibbonControl>();

                frameworkElement.GetRibbonControl(ref temps);

                foreach (var item in temps)
                {
                    var dataContext = frameworkElement.DataContext;
                    if (dataContext == null)
                    {
                        dataContext = item.DataContext;
                    }

                    ribbons.Add(new KeyValuePair<RibbonControl, object>(item, dataContext));
                }
            }

            if (ribbons.Count == 0)
            {
                return false;
            }

            var eum = ribbons.GetEnumerator();

            while (eum.MoveNext())
            {
                var tabs = eum.Current.Key.Tabs;

                RibbonBindingHelper.Binding(eum.Current.Key, eum.Current.Value);

                foreach (var item in tabs)
                {
                    mainRibbon.Tabs.Add(item);
                }
            }
            return true;
        }

        static void GetRibbonControl(this DependencyObject obj, ref List<RibbonControl> ribbons)
        {
            var eum = LogicalTreeHelper.GetChildren(obj).GetEnumerator();

            while (eum.MoveNext())
            {
                var current = eum.Current;

                if (current is RibbonControl)
                {
                    ribbons.Add((RibbonControl)current);
                }

                if (current is DependencyObject depend)
                {
                    GetRibbonControl(depend, ref ribbons);
                }
            }
        }
    }
}
