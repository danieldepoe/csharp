using System;
using Rhino;
using Rhino.Commands;

namespace pacsharp1.Commands
{
    public class AddLayer : Command
    {
        static AddLayer _instance;
        public AddLayer()
        {
            _instance = this;
        }

        ///<summary>The only instance of the AddLayer command.</summary>
        public static AddLayer Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "AddLayer"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // Cook up an unused layer name
            string unused_name = doc.Layers.GetUnusedLayerName(false);

            // Prompt the user to enter a layer name
            Rhino.Input.Custom.GetString gs = new Rhino.Input.Custom.GetString();
            gs.SetCommandPrompt("Name of layer to add");
            gs.SetDefaultString(unused_name);
            gs.AcceptNothing(true);
            gs.Get();
            if (gs.CommandResult() != Rhino.Commands.Result.Success)
                return gs.CommandResult();

            // Was a layer named entered?
            string layer_name = gs.StringResult().Trim();
            if (string.IsNullOrEmpty(layer_name))
            {
                Rhino.RhinoApp.WriteLine("Layer name cannot be blank.");
                return Rhino.Commands.Result.Cancel;
            }

            // Is the layer name valid?
            if (!Rhino.DocObjects.Layer.IsValidName(layer_name))
            {
                Rhino.RhinoApp.WriteLine(layer_name + " is not a valid layer name.");
                return Rhino.Commands.Result.Cancel;
            }

            // Does a layer with the same name already exist?
            int layer_index = doc.Layers.Find(layer_name, true);
            if (layer_index >= 0)
            {
                Rhino.RhinoApp.WriteLine("A layer with the name {0} already exists.", layer_name);
                return Rhino.Commands.Result.Cancel;
            }

            // Add a new layer to the document
            layer_index = doc.Layers.Add(layer_name, System.Drawing.Color.IndianRed);
            if (layer_index < 0)
            {
                Rhino.RhinoApp.WriteLine("Unable to add {0} layer.", layer_name);
                return Rhino.Commands.Result.Failure;
            }
            return Rhino.Commands.Result.Success;
            return Result.Success;
        }
    }
}