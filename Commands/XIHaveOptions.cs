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
    public class XIHaveOptions : Command
    {
        static XIHaveOptions _instance;
        public XIHaveOptions()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XIHaveOptions command.</summary>
        public static XIHaveOptions Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XIHaveOptions"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            //look at this for a summary of all the GET commands.

            // https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Input_Custom_GetBaseClass.htm.

            /*System.Object
            Rhino.Input.Custom.GetBaseClass
                Rhino.Input.Custom.GetInteger
                Rhino.Input.Custom.GetNumber
                Rhino.Input.Custom.GetObject
                Rhino.Input.Custom.GetOption
                Rhino.Input.Custom.GetPoint
                Rhino.Input.Custom.GetString
                * 
                * */
            var gc = new GetObject();
            gc.SetCommandPrompt("Select objects");
            gc.GeometryFilter = ObjectType.AnyObject;

            OptionInteger intOption = new OptionInteger(1, 1, 99);
            OptionDouble dblOption = new OptionDouble(2.2, 0, 99);
            OptionToggle boolOption = new OptionToggle(true, "off", "on");
                        
            gc.AddOptionInteger("Integer", ref intOption);
            gc.AddOptionDouble("Double", ref dblOption);
            gc.AddOptionToggle("Boolean", ref boolOption);

            int listIndex = 0;
            string[] listValues = new string[] { "Bran", "Bob", "Arya", "Sansa", "Rickon" };

            int optList = gc.AddOptionList("List", listValues, listIndex);

            while(true)
            {
                GetResult res = gc.GetMultiple(1, 0);

                //we can check if the user selected something
                if (gc.CommandResult() != Result.Success)
                    return gc.CommandResult();

                if (res == Rhino.Input.GetResult.Object)
                {
                    RhinoApp.WriteLine("Command line option values are");
                    RhinoApp.WriteLine("Integer = {0}", intOption.CurrentValue);
                    RhinoApp.WriteLine("Double = {0}", dblOption.CurrentValue);
                    RhinoApp.WriteLine("Boolean = {0}", boolOption.CurrentValue);
                    RhinoApp.WriteLine("List = {0}", listValues[listIndex]);

                }
                else if(res == Rhino.Input.GetResult.Option)
                {
                    if (gc.OptionIndex() == optList)
                        listIndex = gc.Option().CurrentListOptionIndex; //this updates the list option to whatever index the user has picked

                    continue;
                }

                break;
                    
            }

            return Result.Success;
        }
    }
}