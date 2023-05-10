using Autodesk.AutoCAD.Internal;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadShaftDesign
{
    public class AssembleCommands
    {
        UserInputOutput userInputOutput = new UserInputOutput();
        DrawLineManager drawLineManager = new DrawLineManager();
        /*
        TransactionManager transactionManager = new TransactionManager();
        */

        [CommandMethod("drawline1")]
        public void Draw_Line_1()
        {
            userInputOutput.SendMessage("Drawing first line");
            drawLineManager.Line2D(0, 0, 50, 50);
            userInputOutput.SendMessage("Drawing a second Line");
            drawLineManager.Line2D(0, 0, 500, 500);
            userInputOutput.SendMessage("Drawing a third Line");
            drawLineManager.Line2D(0, 0, 100, 500);
        }
        [CommandMethod("drawpolyline1")]
        public void Draw_Polyline()
        {
            Tuple<double, double>[] polyline = { Tuple.Create(0.0, 0.0), Tuple.Create(0.0, 50.0), Tuple.Create(50.0, 50.0), Tuple.Create(50.0, 0.0), Tuple.Create(0.0, 0.0) };
            userInputOutput.SendMessage("drawing polyline");
            drawLineManager.PolyLine2D(polyline);
        }
        [CommandMethod("userinput1")]
        public void UserInput1()
        {
            double number = userInputOutput.GetInputDouble("Type a number:");
            userInputOutput.SendMessage("your number is " + number);
        }
        [CommandMethod("drawrectangle")]
        public void DrawPolyline() 
        {
            double x = userInputOutput.GetInputDouble("Type x position (left bottom corner):");
            double y = userInputOutput.GetInputDouble("Type y position (left bottom corner):");
            double width = userInputOutput.GetInputDouble("Type width:");
            double height = userInputOutput.GetInputDouble("Type height:");
            drawLineManager.Rectangle2D(x, y, width, height);  
        }
        [CommandMethod("drawjackle")]
        public void DrawJackleProfile()
        {
            JackleManager jackleManager = new JackleManager();
            double x = userInputOutput.GetInputDouble("Type x position (left bottom corner):");
            double y = userInputOutput.GetInputDouble("Type y position (left bottom corner):");
            double width = userInputOutput.GetInputDouble("Type width:");
            double lenght = userInputOutput.GetInputDouble("Type lenght:");
            int isHorizontal = userInputOutput.GetInputInt("Is the beam horizontal? (0=no, >1=yes):");
            jackleManager.DrawJackleTop(x, y, width, lenght, isHorizontal);
        }
        [CommandMethod("createlayer")]
        public void CreateLayer()
        {
            string name = userInputOutput.GetInputString("Type layer name: ");
            int color = userInputOutput.GetInputInt("Type color index: ");
            string linetype = userInputOutput.GetInputString("Type line type: ");
            short colorId = (short)color;
            LayerManager layerManager = new LayerManager();
            layerManager.CreatePlainLayer(name);
            layerManager.UpdateLayerColor(name, colorId);
            layerManager.UpdateLayerLinetype(name, linetype);
        }
        [CommandMethod("listlinetypes")]
        public void ListLinetypes()
        {
            LayerManager layerManager = new LayerManager();
            layerManager.ListAllLineTypes();
        }
            
        //TODO kontrola, jestli se tam zadávají správné parametry (string int atd)
        //TODO nutné vytvořit novou linetype, protože to není načtené řekl bych
        //TODO opravit protože to nefunguje i když ručně hladinu vytv ořím
    }
}
