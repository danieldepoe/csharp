using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Collections;
using Rhino.DocObjects;
using RhinoWindows;

namespace pacsharp1.Commands
{
    public class XPanelGenForm : Command

    {
        private Forms.MTCD Panelform { get; set; } 

        static XPanelGenForm _instance;
        public XPanelGenForm()
        {
            _instance = this;
        }

        ///<summary>The only instance of the XPanelGenForm command.</summary>
        public static XPanelGenForm Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "XPanelGenForm"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            if(null == Panelform)
            {
                Panelform = new Forms.MTCD();

                Panelform.FormClosed += OnFormClosed;

                Panelform.Show(RhinoWinApp.MainWindow);

                //var form_result = form.ShowDialog(RhinoWinApp.MainWindow);


            }

            
            //Forms is the nested namespace inside the pacsharp1 namespace
            //you can instantiate a class by immediately initializing some of it's fields
            // as done above with the {} brackets. This is the object initialize syntax.
            // you can set a property without actually invoking the constructor 

            /*
            if(form_result == DialogResult.Cancel)
            {
                RhinoApp.WriteLine("The dialog closed with cancel or x button");
                return Result.Failure;
            }
            else if (form_result == DialogResult.OK)
            {
                RhinoApp.WriteLine("The dialog closed with OK button");
                return Result.Success;
            }
            */

            return Result.Success;
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            Panelform.Dispose();
            Panelform = null;
        }
    }


    
}