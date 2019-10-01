using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Input.Custom;
using Rhino.Input;



namespace pacsharp1.sandbox

//NOTES FROM THE Rhino Object Class video

// when you  write plugins for Rhino your actions will likely be to select geometry that already exists, creating geometry, or a combination of both.  Yes!!

//We will focus here on selecting geometry that is already in the document, when you draw a object in Rhino it is loaded into your memory.  By accessing those objects you will be able to copy, transform, etc.  

// Visit Rhino Object Class

// https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_DocObjects_RhinoObject.htm


//also look up the ObjRef Constructor

// https://developer.rhino3d.com/api/RhinoCommon/html/M_Rhino_DocObjects_ObjRef__ctor.htm
{
    public class selectcurve : Command
    {
        static selectcurve _instance;
        public selectcurve()
        {
            _instance = this;
        }

        ///<summary>The only instance of the MyCommand1 command.</summary>
        public static selectcurve Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "selectcurve"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            ObjRef object_ref;

            Result res2 = RhinoGet.GetOneObject("Select FANCY curve", false, ObjectType.Curve, out object_ref);

            // RhinoGet takes 5 arguments - prompt as a string, press enter boolean (does not allow the user to press 
            // enter), geometry filter to allow them to only select certain types of geometry, then, most importantly
            // we are going to get our object reference back!!  Via the 'out'.  This means that we are passing this
            // variable by reference and not by value.  Therefore, the value will be able to change, so we can access
            // it outside of this method.

            Curve curve = object_ref.Curve();

            if (null == curve)
                return Result.Failure;

            // this checks if the curve is zero, so it can then return a failure result
            // THERE are a lot of ways to select objects.  Using the ObjRef class is a very powerful way to select
            // geometry.  
            // You will see in the future that whenever we draw something to the document, thereby adding it to memory,
            // we will also be adding a Rhino object to it.  
                                   
            return Result.Success;
        }
    }
}