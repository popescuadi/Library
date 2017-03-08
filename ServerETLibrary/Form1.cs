using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ServerETLibrary
{
    public partial class Form1 : Form
    {
        SynchronousSocketListener Soket;
        public Form1()
        {
            InitializeComponent();
            
            this.FormClosing += new FormClosingEventHandler(this.SaveOnClose);
            Soket = new SynchronousSocketListener();
            Soket.forma = this;
            Thread current = Thread.CurrentThread;
            Soket.threadId = current.ManagedThreadId;
            ColumnHeader h1 = new ColumnHeader();
            ColumnHeader h2 = new ColumnHeader();
            ColumnHeader h3 = new ColumnHeader();
            h1.Text = "Ip";
            h2.Text = "Operatie";
            h3.Text = "Rezultat";
            this.listView1.Columns.AddRange(new ColumnHeader[] { h1, h2, h3 });
            listView1.View = View.Details;
            StartListener();
            textBox1.PasswordChar = '*';
        }
        private async Task StartListener()
        {
            await Task.Run(() =>
            { Soket.StartListening(); });
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!=""&&textBox2.Text!="")
            {
                CoreLayer.AddAngajat(textBox2.Text, textBox1.Text);
                textBox1.Clear();
                textBox2.Clear();
            }
            else
            {
                MessageBox.Show("Va rog sa introduceti date corecte!");
            }
        }
        private void SaveOnClose (object sender, FormClosingEventArgs e)
        {
            int i = 0;
          
            DateTime localt = DateTime.Now;
            string[] data = new string[listView1.Items.Count + 1];
            data[i] = localt.ToString();
            i++;
            foreach (ListViewItem item in listView1.Items)
            {
                StringBuilder str = new StringBuilder();
                str.Append(item.SubItems[0].Text.ToString());
                str.Append(" ");
                str.Append(item.SubItems[1].Text.ToString());
                str.Append(" ");
                str.Append(item.SubItems[2].Text.ToString());
                data[i] = str.ToString();
                i++;
            }
            try
            {
                System.IO.File.WriteAllLines(@"C:\Users\Adrian\Documents\Visual Studio 2015\Projects\ServerETLibrary\ServerETLibrary\log.txt", data);
            }
            catch (Exception ee)
            {

            }
        }
    }
}
