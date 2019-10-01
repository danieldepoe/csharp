using System;
using Rhino;
using Rhino.Commands;
using Rhino.Collections;


/* many types of annotations in Rhino, with their own ways of selecting them. 
 * 
 * geometry types : points, curves, breps, meshes
 * 
 * POINT:  structures like Point3d, Point2d and Point3f
 *      https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Geometry_Point3d.htm
 *      
 * Rhino collections namespace has a method to store lists of points     
 *      https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Collections_Point3dList.htm
 * 
 * CURVE:  free form with control points, knots, etc.  
 * 
 * Base class is Curve, with other curves such as LineCurve, ArcCurve, PolylineCurve, NurbsCurve
 * 
 * line curve (nurbs representation):  
 * https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Geometry_LineCurve.htm
 * 
 * line structure (only 2 points):  
 * https://developer.rhino3d.com/api/RhinoCommon/html/T_Rhino_Geometry_Line.htm
 * 
 * BREP: Classes include surface,
 * 
 * MESH VS. NURBS
 * 
 * https://www.youtube.com/watch?v=50TeShIuPQU
 * 
 * 
 * 
 * 
 * CONSTRUCTORS = THE WAYS THAT YOU CAN MAKE THE THING
 * PROPERTIES = WHAT IS HAS ONCE YOU HAVE MADE IT
 * METHODS = THINGS YOU CAN DO TO IT 
 * 
 * 
 * GARBAGE COLLECTION:
 * 
 * when you program in .NET the memory management is not the responsibility of the programmer,  in C++ it's different.  
 * it keeps track of the unused objects
 * 
 * i disposable
 * 
 * ObjRef class has a garbage collector, the dispose method
 * 
 * You can create your own destructor as well.  
 *  
 * 
 * */


