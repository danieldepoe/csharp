using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Collections;
using Rhino.DocObjects;

namespace pacsharp1.Class
{
    class Panel 
    {
        //private methods will not be accessible only from within this class, and those that inherit from this class
        private Point3dList cornerpts;
        //public methods will be accessible from anywhere through the dot operators
        public Brep brep;
        public TextDot dotname;

        //DEFUALT VERSION
        // private int nr;


        // public int Nr { get; set; }

        //LONG FORM VERSION

        public Panel()
        {
            cornerpts = new Point3dList();
        }

        //properties

        public Point3dList GetCornerPts()
        {
            return cornerpts;
        }

        public void SetCornerPts(Point3dList newcornerpts)
        {
            if (cornerpts != null)
                cornerpts.Clear();

            cornerpts = newcornerpts;
        }

        public TextDot GetDotName()
        {
            return dotname;
        }

        public void SetDotName(TextDot newdotname)
        {
            dotname = newdotname;
        }

        //methods
        public bool Rebuild()
        {
            if (cornerpts.Count != 4)
                return false;

            CurveList edgecurves = new CurveList();

            for(var i=0;i<cornerpts.Count-1;i++)
            {
                LineCurve edge = new LineCurve(cornerpts[i], cornerpts[(i + 1)%4]);  //modulo gives remainer after divided by set number
                if (edge.IsValid)
                    edgecurves.Add(edge);
            }

            if (edgecurves.Count != 4)
                return false;

            brep = UsefulFunctions.XCreateBrepFromEdgeCurves(edgecurves);

            if (brep == null || !brep.IsValid)
                return false;

            
            return true;
        }

    }


}
