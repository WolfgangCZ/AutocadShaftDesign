using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.GraphicsInterface;

namespace AutocadShaftDesign
{
    public class LayerManager
    {
        public string[] createdLayers;
        public void CreatePlainLayer(string name)
        {
            TransactionManager transactionManager = new TransactionManager();
            LayerTableRecord layerTableRecord = new LayerTableRecord();
            layerTableRecord.Name = name;
            createdLayers.Append(name);

            transactionManager.CommitLayer(ref layerTableRecord);
        }
        public void UpdateLayerThickness(string name, int thickness)
        {

        }
        public void UpdateLayerColor(string name, short colorID)
        {
            TransactionManager transactionManager = new TransactionManager();
            foreach (ObjectId layerId in transactionManager.layerTable)
            {
                LayerTableRecord layerTableRecord = transactionManager.trans.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord;

                if (layerTableRecord.Name == name)
                {
                    layerTableRecord.UpgradeOpen();
                    layerTableRecord.Color = Color.FromColorIndex(ColorMethod.ByLayer, colorID);
                    transactionManager.doc.Editor.WriteMessage("Layer updated!");
                    transactionManager.trans.Commit();
                }
            }
        }

        Line
        //not functional, probably create linetype manager? would it be better? 
        public void UpdateLayerLinetype(string name, string linetype)
        {
            TransactionManager transactionManager = new TransactionManager();

            foreach (ObjectId layerId in transactionManager.layerTable)
            {
                LayerTableRecord layerTableRecord = transactionManager.trans.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord;
                //opravit dle kodu dole - přidat foreach
                LinetypeTableRecord linetypeTableRecord = transactionManager.trans.GetObject(lineId, OpenMode.ForRead) as LinetypeTableRecord;

                if (layerTableRecord.Name == name)
                {
                    if (transactionManager.linetypeTable.Has(linetype))
                    {
                        layerTableRecord.LinetypeObjectId = transactionManager.layerTable[linetype];
                        transactionManager.trans.Commit();
                        transactionManager.doc.Editor.WriteMessage("\nLayer linetype changed");
                        break;
                    }

                    else
                    {
                        transactionManager.doc.Editor.WriteMessage("\nNo such linetype");
                        transactionManager.trans.Abort();
                    }

                }
            }

        }
        public void ListAllLineTypes()
        {
            TransactionManager transactionManager = new TransactionManager();
            foreach (ObjectId lineId in transactionManager.linetypeTable)
            {
                LinetypeTableRecord linetypeTableRecord = transactionManager.trans.GetObject(lineId, OpenMode.ForRead) as LinetypeTableRecord;
                transactionManager.doc.Editor.WriteMessage("\nLinetype name: " + linetypeTableRecord.Name);
            }
            transactionManager.trans.Commit();
        }
        public void AddLinetypeFromFile(string filename, string linetypeName)
        {
            TransactionManager transactionManager = new TransactionManager();
            if(transactionManager.linetypeTable.Has(linetypeName))
            {
                transactionManager.doc.Editor.WriteMessage("Linetype already exist");
                transactionManager.trans.Abort();
            }
            else
            {
                //Load the linetype
                transactionManager.db.LoadLineTypeFile(linetypeName, filename);
                transactionManager.doc.Editor.WriteMessage("Linetype [ " + linetypeName + "] was created succesfully");
                transactionManager.trans.Commit();
            }
        }
    }


}


