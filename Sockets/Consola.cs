using System;

namespace Sockets
{
    class Consola
    {
        public static int puerto = 444;
        public const int BufferSize = 1024;

        private static void Main(string[] args)
        {
            var cmd = Console.ReadLine();
            while (cmd != "exit")
            {
                switch (cmd)
                {
                    case "cn clt":
                        Cliente.StartClient();
                        break;
                    case "cn srv":
                        Server.listen=true;
                        Server.allDone.Reset();
                        Server.StartListening();
                        break;
                    case "clr":
                        Console.Clear();
                        break;
                    default:
                        if (!string.IsNullOrEmpty(cmd))
                        {
                            Console.WriteLine("El comando " + cmd + " no fue encontrado");
                        }
                        break;
                }
                cmd = Console.ReadLine();
            }
        }
    }
}
