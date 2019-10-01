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
    public class MakeText : Command
    {
        static MakeText _instance;
        public MakeText()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XMakeText command.</summary>
        public static MakeText Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XMakeText"; }
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

                //Plane myplane = new Plane(new Point3d (0,0,0), new Vector3d (0,0,1))

                Plane myplane = doc.Views.ActiveView.ActiveViewport.ConstructionPlane();

                //this will access the current construction plane that is set by the user.

                myplane.Origin = points[i];

                double height = 1;
                string font = "Arial";

                doc.Objects.AddText(str, myplane, height, font, false, false, TextJustification.Center);

            }


            doc.Views.Redraw();

            return Result.Success;
        }
    }
}