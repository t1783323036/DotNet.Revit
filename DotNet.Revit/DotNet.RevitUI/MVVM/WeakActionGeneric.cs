using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.RevitUI.MVVM
{
    class WeakAction<T> : WeakAction, IExecuteWithObject
    {
        private Action<T> m_StaticAction;

        public override string MethodName
        {
            get
            {
                if (m_StaticAction != null)
                {
                    return m_StaticAction.Method.Name;
                }
                return Method.Name;
            }
        }

        public override bool IsAlive
        {
            get
            {
                if (m_StaticAction == null && Reference == null)
                {
                    return false;
                }

                if (m_StaticAction != null)
                {
                    if (Reference != null)
                    {
                        return Reference.IsAlive;
                    }
                    return true;
                }
                return Reference.IsAlive;
            }
        }

        public WeakAction(Action<T> action)
            : this(action?.Target, action)
        {

        }

        public WeakAction(object target, Action<T> action)
        {
            if (action.Method.IsStatic)
            {
                m_StaticAction = action;

                if (target != null)
                {
                    Reference = new WeakReference(target);
                }

                return;
            }

            Method = action.Method;
            ActionReference = new WeakReference(action.Target);

            Reference = new WeakReference(target);
        }

        public new void Execute()
        {
            Execute(default(T));
        }

        public void Execute(T parameter)
        {
            if (m_StaticAction != null)
            {
                m_StaticAction(parameter);
                return;
            }

            var actionTarget = ActionTarget;

            if (IsAlive)
            {
                if (Method != null && ActionReference != null && actionTarget != null)
                {
                    Method.Invoke(actionTarget, new object[] { parameter });
                }
            }
        }

        public void ExecuteWithObject(object parameter)
        {
            var parameterCasted = (T)parameter;
            Execute(parameterCasted);
        }

        public new void MarkForDeletion()
        {
            m_StaticAction = null;
            base.MarkForDeletion();
        }
    }
}
