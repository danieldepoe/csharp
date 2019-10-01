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
    public class XCreateCurveFromPoints : Command
    {
        static XCreateCurveFromPoints _instance;
        public XCreateCurveFromPoints()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XCreateCurveFromPoints command.</summary>
        public static XCreateCurveFromPoints Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XCreateCurveFromPoints"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            UsefulFunctions.XSelectPoints("Select points to make CRAZY curves and lines", out Point3dList pointer);

            if (pointer.Count < 1)
                return Result.Failure;

            // The declarations could be done like this too, but above is an 'inline declaration'

            //  Point3dList pointer
            //
            //  UsefulFunctions.XSelectPoints("Select points to make a curve", out pointer);

            //RhinoApp.WriteLine("The user selected {0} points successfully", pointer.Count.ToString());

            UsefulFunctions.XCurveFromPoints(pointer, doc, true);
            UsefulFunctions.XCurveFromPoints(pointer, doc, false);

            UsefulFunctions.XLinesFromPoints(pointer, doc);
            UsefulFunctions.XPolylineFromPoints(pointer, doc);


            return Result.Success;
        }
    }
}