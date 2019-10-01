using System;
using Rhino;
using Rhino.Commands;

namespace pacsharp1.Commands
{
    public class AddArrowHead : Command
    {
        static AddArrowHead _instance;
        public AddArrowHead()
        {
            _instance = this;
        }

        ///<summary>The only instance of the AddArrowHead command.</summary>
        public static AddArrowHead Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "AddArrowHead"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        
            {
                // Define a line
                var line = new Rhino.Geometry.Line(new Rhino.Geometry.Point3d(0, 0, 0), new Rhino.Geometry.Point3d(10, 0, 0));

                // Make a copy of Rhino's default object attributes
                var attribs = doc.CreateDefaultAttributes();

                // Modify the object decoration style
                attribs.ObjectDecoration = Rhino.DocObjects.ObjectDecoration.StartArrowhead;

                // Create a new curve object with our attributes
                doc.Objects.AddLine(line, attribs);
                doc.Views.Redraw();

                return Rhino.Commands.Result.Success;
            }
            
        
        
    }
}