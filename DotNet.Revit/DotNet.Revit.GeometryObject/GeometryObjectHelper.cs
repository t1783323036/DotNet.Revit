using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Revit
{
    public static class GeometryObjectHelper
    {
        /// <summary>
        /// 获取元素的所有GeomObjects
        /// </summary>
        /// <param name="elem">The elem.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>

        public static List<Autodesk.Revit.DB.GeometryObject> GetGeometryObjects(this Element elem, Options options = default(Options))
        {
            var result = new List<Autodesk.Revit.DB.GeometryObject>();

            options = options ?? new Options();

            GeometryObjectHelper.RecursionObject(elem.get_Geometry(options), ref result);

            return result;
        }

        /// <summary>
        /// 递归遍历所有GeometryObject.
        /// </summary>
        /// <param name="geometryElement">初始GeometryElement.</param>
        /// <param name="geometryObjects">递归结果.</param>

        private static void RecursionObject(this GeometryElement geometryElement, ref List<Autodesk.Revit.DB.GeometryObject> geometryObjects)
        {
            if (geometryElement == null)
            {
                return;
            }

            var eum = geometryElement.GetEnumerator();

            while (eum.MoveNext())
            {
                var current = eum.Current;

                switch (current)
                {
                    case GeometryInstance instance:
                        instance.SymbolGeometry.RecursionObject(ref geometryObjects);
                        break;

                    case GeometryElement elemlemt:
                        elemlemt.RecursionObject(ref geometryObjects);
                        break;

                    case Solid solid:
                        if (solid.Edges.Size == 0 || solid.Faces.Size == 0)
                        {
                            continue;
                        }
                        geometryObjects.Add(current);
                        break;

                    default:
                        geometryObjects.Add(current);
                        break;
                }
            }
        }
    }
}
