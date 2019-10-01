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
    public class XCreateArcs : Command
    {
        static XCreateArcs _instance;
        public XCreateArcs()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XCreateArcs command.</summary>
        public static XCreateArcs Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XCreateArcs"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            UsefulFunctions.XSelectPoints("Select points to make an arc and/or circles", out Point3dList pointer);

            /*if(pointer.Count == 3)
             {
                 UsefulFunctions.XDrawArc(pointer[0], pointer[1], pointer[2], doc);
                 UsefulFunctions.XDrawCircle(pointer[0], 10, doc);
                 UsefulFunctions.XDrawCircle(pointer[1], 5, doc);
                 UsefulFunctions.XDrawCircle(pointer[2], 2, doc);
             }*/

            Random rnd = new Random();

            for(var i = 0;i<pointer.Count;i++)
            {
                double rndnr = 10 * rnd.NextDouble();

                UsefulFunctions.XDrawCircle(pointer[i], rndnr, doc);
            }

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}