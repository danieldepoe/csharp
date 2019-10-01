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
    public class XSweepTest : Command
    {
        static XSweepTest _instance;
        public XSweepTest()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XSweepTest command.</summary>
        public static XSweepTest Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XSweepTest"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Rhino.DocObjects.ObjRef rail_ref;
            var rc = RhinoGet.GetOneObject("Select rail curve", false, Rhino.DocObjects.ObjectType.Curve, out rail_ref);
            if (rc != Rhino.Commands.Result.Success)
                return rc;

            var rail_crv = rail_ref.Curve();
            if (rail_crv == null)
                return Rhino.Commands.Result.Failure;

            var gx = new Rhino.Input.Custom.GetObject();
            gx.SetCommandPrompt("Select cross section curves");
            gx.GeometryFilter = Rhino.DocObjects.ObjectType.Curve;
            gx.EnablePreSelect(false, true);
            gx.GetMultiple(1, 0);
            if (gx.CommandResult() != Rhino.Commands.Result.Success)
                return gx.CommandResult();

            var cross_sections = new List<Rhino.Geometry.Curve>();
            for (int i = 0; i < gx.ObjectCount; i++)
            {
                var crv = gx.Object(i).Curve();
                if (crv != null)
                    cross_sections.Add(crv);
            }
            if (cross_sections.Count < 1)
                return Rhino.Commands.Result.Failure;

            var sweep = new Rhino.Geometry.SweepOneRail();
            
            
            sweep.AngleToleranceRadians = doc.ModelAngleToleranceRadians;
            sweep.ClosedSweep = true;
            sweep.SweepTolerance = doc.ModelAbsoluteTolerance;
            sweep.SetToRoadlikeTop();
            sweep.MiterType = 1;

            var breps = sweep.PerformSweep(rail_crv, cross_sections);
            for (int i = 0; i < breps.Length; i++)
                doc.Objects.AddBrep(breps[i]);

            doc.Views.Redraw();

            return Rhino.Commands.Result.Success;
            
            
        }
    }
}