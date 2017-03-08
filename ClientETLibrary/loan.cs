using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientETLibrary
{
    public partial class loan : Form
    {
        string book = "";
        string user = "";
        string ip;
        public loan(string bo,string usr,string iP)
        {
            InitializeComponent();
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            label1.BackColor = System.Drawing.Color.Transparent;
            label2.BackColor = System.Drawing.Color.Transparent;
            book = bo;
            user = usr;
            label1.Text = usr;
            label2.Text = bo;
            ip = iP;
        }

        private void loan_Load(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SynchronousSocketClient sok = new SynchronousSocketClient();
                string receive = await sok.SendData("loan " + ip + " " + user + " | " + book + " ");
                MessageBox.Show(receive);
                this.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
