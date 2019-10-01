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
    public class XTransformation : Command
    {
        static XTransformation _instance;
        public XTransformation()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XTransformation command.</summary>
        public static XTransformation Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XTransformation"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            
            var rc = RhinoGet.GetOneObject("Select brep to transform", true, Rhino.DocObjects.ObjectType.Brep, out ObjRef rhobj);
            if (rc != Rhino.Commands.Result.Success)
                return rc;

            //simple translation transformation
            var xform = Rhino.Geometry.Transform.Translation(18, 25, 50);
            doc.Objects.Transform(rhobj, xform, true);
            doc.Views.Redraw();
            return Rhino.Commands.Result.Success;

            
        }
    }
}