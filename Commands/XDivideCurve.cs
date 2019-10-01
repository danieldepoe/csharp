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
    public class XDivideCurve : Command
    {
        static XDivideCurve _instance;
        public XDivideCurve()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XDivideCurve command.</summary>
        public static XDivideCurve Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XDivideCurve"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            int i;

            CurveList selectedcurves = new CurveList();
            int divnr = 10;

            Point3d[] divptarray;
            //declares variable as a standard array in .NET, for use with the out overload below. 



            // step one, select curves --------------------------
            
            var gc = new GetObject();
            
            gc.SetCommandPrompt("Select curves to divide into 10 sections (start+end points added)");
            gc.GeometryFilter = ObjectType.Curve;
            gc.GetMultiple(1, 0);

            if (gc.CommandResult() != Result.Success)
                return gc.CommandResult();

                  
            //enter the loop            

            for (i = 0; i < gc.ObjectCount; i++)
            {
                var curve = gc.Object(i).Curve();
                if (null != curve && !curve.IsShort(RhinoMath.ZeroTolerance))
                    selectedcurves.Add(curve);
                // ADD THE CURVE TO THE CURVES LIST, expanding dynamically through the loop
            }

            if (selectedcurves.Count < 1)
                return Result.Failure;

            //step 2, divide curves ----------------------------------

            for (i = 0; i<selectedcurves.Count; i++)
            {
                double[] parameters = selectedcurves[i].DivideByCount(divnr, true, out divptarray);

                UsefulFunctions.XDrawPoints(divptarray, doc);

                UsefulFunctions.XDrawCurveTangents(parameters, selectedcurves[i], doc);
            }





                return Result.Success;
        }
    }
}