using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace ClientETLibrary
{
    public partial class LoginDenial : Form
    {
        private GifImage gifImage = null;
        private string filePath = @"C:\Users\Adrian\Documents\Visual Studio 2015\Projects\ClientETLibrary\ClientETLibrary\access.gif";
        private System.Windows.Forms.Timer time,closer;
        private int tacks = 0;
        public LoginDenial(bool check)
        {
            //string data = Utilities.GetPath();
            filePath = data + "\\acces.gif";
            if (check)
                filePath = data + "\\granted.gif";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            time = new System.Windows.Forms.Timer();
            Image a;
            pictureBox1 = new PictureBox();
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Size = new System.Drawing.Size(this.Width-10, this.Height-10);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(this.pictureBox1);
            pictureBox1.Visible = true;
             time.Tick += (sender, e) =>
             {
                 a = gifImage.GetNextFrame();
                 pictureBox1.Image = a;
             };
            gifImage = new GifImage(filePath);
            gifImage.ReverseAtEnd = false;
            time.Enabled = true;
            closer= new System.Windows.Forms.Timer();
            closer.Tick += (sender, e) =>
            {
                tacks++;
                if (tacks == 50)
                    this.Close();
            };
            closer.Enabled = true;
        }
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        private void Time_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoginDenial_Load(object sender, EventArgs e)
        {
            this.Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = gifImage.GetNextFrame();
        }
    }
}
