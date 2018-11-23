using System;

namespace Sockets
{
    class Consola
    {
        public static int puerto = 444;
        public const int BufferSize = 1024;
        static void Main(string[] args)
        {
            var cmd = Console.ReadLine();
            while (cmd != "exit")
            {
                switch (cmd)
                {
                    case "cn":
                        Cliente.StartClient();
                        break;
                    case "clr":
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("El comando " + cmd + " no es conocido");
                        break;
                }
                cmd = Console.ReadLine();
            }
        }
    }
}
