using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Collections;

/*GET OBJECT CLASS
 * 
 * https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Input_Custom_GetObject.htm
 * 
 * 
 * 
 * 
 * 
 * */

namespace pacsharp1.Commands
{
    public class xPointSelector1 : Command
    {
        static xPointSelector1 _instance;
        public xPointSelector1()
        {
            _instance = this;
        }

        ///<summary>The only instance of the xPointSelector command.</summary>
        public static xPointSelector1 Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "xPointSelector"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var go = new GetObject();

            //GetObject is more general, and allows you to select any type of object.

            go.GeometryFilter = Rhino.DocObjects.ObjectType.Point;

            //user can only select points

            go.GetMultiple(1, 0);

            if (go.CommandResult()! = Result.Success)
            {
                return go.CommandResult();
            }
            // if statement above is just a check to tell us what to do if the command is a success/failure

            var points = new List<Point>(go.ObjectCount);

            // now we have a list, we just have to save them.  Let's use a different type of array using
            // the System.Collections namespace.  The size of the array will be the size of the ObjectCount list.

            for (var i = 0; i < go.ObjectCount; i++)
            {
                var point = go.Object(i).Point();
                if (null != point)
                { }
            }

            RhinoApp.WriteLine("The user selected {0} points successfully", points.Count.ToString());

            return Result.Success;
        }
    }
}