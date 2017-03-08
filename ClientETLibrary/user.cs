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
    public partial class user : Form
    {
        string SelectedUser;
        string ip;
        string titlu;
        public user(string title,string data,string IP)
        {
            InitializeComponent();
            this.Text = title;
            titlu = title;
            
            FillListView1(data);
            ip = IP;
        }
        private void FillListView1(string data)
        {
            ColumnHeader h1, h2, h3;
            h1 = new ColumnHeader();
            h2 = new ColumnHeader();
            h3 = new ColumnHeader();
            h1.Text = "Autor";
            h2.Text = "Nume";
            h3.Text = "Data";
            listView1.Columns.AddRange(new ColumnHeader[] { h1, h2, h3 });
            listView1.View = View.Details;
            int limit = Utilities.GetVector(data).Length;
            int poz = 0;
            string st1, st2, st3;
            while (poz < limit - 4)
            {
                Utilities.GetThreeStrings(data, ref poz, out st1, out st2, out st3);
                ListViewItem item = new ListViewItem(st1);
                item.SubItems.Add(st2);
                item.SubItems.Add(st3);
                listView1.Items.Add(item);
            }
        }
        private void user_Load(object sender, EventArgs e)
        {

        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                SelectedUser = item.SubItems[0].Text;
                // SelectedUser += " " + item.SubItems[1].Text;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (SelectedUser != "")
            {
                SynchronousSocketClient ClientSoket = new SynchronousSocketClient();
                string data = await ClientSoket.SendData("return " + ip + " "+titlu+" | " + SelectedUser + " ");
                listView1.Clear();
                data = await ClientSoket.SendData("user " + ip + " " + titlu + " ");
                FillListView1(data);
            }
            else
                MessageBox.Show("Nu a fost selectat nici un user!");
        }
    }
}
