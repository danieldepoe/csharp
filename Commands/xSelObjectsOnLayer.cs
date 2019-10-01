using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Collections;

namespace pacsharp1.Commands
{
    public class xSelObjectsOnLayer : Command
    {
        static xSelObjectsOnLayer _instance;
        public xSelObjectsOnLayer()
        {
            _instance = this;
        }

        ///<summary>The only instance of the xSelObjectsOnLayer command.</summary>
        public static xSelObjectsOnLayer Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "xSelObjectsOnLayer"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            {
                
                    // Prompt for a layer name
                    string layername = doc.Layers.CurrentLayer.Name;
                    Result rc = Rhino.Input.RhinoGet.GetString("Name of layer to select objects", true, ref layername);
                    if (rc != Rhino.Commands.Result.Success)
                        return rc;

                    // Get all of the objects on the layer. If layername is bogus, you will
                    // just get an empty list back
                    Rhino.DocObjects.RhinoObject[] rhobjs = doc.Objects.FindByLayer(layername);
                    if (rhobjs == null || rhobjs.Length < 1)
                        return Rhino.Commands.Result.Cancel;

                    for (int i = 0; i < rhobjs.Length; i++)
                        rhobjs[i].Select(true);
                    doc.Views.Redraw();
                    return Rhino.Commands.Result.Success;
               
            }
            return Result.Success;
        }
    }
}

