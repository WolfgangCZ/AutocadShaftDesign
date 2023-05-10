using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutocadShaftDesign
{
    public class UserInputOutput
    {
        private readonly Document doc;
        private readonly Editor edt;
        public UserInputOutput()
        {
            doc = Application.DocumentManager.MdiActiveDocument;
            edt = doc.Editor;
        }
        //send message to command line
        public void SendMessage(string message)
        {
            edt.WriteMessage("\n" + message);
        }
        //obtain string input from user while sending message
        public string GetInputString(string message) 
        {
            //sending instruction to user
            PromptStringOptions prompt = new PromptStringOptions(message);
            prompt.AllowSpaces = false;
            PromptResult result = edt.GetString(prompt);
            while(result.Status != PromptStatus.OK)
            {
                edt.WriteMessage("Incorrect input, try again");
                result = edt.GetString(prompt);
            }
            return result.StringResult;
        }
        //obtain double input from user while sending message
        public double GetInputDouble(string message) 
        {
            //sending instruction to user
            PromptStringOptions prompt = new PromptStringOptions(message);
            prompt.AllowSpaces = false;
            PromptResult result = edt.GetString(prompt);
            while (!((result.Status == PromptStatus.OK) && Double.TryParse(result.StringResult, out _))) //wtf is going on?
            {
                edt.WriteMessage("Incorrect input, try again:");
                result = edt.GetString(prompt);
            }
            return Double.Parse(result.StringResult);


        }
        //obtain int input from user while sending message (bool didnt work, couldnt convert string to bool idk why didnt want to investigate)
        public int GetInputInt(string message)
        {
            PromptStringOptions prompt = new PromptStringOptions(message);
            prompt.AllowSpaces = false;
            PromptResult result = edt.GetString(prompt);
            string value = result.StringResult;
            while (!((result.Status == PromptStatus.OK) && !Int32.TryParse(value, out _)))
            {
                edt.WriteMessage("Incorrect input, try again");
                result = edt.GetString(prompt);
                value = result.StringResult;
            }
            return Int32.Parse(value);
        }
    }
}
