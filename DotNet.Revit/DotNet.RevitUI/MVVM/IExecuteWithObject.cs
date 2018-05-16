using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.RevitUI.MVVM
{
    interface IExecuteWithObject
    {
        object Target
        {
            get;
        }
      
        void ExecuteWithObject(object parameter);

        void MarkForDeletion();
    }
}
