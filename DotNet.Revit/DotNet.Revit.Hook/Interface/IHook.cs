using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Revit.Hook.Interface
{
    /// <summary>
    /// IHook
    /// </summary>
    public interface IHook
    {
        /// <summary>
        /// Installs this Hook.
        /// </summary>
        void Install();

        /// <summary>
        /// Uninstalls this Hook.
        /// </summary>
        void Uninstall();
    }
}
