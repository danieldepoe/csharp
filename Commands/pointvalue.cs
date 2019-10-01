using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input.Custom;

namespace pacsharp1.Commands
{
    public class pointvalue : Command
    {
        static pointvalue _instance;
        public pointvalue()
        {
            _instance = this;
        }

        ///<summary>The only instance of the MyCommand2 command.</summary>
        public static pointvalue Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "pointvalue"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var gp = new GetPoint();

            string prompt;
            prompt = "select a point";

            gp.SetCommandPrompt(prompt);

            gp.Get();

            Result res = gp.CommandResult();

            if (res != Result.Success)
                return gp.CommandResult();

            var point = gp.Point();

            var format = string.Format("F{0}", doc.DistanceDisplayPrecision);
            var provider = System.Globalization.CultureInfo.InvariantCulture;

            var x = point.X.ToString(format);
            var y = point.Y.ToString(format);
            var z = point.Z.ToString(format);

            RhinoApp.Write ("World coordinates: {0},{1},{2}" + Environment.NewLine, x, y, z);

            double number = 0.56;

            RhinoApp.WriteLine(number.ToString("F5", provider));
            
           
            return Result.Success;
        }
    }
}