using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //modify the port if necessary
            int port = 5646;
            byte[] buffer = new byte[256];
            string msg = null;
            string msg1 = null;
            string msgreply = null;
            int i;
            int contador = 0;
            string[] ubicacion = new string[5];
            bool pasar = false;
            TcpListener tcp = new TcpListener(IPAddress.Any, port);
            tcp.Start();
            do
            {
                try
                {
                    TcpClient datos = tcp.AcceptTcpClient();
                    NetworkStream stream = datos.GetStream();
                    i = stream.Read(buffer, 0, buffer.Length);
                    while (i != 0)
                    {
                        try
                        {
                            msg = Encoding.ASCII.GetString(buffer, 0, i);

                            if (!String.IsNullOrEmpty(msg))
                            {
                                if (msg.Substring(0, 2) == "##")
                                {
                                    msgreply = msg.Replace("##", "**").Replace("A", "B");
                                    buffer = Encoding.ASCII.GetBytes(msgreply);
                                    stream.Write(buffer, 0, buffer.Length);
                                }

                                i = stream.Read(buffer, 0, buffer.Length);
                                msg1 = Encoding.ASCII.GetString(buffer, 0, i);
                                ubicacion[contador] = msg1;
                                ++contador;
                                if (msg1.Contains(";"))
                                {
                                    for (int u = 0; u < ubicacion.Length; u++)
                                    {
                                        if (!String.IsNullOrEmpty(ubicacion[u]))
                                        {
                                            Console.Write(ubicacion[u]);
                                        }
                                    }
                                    Console.Write("\n");
                                    contador = 0;
                                }
                                msg1 = null;
                                msg = null;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    datos.Close();
                }
                catch
                {
                    Console.WriteLine("ERROR");
                }
            } while (pasar == false);
        }
    }
}