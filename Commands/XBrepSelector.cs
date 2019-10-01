using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Collections;
using Rhino.DocObjects;

/* preselecting!!
 * 
 *  http://developer.rhino3d.com/api/RhinoCommon/html/M_Rhino_Input_Custom_GetObject_EnablePreSelect.htm
 *  
 *  BREPS
    https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Geometry_Brep.htm
 *  *  * 
 * BREP FACE CLASS
 * https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Geometry_BrepFace.htm
 * 
 * BREP FACELIST CLASS
 * 
 * https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Geometry_Collections_BrepFaceList.htm
 * 
 * AREA MASS PROPERTIES
 * 
 * https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Geometry_AreaMassProperties.htm
 * 
 * COMPUTE METHOD
 * 
 * https://developer.rhino3d.com/api/RhinoCommon/html/Overload_Rhino_Geometry_AreaMassProperties_Compute.htm
*/


namespace pacsharp1.Commands
{
    public class XBrepSelector : Command
    {
        static XBrepSelector _instance;
        public XBrepSelector()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XBrepSelector command.</summary>
        public static XBrepSelector Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XBrepSelector"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            int i, j;

            var gc = new GetObject();

            gc.SetCommandPrompt("Select some Breps");
            gc.EnablePreSelect(true, true);
            gc.GeometryFilter = ObjectType.Brep;
            gc.GetMultiple(1, 0);

            if (gc.CommandResult() != Result.Success)
                return gc.CommandResult();

            RhinoList<Brep> breps = new RhinoList<Brep>();

            for (i = 0;i<gc.ObjectCount; i++)
            {
                var brep = gc.Object(i).Brep();
                if (null != brep)
                    breps.Add(brep);

                var format = string.Format("F{0}", doc.DistanceDisplayPrecision);

                var brepinfo = string.Format("Brep {0} has {1} face(s) and total surface of {2} square {3}", 
                                            i.ToString(), 
                                            brep.Faces.Count, 
                                            brep.GetArea().ToString(format), 
                                            doc.ModelUnitSystem.ToString()
                                            );

                for (j=0; j<brep.Faces.Count;j++)
                {

                    AreaMassProperties brepfacemass = AreaMassProperties.Compute(brep.Faces[j]);

                    double brepfaceearea = brepfacemass.Area;

                    var brepfaceinfo = string.Format("Brepface {0} belongs to brep {1} and has a surface of {2} square {3}",
                                            j.ToString(),
                                            i.ToString(),
                                            brepfaceearea.ToString(format),
                                            doc.ModelUnitSystem.ToString()
                                            );

                    var brepfacedomain = string.Format(" and its Domain is U:{0} to {1} and V: {2} to {3}",
                                            brep.Faces[j].Domain(0).T0.ToString(format),
                                            brep.Faces[j].Domain(0).T1.ToString(format),
                                            // Domain(0) means U, start at T0, end at T1
                                            brep.Faces[j].Domain(1).T0.ToString(format),
                                            brep.Faces[j].Domain(1).T1.ToString(format)
                                            // Domain(1) means V
                                            );

                    RhinoApp.WriteLine(brepfaceinfo + brepfacedomain);

                }

                RhinoApp.WriteLine(brepinfo);
            }



            //if the breps are not joined, they will be joined with the function below. 

            Brep[] joinedbreps = Brep.JoinBreps(breps, doc.ModelAbsoluteTolerance);
            
            for(i=0; i<joinedbreps.Length; i++)
            // we use the .Length instead of .Count because we have a <Standard> array from Rhino.
            {
                doc.Objects.AddBrep(joinedbreps[i]);
            }

            doc.Views.Redraw();

            RhinoApp.WriteLine("The user selected {0} brep(s) successfully, and they have been joined.", breps.Count.ToString());


            return Result.Success;
        }
    }
}