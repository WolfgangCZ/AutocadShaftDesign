using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

namespace AutocadShaftDesign
{
    public class TransactionManager
    {
        public Document doc;
        public Database db;
        public Transaction trans;
        public BlockTable bt;
        public BlockTableRecord btr;
        public LayerTable layerTable;
        public LinetypeTable linetypeTable;

        public TransactionManager()
        {
            try
            {
                doc = Application.DocumentManager.MdiActiveDocument;
                db = doc.Database;
                trans = db.TransactionManager.StartTransaction();

                //open object database
                bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                layerTable = trans.GetObject(db.LayerTableId, OpenMode.ForWrite) as LayerTable;
                linetypeTable = trans.GetObject(db.LinetypeTableId, OpenMode.ForWrite) as LinetypeTable;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void CommitLine(ref Line line)
        {
            btr.AppendEntity(line);
            trans.AddNewlyCreatedDBObject(line, true);
            trans.Commit();
        }
        public void CommitPolyline(ref Polyline polyLine)
        {
            btr.AppendEntity(polyLine);
            trans.AddNewlyCreatedDBObject(polyLine, true);
            trans.Commit();
        }
        public void CommitLayer(ref LayerTableRecord layerTableRecord)
        {
            if(layerTable.Has(layerTableRecord.Name)) 
            {
                doc.Editor.WriteMessage("Layer already exist");
                trans.Abort();
            }
            else
            {
                layerTable.UpgradeOpen();
                layerTable.Add(layerTableRecord);
                trans.AddNewlyCreatedDBObject(layerTableRecord, true);
                db.Clayer = layerTable[layerTableRecord.Name];
                doc.Editor.WriteMessage("Layer [" + layerTableRecord.Name + "] created succesfully");
                trans.Commit();
            }
        }
        /*
        public void AddLayer(string name, int thickness, short colorIndex, string lineType)
        {
            if(layerTable.Has(name)) 
            {
                doc.Editor.WriteMessage("layer already exist");
                trans.Abort();
            }

            //TODO dodělat linetype - nastavení linetype do hladiny
            else if(linetypeTable.Has(lineType))
            {
                layerTable.UpgradeOpen();
                LayerTableRecord layerTableRecord = new LayerTableRecord();
                layerTableRecord.Name = name;
                layerTableRecord.LineWeight = (LineWeight)thickness;
                layerTableRecord.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByLayer, colorIndex);

            }
            else
            {
                doc.Editor.WriteMessage("non existing line type!!!");
                trans.Abort();
            }
        }
        */

        //TODO přidat commit všechno ostatní co bude potřeba
        //přidat buffer pro polyline?

    }
}
