using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Collections;
using Rhino.DocObjects;
using pacsharp1.Class;
using System;

namespace pacsharp1.Commands
{
    public class XGeneratePanels : Command
    {
        static XGeneratePanels _instance;
        public XGeneratePanels()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XGeneratePanels command.</summary>
        public static XGeneratePanels Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XGeneratePanels"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            //create a single object of our brand new Panel class

            //Panel panel = new Panel();

            // we are calling our constructor that initializes the corner points and nothing else

            UsefulFunctions.XSelectPoints("Select four points in the right order", out Point3dList newcornerpts);

            if (newcornerpts.Count != 4)
                return Result.Failure;

            Rhino.DocObjects.Layer parentlayer = UsefulFunctions.SetLayer(doc, "L", System.Drawing.Color.Coral); //not overloaded, sets layer to work on

            int rep = 20;
            Random rnd = new Random(); 

            
            for (var i=0;i<rep;i++)
            {
                Panel panel = new Panel(); 
                
                
                panel.SetCornerPts(newcornerpts);
                panel.SetDotName(new TextDot(i.ToString(), newcornerpts.BoundingBox.Center));

                if (panel.Rebuild())
                {
                    UsefulFunctions.SetLayer(doc, "Panels", System.Drawing.Color.Turquoise, parentlayer); //overloaded, sets the layer so we can draw on it

                    doc.Objects.AddBrep(panel.brep); // add the brep

                    UsefulFunctions.SetLayer(doc, "Panel dots", System.Drawing.Color.DarkRed, parentlayer); //overloaded, sets the layer so we can draw on it

                    doc.Objects.AddTextDot(panel.GetDotName()); //add the dot

                    doc.Views.Redraw();
                }
                else
                    continue;

                //translation

                var xform = Transform.Translation(new Vector3d(newcornerpts[1] - newcornerpts[0]));
                newcornerpts.Transform(xform);

                //rotation
                    int rndrange = rnd.Next(-10, 10);
                    double rndanglerad = ((double)rndrange / 180) * Math.PI;

                    var xformrot = Transform.Rotation(rndanglerad, newcornerpts[0]);
                    newcornerpts.Transform(xformrot);

            }
                                

            return Result.Success;

        }

        
    }
}