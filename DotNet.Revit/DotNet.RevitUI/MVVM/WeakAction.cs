using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.RevitUI.MVVM
{
    class WeakAction
    {
        private Action m_StaticAction;

        protected System.Reflection.MethodInfo Method { get; set; }

        public virtual string MethodName
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

        protected WeakReference ActionReference { get; set; }

        protected WeakReference Reference { get; set; }

        public bool IsStatic
        {
            get
            {
                return m_StaticAction != null;
            }
        }

        protected WeakAction()
        {

        }
        public WeakAction(Action action)
            : this(action?.Target, action)
        {
        }

        public WeakAction(object target, Action action)
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

        public virtual bool IsAlive
        {
            get
            {
                if (m_StaticAction == null
                    && Reference == null)
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

        public object Target
        {
            get
            {
                if (Reference == null)
                {
                    return null;
                }

                return Reference.Target;
            }
        }

        protected object ActionTarget
        {
            get
            {
                if (ActionReference == null)
                {
                    return null;
                }

                return ActionReference.Target;
            }
        }

        public void Execute()
        {
            if (m_StaticAction != null)
            {
                m_StaticAction();
                return;
            }

            var actionTarget = ActionTarget;

            if (IsAlive)
            {
                if (Method != null
                    && ActionReference != null
                    && actionTarget != null)
                {
                    Method.Invoke(actionTarget, null);
                    return;
                }
            }
        }
        public void MarkForDeletion()
        {
            Reference = null;
            ActionReference = null;
            Method = null;
            m_StaticAction = null;
        }
    }
}
