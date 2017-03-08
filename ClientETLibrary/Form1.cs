using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Drawing.Drawing2D;

namespace ClientETLibrary
{
    
    public partial class Form1 : Form
    {
        SynchronousSocketClient ClientSoket;
        string ip = "92.168.1.190";
        string SelectedUser="";
        string SelectedBook="";
        private GifImage gifImage = null;
        private string filePath = @"C:\Users\Adrian\Documents\Visual Studio 2015\Projects\ClientETLibrary\ClientETLibrary\croplog.gif";
        private System.Windows.Forms.Timer time;
        public Form1()
        {
            InitializeComponent();
            filePath = Utilities.GetPath();
            filePath += "\\croplog.gif";
            //textBox2.Text = "Parola";
            //this.BackgroundImage.Size.Width = this.Width;
            this.BackgroundImageLayout = ImageLayout.Stretch;
                this.SetStyle(ControlStyles.AllPaintingInWmPaint |ControlStyles.DoubleBuffer, true);
            label1.BackColor = System.Drawing.Color.Transparent;
            label2.BackColor = System.Drawing.Color.Transparent;
            ClientSoket = new SynchronousSocketClient();
            listView1.Visible = false;
            listView2.Visible = false;
           // listView1.
            button2.Visible = false;
            button3.Visible = false;
            ColumnHeader h1, h2, h3;
            h1 = new ColumnHeader();
            h2 = new ColumnHeader();
            h3 = new ColumnHeader();
            h1.Text = "Index";
            h2.Text = "Nume";
            h3.Text = "CNP";
            listView1.Columns.AddRange(new ColumnHeader[] { h1, h2, h3 });
            listView1.View = View.Details;
            ColumnHeader s1, s2, s3;
            s1 = new ColumnHeader();
            s2 = new ColumnHeader();
            s3 = new ColumnHeader();
           
            s1.Text = "IBAN";
            s2.Text = "Autor";
            s3.Text = "Nume";
            listView2.Columns.AddRange(new ColumnHeader[] { s1, s2, s3 });
            listView2.View = View.Details;
            textBox2.PasswordChar = '*';
            ip = GetIP4Address();
            time = new System.Windows.Forms.Timer();
            time.Tick += (sender, e) =>
              {
                  Image a = gifImage.GetNextFrame();
                  Image b = resizeImage(a, 50, 50);
                  button1.Image = b;
                  //button1.Image.Width = 20;
              };
            gifImage = new GifImage(filePath);
            gifImage.ReverseAtEnd = false;
            time.Enabled = true;



            //test

           // myButtonObject myButton = new myButtonObject();
           // //EventHandler myHandler = new EventHandler(myButton_Click);
           //// myButton.Click += myHandler;
           // myButton.Location = new System.Drawing.Point(20, 20);
           // myButton.Size = new System.Drawing.Size(101, 101);
           // this.Controls.Add(myButton);
           // myButton.Visible = true;
           // myButton.BackColor = System.Drawing.Color.Transparent;
        }


        private static Image resizeImage(Image image, int new_height, int new_width)
        {
            Bitmap new_image = new Bitmap(new_width, new_height);
            Graphics g = Graphics.FromImage((Image)new_image);
            g.InterpolationMode = InterpolationMode.High;
            g.DrawImage(image, 0, 0, new_width, new_height);
            return new_image;
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!=""&&textBox2.Text!="")
            {
                string send_msg;
                send_msg = "login ";
                send_msg += ip + " " + textBox1.Text + " " + textBox2.Text + " ";
                send_msg = await ClientSoket.SendData(send_msg);
                if (send_msg == "login")
                {
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    button1.Visible = false;
                    listView1.Visible = true;
                    listView2.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true;
                    //MessageBox.Show("Logare efectuata cu succes!");
                    try
                    {
                        bool x = true;
                        LoginDenial log = new LoginDenial(x);
                        log.ShowDialog();
                    }
                    catch (Exception ex)
                    {

                    }
                    
                    FillListView1( await ClientSoket.SendData("users " + ip + " "));
                    FillListView2(await ClientSoket.SendData("books " + ip + " "));
                }
                else
                {
                    try
                    {
                        bool x = false;
                        LoginDenial log = new LoginDenial(x);
                        log.ShowDialog();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
                MessageBox.Show("Va rog sa introduceti credentialele!");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                SelectedUser = item.SubItems[1].Text;
               // SelectedUser += " " + item.SubItems[1].Text;
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                ListViewItem item = listView2.SelectedItems[0];
                SelectedBook = item.SubItems[2].Text;
            }
        }
       
        
        private void FillListView1(string data)
        {
            int limit = Utilities.GetVector(data).Length;
            int poz = 0;
            string st1, st2, st3;
            while (poz<limit-4)
            {
                Utilities.GetThreeStrings(data,ref poz,out st1,out st2,out st3);
                ListViewItem item = new ListViewItem(st1);
                item.SubItems.Add(st2);
                item.SubItems.Add(st3);
                listView1.Items.Add(item);
            }
        }
        private void FillListView2(string data)
        {
            int limit = Utilities.GetVector(data).Length;
            int poz = 0;
            string st1, st2, st3;
            while (poz < limit - 4)
            {
                Utilities.GetThreeStrings(data, ref poz, out st1, out st2, out st3);
                ListViewItem item = new ListViewItem(st1);
                item.SubItems.Add(st2);
                item.SubItems.Add(st3);
                listView2.Items.Add(item);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (SelectedUser != "")
            {
                string data = await ClientSoket.SendData("user " + ip + " "+SelectedUser+ " ");
                user form = new user(SelectedUser, data,ip);
                form.ShowDialog();
            }
            else
                MessageBox.Show("Nu a fost selectat nici un user!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (SelectedBook != "" && SelectedUser != "")
            {
                loan form = new loan(SelectedBook, SelectedUser, ip);
                form.ShowDialog();
            }
            else
                MessageBox.Show("carte sau user neselectat");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
