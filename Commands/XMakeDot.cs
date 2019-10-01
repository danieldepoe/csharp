using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Collections;
using Rhino.DocObjects;

namespace pacsharp1.Commands
{
    public class XMakeDot : Command
    {
        static XMakeDot _instance;
        public XMakeDot()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XMakeDot command.</summary>
        public static XMakeDot Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XMakeDot"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            UsefulFunctions.XSelectPoints("Seleect points for enumeration", out Point3dList points);

            var format = string.Format("F{0}", doc.ModelDistanceDisplayPrecision);

            for (var i = 0; i < points.Count; i++)
            {
                var str = string.Format("Point {0} X:{1},Y:{2},Z:{3}",
                                        i.ToString(),
                                        points[i].X.ToString(format),
                                        points[i].Y.ToString(format),
                                        points[i].Z.ToString(format));
                if (i == 0)
                    str = "First Point";
                else if (i == points.Count - 1)
                    str = "Last Point";

                var dot = new TextDot(str, points[i]);

                doc.Objects.AddTextDot(dot);
            }

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}