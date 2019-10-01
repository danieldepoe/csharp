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
    public class XMakeLinearDimension : Command
    {
        static XMakeLinearDimension _instance;
        public XMakeLinearDimension()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XMakeLinearDimension command.</summary>
        public static XMakeLinearDimension Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XMakeLinearDimension"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            UsefulFunctions.XSelectCurves("Select curves for linear dimensioning", out CurveList curves);

            //var lines = new RhinoList<Line>();

            //for (var i = 0; i < curves.Count; i++)
            //{
            //    lines.Add(new Line(curves[i].PointAtStart, curves[i].PointAtEnd));
            //}



            //get the user to select a point

                var gp = new GetPoint();

                string prompt;

                prompt = "Pick a point";

                gp.SetCommandPrompt(prompt);

            
                gp.Get();

            Point3d pt = gp.Point();

            foreach (var curve in curves)
            {
                Point3d origin = curve.PointAtStart;
                Point3d offset = curve.PointAtEnd;

                //Vector3d linevec = new Vector3d(offset - origin);

                // you can rotate vector so its perp. with the line...
                                                

                Plane plane = Plane.WorldXY;

                plane.Origin = origin;

                double u, v;
                plane.ClosestParameter(origin, out u, out v);
                Point2d ext1 = new Point2d(u, v);

                plane.ClosestParameter(offset, out u, out v);
                Point2d ext2 = new Point2d(u, v);

                plane.ClosestParameter(pt, out u, out v);
                Point2d linePt = new Point2d(u, v);

                LinearDimension lindim = new LinearDimension(plane, ext1, ext2, linePt);

                doc.Objects.AddLinearDimension(lindim);
                
            }

            return Result.Success;
        }
    }
}