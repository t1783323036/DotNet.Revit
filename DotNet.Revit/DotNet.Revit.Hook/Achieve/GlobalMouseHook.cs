using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Revit.Hook.Achieve
{
    /// <summary>
    /// 全局鼠标钩子
    /// </summary>
    /// <seealso cref="DotNet.Hook.Achieve.MouseHookBase" />
    public class GlobalMouseHook : MouseHookBase
    {
        public GlobalMouseHook(IntPtr processId)
            : base(processId)
        {

        }
    }
}
