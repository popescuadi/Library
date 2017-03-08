using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;


namespace ServerETLibrary
{
    static class  CoreLayer
    {
      public static  string SwitchChoise (string data)
        {
            string buffer = data;
            string[] vec = buffer.Split(new char[0]);
            return vec[0];
        }
     public static  string [] GetVector(string data)
        {
            return data.Split(new char[0]);
        }
        public static bool LoginTest(string data)
        {
            string[] msg = GetVector(data);
            bool return_value=false;
            try
            {
                var engine = new DataLayer();
                string usr = msg[2] + " " + msg[3];
                return_value = engine.Authentificate(usr, msg[4]);
            }
            catch (Exception ex)
            {
                return_value = false;
            }
            return return_value;
        }
        private static void SetText(Form1 cs,string data,string result)
        {
            string[] msg = GetVector(data);
            ListViewItem item = new ListViewItem(msg[1]);
            item.SubItems.Add("Operatie de : " + msg[0]);
            item.SubItems.Add(result);
            cs.listView1.Items.Add(item);
        }
        public static void LogView(Form1 cs,string data,string result)
        {
            if (!cs.InvokeRequired)
                  {
                       SetText(cs, data, result);
                   }
                //SetText(cs, data, result);
            else
            {
                cs.BeginInvoke((MethodInvoker)delegate () {
                    string[] msg = GetVector(data);
                    ListViewItem item = new ListViewItem(msg[1]);
                    item.SubItems.Add("Operatie de : " + msg[0]);
                    item.SubItems.Add(result);
                    cs.listView1.Items.Add(item);
                });
            }
        }
        public static string GetLoans(string data)
        {
            string retval = "";
            using (var datafunctionality = new DataLayer())
            {
                string[] msg = GetVector(data);
                int i = 2;
                string buffer = "";
                while (msg[i]!="<EOF>")
                {
                    buffer +=msg[i];
                    i++;
                }
                retval = datafunctionality.GetLoans(datafunctionality.GetUserIndex(buffer));
            }
            return retval;
        }
        public static string GetLoansASP(string data)
        {
            string retval = "";
            using (var datafunctionality = new DataLayer())
            {
                string[] msg = GetVector(data);
                int i = 1;
                string buffer = "";
                /*while (msg[i] != "<EOF>")
                {
                    buffer += msg[i];
                    i++;
                }*/
                if (AuthentificateAspUser(msg[1], msg[2]) == true)
                    retval = datafunctionality.GetLoans(datafunctionality.GetIndexByEmail(msg[1]));
                else
                    retval = "false";
            }
            return retval;
        }
        public static bool AddASPClient(string data)
        {
            using (var datafunctionality = new DataLayer())
            {
                string[] msg = GetVector(data);
                if (msg.Length >= 5)
                {
                    if (msg[1] != "" && msg[1] != "" && msg[3] != "" && msg[4] != "")
                    {
                        if (msg[4].Length == 10)
                        {
                            string[] mg=new string[4];
                            for (int i = 0; i < 4; i++)
                                mg[i] = msg[i + 1];
                            datafunctionality.RegisterUser(mg);
                            return true;
                                }

                }
                }
            }
            return false;
        }
        public static bool DeleteImprumut (string data)
        {
            using (var datafunctionality = new DataLayer())
            {
                string[] msg = GetVector(data);
                string buffer="", index="";
                int i = 2;
                while (msg[i]!="|")
                {
                    buffer += msg[i];
                    i++;
                }
                i++;
                while (msg[i]!="<EOF>")
                {
                    index += msg[i];
                    if (msg[i + 1] != "<EOF>")
                        index += " ";
                    i++;
                }
                string id;
                id = datafunctionality.GetUserIndex(buffer);
                index = datafunctionality.GetBook(index);
                datafunctionality.DeleteImprumut(id, index);
            }
            return false;
        }
        public static string AddLoan(string data)
        {
            string user="";
            string book = "";
            int i = 2;
            string[] msg = GetVector(data);
            while (msg[i]!="|")
            {
                user += msg[i];
                i++;
            }
            i++;
            while (msg[i] != "<EOF>")
            {
                book += msg[i];
                if (msg[i + 1] != "<EOF>")
                    book += " ";
                i++;
            }
            using (var datafunctionality = new DataLayer())
            {
                user = datafunctionality.GetUserIndex(user);
                book = datafunctionality.GetBook(book);
                if (datafunctionality.AddLoan(user, book))
                    return "succes";
            }
                return "false";
        }
        public static bool AuthentificateAspUser(string user,string pass)
        {
            using (var datafunctionality = new DataLayer())
            {
                return datafunctionality.AuthentificateAspUser(user, pass);
            }
            return false;
        }
        public static void AddAngajat(string data, string pass)
        {
            using (var datafunctionality = new DataLayer())
            {
                string[] msg = GetVector(data);
                string nume = msg[0];
                string prenume = msg[1];
                datafunctionality.AddAngajat(nume, prenume, pass);
            }
        }
    }
   
}
