using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.Utility;
using Autodesk.RevitAddIns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Revit.NET
{
    public class Program
    {
        static readonly string WorkPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);

        static Program()
        {
            RevitCoreContext.Instance.Run();
        }

        /// <summary>
        /// Revit 内核必须加 STAThread 标签
        /// </summary>
        /// <param name="args"></param>

        [STAThread]
        static void Main(string[] args)
        {
            var app = RevitCoreContext.Instance.Application;

            // Todo ...

            var projectrTemplate = app.DefaultProjectTemplate;

            if (!File.Exists(projectrTemplate))
            {
                throw new FileNotFoundException("默认项目路径不存在 , 请指定 !");
            }

            var document = app.NewProjectDocument(projectrTemplate);

            if (document == null)
            {
                throw new InvalidOperationException();
            }

            // create wall demo ...

            var p1 = XYZ.Zero;

            var p2 = p1 + XYZ.BasisX * 10;

            var p3 = p2 + XYZ.BasisY * 10;

            var p4 = p1 + XYZ.BasisY * 10;

            var points = new XYZ[] { p1, p2, p3, p4 };

            document.Invoke(m =>
            {
                var level0 = document.QueryByType<Level>().OfType<Level>().FirstOrDefault(x => Math.Abs(x.Elevation - 0.0) <= 1e-7);

                if (level0 == null)
                {
                    level0 = Level.Create(document, 0);

                    document.Regenerate();
                }

                for (int i = 0; i < points.Length; i++)
                {
                    var a = points[i];

                    var b = i == points.Length - 1 ? points[0] : points[i + 1];

                    Wall.Create(document, Line.CreateBound(a, b), level0.Id, false);
                }
            });

            document.SaveAs(Path.Combine(WorkPath, "demowall.rvt"), new SaveAsOptions() { OverwriteExistingFile = true });

            RevitCoreContext.Instance.Stop();
        }
    }

    public class RevitCoreContext
    {
        // 此路径为动态反射搜索路径 、 此路径可为任意路径（只要路径下有RevitNET 所需依赖项即可，完整依赖项可在 Naviswork 2016 下面找到）

        static readonly string[] Searchs = RevitProductUtility.GetAllInstalledRevitProducts().Select(x => x.InstallLocation).ToArray();

        static readonly object lockobj = new object();

        static RevitCoreContext _instance;

        private Product _product;

        public Application Application { get => _product.Application; }

        public static RevitCoreContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockobj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RevitCoreContext();
                        }
                    }
                }

                return _instance;
            }
        }

        static RevitCoreContext()
        {
            AddEnvironmentPaths(Searchs);

            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        public void Run()
        {
            _product = Product.GetInstalledProduct();

            var clientId = new ClientApplicationId(Guid.NewGuid(), "DotNet", "BIMAPI");

            // I am authorized by Autodesk to use this UI-less functionality. 必须是此字符串。 Autodesk 规定的.

            _product.Init(clientId, "I am authorized by Autodesk to use this UI-less functionality.");
        }

        public void Stop()
        {
            _product?.Exit();
        }

        static void AddEnvironmentPaths(params string[] paths)
        {
            var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

            var newPath = string.Join(System.IO.Path.PathSeparator.ToString(), path.Concat(paths));

            Environment.SetEnvironmentVariable("PATH", newPath);
        }

        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);

            foreach (var item in Searchs)
            {
                var file = string.Format("{0}.dll", System.IO.Path.Combine(item, assemblyName.Name));

                if (File.Exists(file))
                {
                    return Assembly.LoadFile(file);
                }
            }

            return args.RequestingAssembly;
        }
    }

    public static class DocumentExtension
    {
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

        public static void InvokeSub(this Document doc, Action<SubTransaction> action)
        {
            using (var tr = new SubTransaction(doc))
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
                        break;
                    case TransactionStatus.Error:
                        tr.RollBack();
                        return;
                    default:
                        return;
                }
            }
        }


        public static TResult InvokeSub<TResult>(this Document doc, Func<SubTransaction, TResult> func)
        {
            using (var tr = new SubTransaction(doc))
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

        public static void InvokeGroup(this Document doc, Action<TransactionGroup> action, string name = "default")
        {
            using (var tr = new TransactionGroup(doc, name))
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
                        break;
                    case TransactionStatus.Error:
                        tr.RollBack();
                        return;
                    default:
                        return;
                }
            }
        }

        public static TResult InvokeGroup<TResult>(this Document doc, Func<TransactionGroup, TResult> func, string name = "default")
        {
            using (var tr = new TransactionGroup(doc, name))
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

        public static FilteredElementCollector QueryByType<T>(this Document doc) where T : Element
        {
            return new FilteredElementCollector(doc).OfClass(typeof(T));
        }
    }
}
