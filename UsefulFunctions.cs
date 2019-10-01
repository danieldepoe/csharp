using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Geometry;
using Rhino.Commands;
using Rhino.Collections;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;



namespace pacsharp1
{
    class UsefulFunctions
    {
        public static Point3d XDrawPoint(Line line, bool start)
            //access modifier = public
            //stored only once in the memory = static
            //what does the function return?  in this case, a Point3d
        {
            Point3d resultpt = new Point3d();

            if (start)
                resultpt = line.From;
                // draws point at start
            else
                resultpt = line.To;
                //  draws point at end

            return resultpt;

        }

        /// <summary>
        /// This method takes a point array and draws points. 
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="doc"></param>

        public static void XDrawPoints(Point3d[] points, RhinoDoc doc)
        {
            for (var i = 0; i < points.Count(); i++)
                doc.Objects.AddPoint(points[i]);

            doc.Views.Redraw();
        }

        /// <summary>
        /// This method draw tangents at specified curve points.         
        /// </summary>
        /// <param name="points"></param>
        /// <param name="doc"></param>

        public static void XDrawCurveTangents(double[] parameters, Curve curve, RhinoDoc doc)
        {
            for (var i = 0; i < parameters.Count(); i++)
            {
                Point3d point = curve.PointAt(parameters[i]);
                Vector3d tangent = curve.TangentAt(parameters[i]);

                Line tangentline = new Line(point, point + (tangent * 10));

                doc.Objects.AddLine(tangentline);
                //draws lines at points, tangent to the selected curve and 10 units long
            }

            doc.Views.Redraw();
        }


        /// <summary>
        /// This function selects points
        /// </summary>
        /// <param name="message"></param>
        /// <param name="pointer"></param>
        public static void XSelectPoints(string message, out Point3dList pointer)
        {
            // variables are:
            // string message
            // out Point3dList points - the out won't be recognized unless you have Rhino.Collections and the 'out parameters
            // have to be filled in the function.  
            // don't need a doc because we won't write anything to the document table, only reading.


            pointer = new Point3dList();

            var go = new GetObject();

            go.GeometryFilter = Rhino.DocObjects.ObjectType.Point;
            go.SetCommandPrompt(message);
            go.GetMultiple(1, 0);

            if (go.CommandResult() != Result.Success)
                return;

            for (var i = 0; i < go.ObjectCount; i++)
            {
                var point = go.Object(i).Point();
                if (null != point)
                    pointer.Add(point.Location);
            }


        }

        /// <summary>
        /// This function creates lines from points
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="doc"></param>
        /// 
        /// 
        public static void XLinesFromPoints(Point3dList pointer, RhinoDoc doc)
        {
            for(var i = 0; i<pointer.Count-1; i++)
            {
                Line line = new Line(pointer[i], pointer[i + 1]);

                if (line != null && line.IsValid)
                    doc.Objects.AddLine(line); 
            }

            doc.Views.Redraw();

            
        }


        /// <summary>
        /// This function creates curves between points
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="doc"></param>
        /// <param name="through"></param>
        public static void XCurveFromPoints(Point3dList pointer, RhinoDoc doc, bool through)
        {

            if (through)
            {
                //create a curve through points
                Curve curve = Curve.CreateInterpolatedCurve(pointer, 3);

                if (curve != null && curve.IsValid)
                {
                    doc.Objects.AddCurve(curve);
                    doc.Views.Redraw();
                }

            }

            else
            {
                //use points as control points
                NurbsCurve curve = NurbsCurve.Create(false, 3, pointer);

                if (curve != null && curve.IsValid)
                {
                    doc.Objects.AddCurve(curve);
                    doc.Views.Redraw();
                }

            }

        }

        /// <summary>
        /// This function creates polyline from points
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="doc"></param>
        /// 
        /// 
        public static void XPolylineFromPoints(Point3dList pointer, RhinoDoc doc)
        {
            Polyline pline = new Polyline(pointer);

            pline.Add(pline[0]);   //this will close the polyline

            if (pline != null && pline.IsValid)
                doc.Objects.AddPolyline(pline);

            doc.Views.Redraw();


        }


        /// <summary>
        /// This function creates a single circle
        /// </summary>
        /// <param name="center"></param>
        /// <param name="doc"></param>
        /// 
        /// 
        public static void XDrawCircle(Point3d center, double radius, RhinoDoc doc)
        {
            Circle circle = new Circle(center, radius);

            //  we cannot write
            //  if (circle != null && pline.IsValid)
            //     doc.Objects.AddPolyline(pline);
            //  Because the circle is a structure and not a class.  
            //  https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Geometry_Circle.htm
            //  
            //      Therefore, we need to write

            if (circle.IsValid)
                doc.Objects.AddCircle(circle);

            //doc.Views.Redraw();


        }


        /// <summary>
        /// This function creates arc from points
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="doc"></param>
        /// 
        /// 
        public static void XDrawArc(Point3d pt0, Point3d pt1, Point3d pt2, RhinoDoc doc)
        {
            Arc arc = new Arc(pt0,pt1,pt2);                 

            if (arc.IsValid)
                doc.Objects.AddArc(arc);

            doc.Views.Redraw();

        }


        /// <summary>
        /// This function creates a brep from planar curves
        /// </summary>
        /// 
        /// 
        public static Brep[] XCreateBrepFromPlanarCurves(CurveList curves, RhinoDoc doc)
        {
            return Brep.CreatePlanarBreps(curves, doc.ModelAbsoluteTolerance);
            
            //it's a bit odd for us to create our own method XCreateBrepFromPlanarCurves that 
            //simply calls the RhinoCommon CreatePlanarBreps method, but for the purpose of the
            //exercise, we'll let it go.  
        }


        /// <summary>
        /// This function creates a brep from edge curves
        /// </summary>
        /// 
        /// 
        public static Brep XCreateBrepFromEdgeCurves(CurveList curves)
        {
            return Brep.CreateEdgeSurface(curves);
        }


        /// <summary>
        /// This function selects some curves
        /// </summary>
        /// 
        /// 
        public static bool XSelectCurves(string message, out CurveList curves)
        {
            curves = new CurveList();

            var gc = new GetObject();

            gc.SetCommandPrompt(message);
            gc.GeometryFilter = Rhino.DocObjects.ObjectType.Curve;
            gc.GetMultiple(1, 0);

            if (gc.CommandResult() != Result.Success)
                return false;

            for(var i = 0;i<gc.ObjectCount; i++)
            {
                var curve = gc.Object(i).Curve();
                if (null != curve)
                    curves.Add(curve);
            }

            return true;
        }

        /// <summary>
        /// This function selects breps
        /// </summary>
        /// 
        /// 
        public static bool XSelectBreps(string message, out RhinoList<Brep> breps)
        {
            breps = new RhinoList<Brep>();

            int i;

            var gc = new GetObject();

            
            gc.SetCommandPrompt("Select some Breps");
            gc.EnablePreSelect(true, true);
            gc.GeometryFilter = Rhino.DocObjects.ObjectType.Brep;
            gc.GetMultiple(1, 0);

            //we do our double check to make sure the user selected something

            if (gc.CommandResult() != Result.Success)
                return false;

            
            for (i = 0; i < gc.ObjectCount; i++)
            {
                var brep = gc.Object(i).Brep();
                if (null != brep)
                    breps.Add(brep);                                
            }

            return true;
        }

        /// <summary>
        /// Set or create a new layer
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static Rhino.DocObjects.Layer SetLayer(RhinoDoc doc, string layername, System.Drawing.Color color)
        {
            //https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_DocObjects_Tables_LayerTable.htm

            if (string.IsNullOrEmpty(layername) || !Rhino.DocObjects.Layer.IsValidName(layername))
                return null;
            
            
            Rhino.DocObjects.Layer layer = doc.Layers.FindName(layername);

            if (layer != null)
            {
                //the layer already exists
                if (layer.Index >= 0)
                    doc.Layers.SetCurrentLayerIndex(layer.Index, false);
            }
            else
            {
                layer = new Rhino.DocObjects.Layer();

                layer.Name = layername;
                layer.Color = color;

                //we have to create a new layer
                int layer_index = doc.Layers.Add(layer);
                if (layer_index >= 0)
                    doc.Layers.SetCurrentLayerIndex(layer_index, false);
            }


            return doc.Layers[layer.Index];
        }

        /// <summary>
        /// Set or create a new layer - with a parent
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static Rhino.DocObjects.Layer SetLayer(RhinoDoc doc, string layername, System.Drawing.Color color, Rhino.DocObjects.Layer parentlayer)

            //this is an overloaded function!! it has the saame name as above 'SetLayer' but with different parameters.  
        {
            //https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_DocObjects_Tables_LayerTable.htm

            if (string.IsNullOrEmpty(layername) || !Rhino.DocObjects.Layer.IsValidName(layername))
                return null;


            Rhino.DocObjects.Layer layer = doc.Layers.FindName(layername);

            if (layer != null)
            {
                //the layer already exists
                if (layer.Index >= 0)
                    doc.Layers.SetCurrentLayerIndex(layer.Index, false);
            }
            else
            {
                layer = new Rhino.DocObjects.Layer();

                if (parentlayer.Id != null)
                    layer.ParentLayerId = parentlayer.Id;

                layer.Name = layername;
                layer.Color = color;

                //we have to create a new layer
                int layer_index = doc.Layers.Add(layer);
                if (layer_index >= 0)
                    doc.Layers.SetCurrentLayerIndex(layer_index, false);
            }

                       
            return doc.Layers[layer.Index];
        }
    }
}
