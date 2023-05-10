using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadShaftDesign
{
    internal class JackleManager
    {
        public void DrawJackleTop(double x, double y, double width, double lenght, int isHorizontal)
        {
            DrawLineManager drawLineManager = new DrawLineManager();
            if(isHorizontal > 0)
            {
                (lenght, width) = (width, lenght);
            }
            drawLineManager.Rectangle2D(x, y, width, lenght);
        }
    }
}
