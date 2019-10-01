using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Collections;
using Rhino.DocObjects;


//NOT WORKING 

namespace pacsharp1.Commands
{
    public class XMakeTextLineLength : Command
    {
        static XMakeTextLineLength _instance;
        public XMakeTextLineLength()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XMakeTextLineLength command.</summary>
        public static XMakeTextLineLength Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XMakeTextLineLength"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            UsefulFunctions.XSelectCurves("Select curves for enumeration", out CurveList curves);

            var format = string.Format("F{0}", doc.ModelDistanceDisplayPrecision);

            for (var i = 0; i < curves.Count; i++)
            {
                var str = string.Format("Curve {0} Length = {1}",
                                        i.ToString(),
                                        curves[i].Dimension.ToString(format));



                //Plane myplane = new Plane(new Point3d (0,0,0), new Vector3d (0,0,1))

                Plane myplane = doc.Views.ActiveView.ActiveViewport.ConstructionPlane();

                //this will access the current construction plane that is set by the user.

                myplane.Origin = curves[i].PointAtEnd;

                double height = 1;
                string font = "Arial";

                doc.Objects.AddText(str, myplane, height, font, false, false, TextJustification.Center);

                //var dot = new TextObject(str, points[i]);

                
            }


            doc.Views.Redraw();

            
            return Result.Success;
        }
    }
}