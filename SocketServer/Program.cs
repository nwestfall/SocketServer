using System;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketServer server = new SocketServer();

            Console.WriteLine("Welcome to the Socket Server Tester");
            Console.WriteLine();

            while(!server.VerifyIP())
            {
                Console.Write("Enter Socket IP (without port): ");
                server.IP = Console.ReadLine();
            }

            while (server.Port == 0)
            {
                Console.Write("Enter Socket Port: ");
                var portStr = Console.ReadLine();
                int portNum;
                if (int.TryParse(portStr, out portNum))
                    server.Port = portNum;
            }

            Console.Write("Ready to start? (Y/N) ");
            var key = Console.ReadLine();
            if(key.StartsWith("y", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine("Starting Socket Server!  Stop sending data by clicking any key in the console");

                server.Start();

                Console.ReadKey();
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            return; //QUIT
        }
    }
}