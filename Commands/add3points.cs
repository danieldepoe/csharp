using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;

namespace pacsharp1.Commands
{
    public class add3points : Command
    {
        static add3points _instance;
        public add3points()
        {
            _instance = this;
        }

        ///<summary>The only instance of the MyCommand1 command.</summary>
        public static add3points Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "add3points"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Point3d pt0 = new Point3d(0, 0, 0);
            Point3d pt1 = new Point3d(0, 1, 0);
            Point3d pt2 = new Point3d(0, 5, 5);

            doc.Objects.AddPoint(pt0);
            doc.Objects.AddPoint(pt1);
            doc.Objects.AddPoint(pt2);

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}