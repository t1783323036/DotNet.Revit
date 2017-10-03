using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Revit.Hook
{
    class ElementMonitor
    {
        private SelectedStatus m_SelectedStatus;
        private bool m_IsMonitor;
        private MouseHook m_MouseHook;

        internal ElementMonitor(UIApplication uiApp)
        {

        }

    }

    /// <summary>
    /// 元素的选择状态.
    /// </summary>
    enum SelectedStatus
    {
        /// <summary>
        /// 单选
        /// </summary>
        RadioSelected,

        /// <summary>
        /// 多选
        /// </summary>
        MultiSelected,

        /// <summary>
        /// 无选择
        /// </summary>
        NoSelected
    }
}
