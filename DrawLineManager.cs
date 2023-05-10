using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;


namespace AutocadShaftDesign
{
    public class DrawLineManager
    {

        public void Line2D(double b_x, double b_y, double e_x, double e_y) //TODO add layer
        {
            Point3d pt_begin = new Point3d(b_x, b_y, 0);
            Point3d pt_end = new Point3d(e_x, e_y, 0);
            Line line = new Line(pt_begin, pt_end);

            TransactionManager transactionManager = new TransactionManager();
            transactionManager.CommitLine(ref line);
        }

        public void PolyLine2D(Tuple<double, double> [] points)
        {
            Polyline polyline = new Polyline();
            for (int i = 0; i < points.Length; i++) 
            {
                polyline.AddVertexAt(i, new Point2d(points[i].Item1, points[i].Item2), 0, 0, 0);
            }
            TransactionManager transactionManager = new TransactionManager();
            transactionManager.CommitPolyline(ref polyline);
        }

        public void Rectangle2D(double x, double y, double width, double height)
        {
            Tuple<double, double>[] polyline = { Tuple.Create(x, y), Tuple.Create(x + width, y), Tuple.Create(x + width, y + height), Tuple.Create(x, y + height), Tuple.Create(x, y) };
            PolyLine2D(polyline);
        }

        //public void RectangleWH(double x, double y, )


        /*
         bufferovat IDs nakreslených objektů pro pozdější vyhledání a případnou modifikaci do managera - TOTO UDĚLAT U VŠECH MANAGERŮ
         */
    }
}
