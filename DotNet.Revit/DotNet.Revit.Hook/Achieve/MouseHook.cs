using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Revit.Hook.Achieve
{
    /// <summary>
    /// 线程鼠标Hook.
    /// </summary>
    /// <seealso cref="DotNet.Hook.Achieve.MouseHookBase" />
    public class MouseHook : MouseHookBase
    {
        public MouseHook(int threadId = 0)
            : base(threadId)
        {

        }
    }

}
