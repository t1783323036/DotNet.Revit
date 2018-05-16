using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.DotNet.RevitUI
{
    public static class DocumentExtension
    {
        /// <summary>
        /// 使用委托启动事务.事务内自动进行事务启动，提交、回滚等处理。
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="action">The action.</param>
        /// <param name="name">The name.</param>
        public static void Invoke(this Document doc, Action<Transaction> action, string name = "default")
        {
            using (var tr = new Transaction(doc, name))
            {
                tr.Start();

                action(tr);

                var status = tr.GetStatus();

                switch (status)
                {
                    case TransactionStatus.Started:
                        tr.Commit();
                        return;
                    case TransactionStatus.Committed:
                    case TransactionStatus.RolledBack:
                        return;
                    case TransactionStatus.Error:
                        tr.RollBack();
                        return;
                    default:
                        return;
                }
            }
        }

        /// <summary>
        /// 使用委托启动事务,并返回委托执行结果.事务内自动进行事务启动，提交、回滚等处理。
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="doc">The document.</param>
        /// <param name="func">The action.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static TResult Invoke<TResult>(this Document doc, Func<Transaction, TResult> func, string name = "default")
        {
            using (var tr = new Transaction(doc, name))
            {
                tr.Start();

                var result = func(tr);

                var status = tr.GetStatus();
                switch (status)
                {
                    case TransactionStatus.Started:
                        tr.Commit();
                        return result;
                    case TransactionStatus.Committed:
                    case TransactionStatus.RolledBack:
                        return result;
                    case TransactionStatus.Error:
                        tr.RollBack();
                        return result;
                    default:
                        return result;
                }
            }
        }
    }
}
