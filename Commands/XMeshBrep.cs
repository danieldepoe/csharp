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
    public class XMeshBrep : Command
    {
        static XMeshBrep _instance;
        public XMeshBrep()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XMeshBrep command.</summary>
        public static XMeshBrep Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XMeshBrep"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            
            UsefulFunctions.XSelectBreps("Select breps for meshing", out RhinoList<Brep> breps);

            for(var i = 0; i<breps.Count; i++)
            {
                //mesh create form brep method
                // https://developer.rhino3d.com/api/RhinoCommon/html/M_Rhino_Geometry_Mesh_CreateFromBrep_1.htm

                //MeshingParameters mp = new MeshingParameters();

                //mp.MinimumEdgeLength = 4;
                //mp.MaximumEdgeLength = 12;

                var meshes = Mesh.CreateFromBrep(breps[i], MeshingParameters.FastRenderMesh);
                               
                var weldedmesh = new Mesh(); //just creates a new mesh object

                foreach (var mesh in meshes) // foreach is just a different type of iterator
                    weldedmesh.Append(mesh);  //not a truly welded mesh, just joined together.  
                                                // we do this because the meshing of a brep may result in several
                                                  //meshes.  Here, we 'weld' them into one. 
                        
                doc.Objects.AddMesh(weldedmesh);            

            }

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}