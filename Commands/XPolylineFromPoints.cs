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
    public class XPolylineFromPoints : Command
    {
        static XPolylineFromPoints _instance;
        public XPolylineFromPoints()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XPolylineFromPoints command.</summary>
        public static XPolylineFromPoints Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XPolylineFromPoints"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            UsefulFunctions.XSelectPoints("Select points to make CRAZY curves and lines", out Point3dList pointer);

            if (pointer.Count < 1)
                return Result.Failure;

            UsefulFunctions.XPolylineFromPoints(pointer, doc);

            return Result.Success;
        }
    }
}