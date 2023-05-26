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
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;

namespace AutocadShaftDesign
{
    public class LayerManager
    {
        public string[] createdLayers = new string[0];
        public string[] createdLinetypes = new string[0];
        public void CreateAllBasicLayers()
        {
            CreatePlainLayer("011ACSD_THIN_SOLID");
            CreatePlainLayer("012ACSD_THIN_DASH");
            CreatePlainLayer("013ACSD_THIN_DASHDOT");
            CreatePlainLayer("014ACSD_THIN_DASHDOTDOT");
            CreatePlainLayer("021ACSD_THICK_SOLID");
            CreatePlainLayer("022ACSD_THICK_DASH");
            CreatePlainLayer("023ACSD_THICK_DASHDOT");
            CreatePlainLayer("024ACSD_THICK_DASHDOTDOT");
            CreatePlainLayer("031ACSD_DIM");
            CreatePlainLayer("031ACSD_TEXT");
            CreatePlainLayer("test");
        }
        public void CreateAllBasicLinetypes()
        {
            AddLinetypeFromFile("acad.lin", "ACAD_ISO02W100");
            AddLinetypeFromFile("acad.lin", "ACAD_ISO10W100");
            AddLinetypeFromFile("acad.lin", "ACAD_ISO12W100");
        }
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

        //TODO pokud linetype není načtená, načíst, pokud neexistuje, napsat, že neexistuje a nastavit na continuous
        public void UpdateLayerLinetype(string name, string linetype)
        {
            TransactionManager transactionManager = new TransactionManager();

            foreach (ObjectId layerId in transactionManager.layerTable)
            {
                LayerTableRecord layerTableRecord = transactionManager.trans.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                
                if (layerTableRecord.Name == name)
                {
                    if (transactionManager.linetypeTable.Has(linetype))
                    {
                        layerTableRecord.LinetypeObjectId = transactionManager.linetypeTable[linetype];
                        transactionManager.doc.Editor.WriteMessage("\nLayer linetype changed");
                        transactionManager.trans.Commit();
                        break;
                    }

                    else
                    {
                        transactionManager.doc.Editor.WriteMessage("\nlinetype doesnt exist or isnt loaded - linetype unchanged");
                        transactionManager.trans.Abort();
                        break;
                    }
                }
            }

        }
        public void ListAllLineTypes() //prostě jen pro vypsání hladin
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


