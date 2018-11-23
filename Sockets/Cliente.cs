using System;
using System.Net;
using System.Net.Sockets;

namespace Sockets
{
    class Cliente
    {
        public static void StartClient()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEp = new IPEndPoint(ipAddress, Consola.puerto);

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sender.Connect(remoteEp);
                Console.WriteLine("Conexion con: " + sender.RemoteEndPoint.ToString());
            }
            catch (Exception e )
            {
                if (e is SocketException || e is ArgumentNullException)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine("Error en la conexion:" + e);
            }
            Console.ReadLine();
        }
    }
}
