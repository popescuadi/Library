using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServerETLibrary
{
    public class SynchronousSocketListener
    {
        public Form1 forma;
        public int threadId;
        public static string data = null;
        public async Task StartListening()
        {
            byte[] bytes = new Byte[1024];

            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(1000);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket handler = listener.Accept();
                    data = null;
                    await sender(handler, data);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }
        private async Task sender(Socket handler, string data)
        {
            byte[] bytes = new Byte[1024];
            string datarec = null;
            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                datarec += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (datarec.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }
            string xchg = CoreLayer.SwitchChoise(datarec);
            switch (xchg)
            {
                case "login":
                    if (CoreLayer.LoginTest(datarec))
                    {
                        data = "login";
                        CoreLayer.LogView(forma, datarec, data);
                    }
                    else
                    { 
                        data = "eroare";
                       CoreLayer.LogView(forma, datarec, data);
                    }
                    break;
                case "users":
                    using (var datafunctionality = new DataLayer())
                    {
                        data = datafunctionality.GetUsers();
                        CoreLayer.LogView(forma, datarec, "success");
                    }
                        break;
                case "books":
                    using (var datafunctionality = new DataLayer())
                    {
                        data = datafunctionality.GetBooks();
                        CoreLayer.LogView(forma, datarec, "success");
                    }
                    break;
                case "user":
                   data= CoreLayer.GetLoans(datarec);
                        CoreLayer.LogView(forma, datarec, "success");
                    break;
                case "return":
                    CoreLayer.LogView(forma, datarec, CoreLayer.DeleteImprumut(datarec).ToString()) ;
                    data = "success";
                    break;
                case "loan":
                    data = CoreLayer.AddLoan(datarec);
                    break;
                case "aspuser":
                    data = CoreLayer.GetLoansASP(datarec);
                    CoreLayer.LogView(forma, "web client login", data);
                    break;
                case "aspreg":
                    if (CoreLayer.AddASPClient(datarec))
                    {
                        data = "true";
                        CoreLayer.LogView(forma, "web client registration", data);
                    }
                    else
                    {
                        data = "false";
                        CoreLayer.LogView(forma, "web client registration", data);
                    }
                 
                    break;
                default:
                    data = "eroare";
                    break;
            }
            byte[] msg = Encoding.ASCII.GetBytes(data);

            handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

    }
}
