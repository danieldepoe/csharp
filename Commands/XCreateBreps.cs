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
    public class XCreateBreps : Command
    {
        static XCreateBreps _instance;
        public XCreateBreps()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XCreateBreps command.</summary>
        public static XCreateBreps Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XCreateBreps"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            UsefulFunctions.XSelectCurves("Select curves for brep creation", out CurveList curves);

            if(curves.Count >= 3 && curves.Count <= 4)
            {
                Brep brep = UsefulFunctions.XCreateBrepFromEdgeCurves(curves);

                RhinoApp.WriteLine("The user created the brep(s) successfully");

                if (brep != null && brep.IsValid)
                    doc.Objects.AddBrep(brep);
            }

            else
            {
                Brep[] breps = UsefulFunctions.XCreateBrepFromPlanarCurves(curves, doc);

                for(var i = 0; i<breps.Length; i++)

                {
                    if (breps[i] != null && breps[i].IsValid)
                        doc.Objects.AddBrep(breps[i]);



                }

            }

            return Result.Success;

            
               
        }
    }
}