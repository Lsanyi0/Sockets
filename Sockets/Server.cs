using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Sockets
{
    public class StateObject
    {
        public Socket WorkSocket = null;
        public byte[] buffer = new byte[Consola.BufferSize];
        public StringBuilder sb = new StringBuilder();
    }

    //Socket Listener
    public class Server
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static bool listen;

        public Server()
        {
        }

        public static void StartListening()
        {
            IPAddress ipAddress = IPAddress.Parse("192.168.0.20");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Consola.puerto);

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Waiting for a connection on " + ipAddress +":" +Consola.puerto);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);
                Console.CancelKeyPress += new ConsoleCancelEventHandler(Exit);
                while (listen != false)
                {
                    allDone.Reset();

                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject();
            state.WorkSocket = handler;
            handler.BeginReceive(state.buffer, 0, Consola.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;

            StateObject state = (StateObject) ar.AsyncState;
            Socket handler = state.WorkSocket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));


                    Console.WriteLine("{0} {1}",bytesRead,state.sb);
                   // Send(handler,content);

            }

        }

        public static void Send(Socket handler, string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        public static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket) ar.AsyncState;

                int bytesSent = handler.EndSend(ar);

                Console.WriteLine("Sent {0}",bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected static void Exit(object sender, ConsoleCancelEventArgs args)
        {
            var asyncCallback = new AsyncCallback(SendCallback);
            Console.WriteLine("Listening stopped...");
            listen = false;
            allDone.Set();
        }
    }
}
