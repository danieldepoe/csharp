using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Collections;


/*SELECT POINTS
 * 
 * how to pick a point, save it, and get it's info.  
 * 
 * get result enumeration can be found at
 * 
 * https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Input_GetResult.htm
 * 
 * 
 * 
 * 
 * */

namespace pacsharp1.Commands
{
    public class xpointpicker : Command
    {
        static xpointpicker _instance;
        public xpointpicker()
        {
            _instance = this;
        }

        ///<summary>The only instance of the xpointpicker command.</summary>
        public static xpointpicker Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "xpointpicker"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Point3dList points = new Point3dList();

            // Point3dList is from the Collections class, we add a new variable points then initialize it and add 
            // empty array

            Result commandres;

            //calls class Point3dList and initializes a new empty array with the name 'points'

            while(true)

                //make an infinite loop.  every time we click on screen we will create a point which will
                // be added to the document and the screen will be refreshed.  it happens fast!!
            {
                var gp = new GetPoint();

                //prompt user
                string prompt;
                prompt = "set point(s) by click";

                gp.SetCommandPrompt(prompt);


                //set up what to do with the result
                var res = gp.Get();

                //get function returns a really cool type of enumeration
                //  https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Input_GetResult.htm

                if (res == GetResult.Point)

                    //using Rhino.Input, double equal sign is necessary...
                   
                {
                    doc.Objects.AddPoint(gp.Point());
                    //adds a point object to the document to object table

                    doc.Views.Redraw();
                    //refreshes the views

                    points.Add(gp.Point());
                    // add the point to our points array declared above

                    commandres = Result.Success;

                }

                else if (res == GetResult.Nothing)
                {
                    commandres = Result.Failure;
                    break;

                }
                else
                {
                    commandres = Result.Cancel;
                    break;
                }

                


            }

            RhinoApp.WriteLine("The user drew {0} point(s) successfully", points.Count.ToString());

            // access the points array and cast it to a string, outside the function so that it will only
            // show how many ponints you've drawn when the function is complete.  

            return Result.Success;
        }
    }
}