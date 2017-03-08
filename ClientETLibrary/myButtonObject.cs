using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientETLibrary
{
    public class myButtonObject : UserControl
    { 
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Pen myPen = new Pen(Color.Black);
            graphics.DrawEllipse(myPen, 0, 0, 100, 100);
            myPen.Dispose();
        }
    }
}
