using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadShaftDesign
{
    public class LayerManager
    {
        public void CreatePlainLayer(string name)
        {
            TransactionManager transactionManager = new TransactionManager();  
            LayerTableRecord layerTableRecord = new LayerTableRecord();
            layerTableRecord.Name = name;
            
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
        //not functional
        public void UpdateLayerLinetype(string name, string linetype)
        {
            TransactionManager transactionManager = new TransactionManager();

            foreach (ObjectId layerId in transactionManager.layerTable)
            {
                LayerTableRecord layerTableRecord = transactionManager.trans.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord;

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
            foreach(ObjectId lineId in transactionManager.linetypeTable) 
            {
                LinetypeTableRecord linetypeTableRecord = transactionManager.trans.GetObject(lineId, OpenMode.ForRead) as LinetypeTableRecord;
                transactionManager.doc.Editor.WriteMessage("\nLinetype name: " + linetypeTableRecord.Name);
            }
            transactionManager.trans.Commit();
        }
    }


}


