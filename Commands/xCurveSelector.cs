using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Collections;
using Rhino.DocObjects;

/* LISTS AND ARRAYS
 * 
 * You can program with single variables (a single storage of data), but more often than not, you will be dealing
 * with groups of data.  In programming language they are called arrays and in the .NET framework they are called LISTS.
 * 
 * If you know how many elements your array will have, you can simply define it:
 * 
 * int[] my array = new int[5];
 * 
 * or to define the elements, you would do this:
 * 
 * int[] myarray = {5,25,56,67,7};
 * 
 * if you want to access the 3rd element, you would write:
 * 
 * int thirdelement = myarray [2];
 * 
 *The Rhino SDK has it's own type of list, called RhinoList - where you define the type of list within the <>
 * 
 * RhinoList<Point>();
 * 
 * There are also another type of lists, for example Point3dList and CurveList - for this type of list, you don't need to specify type
 * 
 * var curves = new CurveList();
 * 
 * If you wanted to set an array with a specific capacity, you could write
 * 
 * int arrcount = 10'
 * var mylist = new List<Curve>(arrcount);
 * 
 * You can also clear and remove items from lists
 * 
 * curves.Clear     https://developer.rhino3d.com/api/RhinoCommon/html/M_Rhino_Collections_RhinoList_1_Clear.htm
 * 
 * curves.Remove    https://developer.rhino3d.com/api/RhinoCommon/html/M_Rhino_Collections_RhinoList_1_Remove.htm
 * 
 * Point3dList I = new Point3dList();
 * 
 *  l.SetAllX
 *  
 *  https://developer.rhino3d.com/api/RhinoCommon/html/M_Rhino_Collections_Point3dList_SetAllX.htm
 * 
 * 
 * */



namespace pacsharp1.Commands
{
    public class xCurveSelector : Command
    {
        static xCurveSelector _instance;
        public xCurveSelector()
        {
            _instance = this;
        }

        ///<summary>The only instance of the xCurveSelector command.</summary>
        public static xCurveSelector Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "xCurveSelector"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var gc = new GetObject();

            gc.SetCommandPrompt("Select some FANCY curves");
            gc.GeometryFilter = ObjectType.Curve;
            gc.GetMultiple(1, 0);

            if (gc.CommandResult() != Result.Success)
                return gc.CommandResult();

            int linecount = 0;
            int arccount = 0;
            int circlecount = 0;
            int polylinecount = 0;

                        
            //create a collection of curves

            // var curves = new List<Curve>(gc.ObjectCount); OR

            var curves = new CurveList();

            for (var i=0; i<gc.ObjectCount; i++)
            {
                var curve = gc.Object(i).Curve();
                if (null != curve)
                    curves.Add(curve);
                    // ADD THE CURVE TO THE CURVES LIST, expanding dynamically through the loop

                LineCurve line_curve = curve as LineCurve;  //check if curve is a line, 'as' is simplest form of casting
                if (line_curve != null)     // so long as the selection is not null, the our curve is a line
                    linecount++;            // then we can increase our linecount

                PolylineCurve polyline_curve = curve as PolylineCurve;  
                if (polyline_curve != null)     
                    polylinecount++;

                ArcCurve arc_curve = curve as ArcCurve;
                if (arc_curve != null)
                {
                    if (arc_curve.IsCircle())
                        circlecount++;
                    else
                        arccount++;
                }

                curve.Domain = new Interval(0, 1);

                //this will force all curves to have the Domain from 0 to 1

                doc.Objects.AddPoint(curve.PointAtStart);
                doc.Objects.AddPoint(curve.PointAtEnd);
                doc.Objects.AddPoint(curve.PointAt(0.75));

                //add points along the domain...
                

                var format = string.Format("F{0}", doc.DistanceDisplayPrecision);

                string crvinfo = string.Format("The curve {0} has the length {1} and domain: {2} to {3}. Degree {4}. Points at 0,0.75 and 1 of domain.", 
                    i,
                    curve.GetLength().ToString(format),                    
                    curve.Domain.T0.ToString (format),
                    curve.Domain.T1.ToString (format),
                    curve.Degree.ToString(format));
                
                RhinoApp.WriteLine(crvinfo);
                
            }

            doc.Views.Redraw();

            string s = string.Format("{0} line(s), {1} polyline(s), {2} circle(s), {3} arc(s) and {4} curve(s) selected in total",
                    linecount.ToString(), polylinecount.ToString(), circlecount.ToString(), arccount.ToString(), curves.Count.ToString());

            RhinoApp.WriteLine(s);

            return Result.Success;

            
        }
    }
}

