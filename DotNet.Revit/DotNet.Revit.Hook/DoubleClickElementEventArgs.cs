using Autodesk.Revit.DB;
using System;

namespace DotNet.Revit.Hook
{
    public class DoubleClickElementEventArgs : EventArgs
    {
        public DoubleClickElementEventArgs(Element elem)
        {
            this.Element = elem;
        }

        public Element Element { get; private set; }
    }
}