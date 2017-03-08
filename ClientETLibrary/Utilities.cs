using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientETLibrary
{
   static class Utilities
    {
        static public string[] GetVector(string data)
        {
            return data.Split(new char[0]);
        }
        static public string GetPath ()
        {
            string data = "";
            data= Directory.GetCurrentDirectory();
            return data;
        }
        static public void GetThreeStrings(string data, ref int poz, out string st1, out string st2, out string st3)
        {
            string[] vec = GetVector(data);
            string buffer = "";
            while (vec[poz] != "|")
            {
                buffer += vec[poz] + " ";
                poz++;
            }
            st1 = buffer;
            poz++;
            buffer = "";
            while (vec[poz] != "|")
            {
                buffer += vec[poz] + " ";
                poz++;
            }
            st2 = buffer;
            poz++;
            buffer = "";
            while (vec[poz] != "/")
            {
                buffer += vec[poz] + " ";
                poz++;
            }
            st3 = buffer;
            poz++;
        }
    }
}
