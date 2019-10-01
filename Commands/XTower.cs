using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Collections;
using Rhino.DocObjects;

// TRAMSFORMS INCLUDE:  TRANSFORMATION, SCALING AND ROTATION.  

namespace pacsharp1.Commands
{
    public class XTower : Command
    {
        static XTower _instance;
        public XTower()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XTower command.</summary>
        public static XTower Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XTower"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            UsefulFunctions.XSelectCurves("Select curves for brep creation, translation and scaling", out CurveList curves);

            Brep brep = null;

                if (curves.Count >= 3 && curves.Count <= 4)
                {
                    brep = UsefulFunctions.XCreateBrepFromEdgeCurves(curves);
                              

                    //if (brep != null && brep.IsValid)
                        //doc.Objects.AddBrep(brep);
                }

            if (brep == null)
                return Result.Failure;

            //continue function...

            for(var i=0;i<20;i++)
            {
                Random rnd = new Random();
                var rndrange = (Math.PI / 10) * (rnd.NextDouble());
                                               

                var xformtrans = Transform.Translation(0, 0, 5);
                var xformrot = Transform.Rotation(rndrange, brep.GetBoundingBox(true).Center);

                var xformscale = new Transform();

                if (i < 7)
                    xformscale = Transform.Scale(brep.GetBoundingBox(true).Center,1.1); //enlarges by 10%
                else if (i > 7 && i <11)
                    xformscale = Transform.Scale(brep.GetBoundingBox(true).Center, 0.8); //shrinks by 20%
                else
                    xformscale = Transform.Scale(brep.GetBoundingBox(true).Center, 1.05); //enlarges by 5%



                // Transform.Multiply
                //  https://developer.rhino3d.com/api/RhinoCommon/html/M_Rhino_Geometry_Transform_Multiply.htm

                brep.Transform(xformtrans);
                brep.Transform(xformrot);
                brep.Transform(xformscale);


                doc.Objects.AddBrep(brep);
            }

            // https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Geometry_Transform.htm

            RhinoApp.WriteLine("The user created the brep(s) successfully");

            return Result.Success;
        }
    }
}