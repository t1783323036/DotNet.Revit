using Autodesk.Windows;
using Autodesk.Windows.ToolBars;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace DotNet.RevitUI.Helper
{

    class NonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// 

    public static class RibbonBindingHelper
    {
        /// <summary>
        /// 自动关联绑定Ribbon子控件数据.
        /// </summary>
        /// <param name="ribbon">The ribbon.</param>
        /// <param name="dataContext">The data context.</param>

        public static void Binding(RibbonControl ribbon, object dataContext)
        {
            if (ribbon == null)
            {
                throw new NullReferenceException("ribbon parameter is null rference !");
            }

            if (ribbon.Tabs.Count == 0)
            {
                return;
            }

            if (dataContext == null)
            {
                dataContext = ribbon.DataContext;
            }

            if (dataContext == null)
            {
                return;
            }

            var eum = ribbon.Tabs.GetEnumerator();

            while (eum.MoveNext())
            {
                var current = eum.Current;

                foreach (var panel in current.Panels)
                {
                    ItemBinding(panel, dataContext);

                    if (panel.Source.DialogLauncher != null)
                    {
                        ItemBinding(panel.Source.DialogLauncher, dataContext);
                    }

                    foreach (var item in panel.Source.Items)
                    {
                        ItemRecursive(item, dataContext);
                    }
                }
            }
        }

        /// <summary>
        /// 递归遍历Item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="dataContext">The data context.</param>

        private static void ItemRecursive(RibbonItem item, object dataContext)
        {
            if (item == null)
            {
                return;
            }

            switch (item)
            {
                case RibbonListButton listButton:

                    listButton.CommandHandler = new NonCommand();

                    for (int i = 0; i < listButton.Items.Count; i++)
                    {
                        RibbonBindingHelper.ItemRecursive(listButton.Items[i], dataContext);
                    }

                    break;

                case ToolBarHistoryButton toolBar:

                    RibbonBindingHelper.ItemBinding(item, dataContext);

                    for (int i = 0; i < toolBar.Items.Count; i++)
                    {
                        var current = toolBar.Items[i] as RibbonItem;

                        if (current == null)
                        {
                            continue;
                        }

                        RibbonBindingHelper.ItemRecursive(current, dataContext);
                    }

                    break;
                case RibbonMenuItem menuItem:

                    for (int i = 0; i < menuItem.Items.Count; i++)
                    {
                        RibbonBindingHelper.ItemRecursive(menuItem.Items[i], dataContext);
                    }

                    break;

                case RibbonList list:


                    for (int i = 0; i < list.Items.Count; i++)
                    {
                        var current = list.Items[i] as RibbonItem;

                        if (current == null)
                        {
                            continue;

                        }
                        RibbonBindingHelper.ItemRecursive(current, dataContext);
                    }

                    break;
                case RibbonRowPanel rowPanel:

                    for (int i = 0; i < rowPanel.Items.Count; i++)
                    {
                        RibbonBindingHelper.ItemRecursive(rowPanel.Items[i], dataContext);
                    }

                    break;
                default:

                    RibbonBindingHelper.ItemBinding(item, dataContext);

                    break;
            }
        }

        static void ItemBinding(object obj, object dataContext)
        {
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj, null);

                if (value == null)
                {
                    continue;
                }

                if (value is Binding binding)
                {
                    if (string.IsNullOrEmpty(binding.Path.Path) || binding.Source != null)
                    {
                        continue;
                    }

                    try
                    {
                        binding.Source = dataContext;

                        if (obj is RibbonCommandItem commandItem
                            && property.Name == "CommandHandlerBinding")
                        {
                            commandItem.CommandParameter = obj;
                        }
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Debug.WriteLine(ex.Message);
#endif
                    }
                }
            }
        }
    }
}
