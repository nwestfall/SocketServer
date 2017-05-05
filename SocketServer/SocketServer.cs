using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketServer
{
    public class SocketServer
    {
        public string IP { get; set; }
        public int Port { get; set; }
        private IPAddress Address;
        private IPEndPoint Endpoint;
        private Socket Socket;
        private CancellationTokenSource CT;

        public SocketServer() { }

        public bool VerifyIP()
        {
            return IPAddress.TryParse(IP, out Address);
        }

        public async void Start()
        {
            CT = new CancellationTokenSource();
            CT.Token.Register(() =>
            {
                Socket.Shutdown(SocketShutdown.Both);
            });

            try
            {
                Endpoint = new IPEndPoint(Address, Port);

                if(Endpoint.Address.IsIPv6LinkLocal) //Support for IPv6
                    Socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                else
                    Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                Console.WriteLine($"Socket setup with Address: {Endpoint.Address.ToString()}");

                Socket.Bind(Endpoint);
                Socket.Listen(10);

                await Task.Run(() =>
                {
                    while (!CT.IsCancellationRequested)
                    {
                        Console.WriteLine("Waiting for a connection");

                        Socket handler = Socket.Accept();

                        Console.Write("Connection Recevied:" + handler.RemoteEndPoint.ToString());

                        while (!CT.IsCancellationRequested)
                        {
                            string data = null;

                            bool errorRec = false;

                            while (!CT.IsCancellationRequested)
                            {
                                try
                                {
                                    byte[] bytes = new byte[1024];
                                    int rec = handler.Receive(bytes);
                                    if(rec != 0)
                                        Console.WriteLine($"{rec} bytes received");
                                    data += Encoding.ASCII.GetString(bytes, 0, rec);
                                    if (rec != 1024 && rec != 0)
                                        break;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("And error occured while receiving data: " + ex.StackTrace);
                                    errorRec = true;
                                    break;
                                }
                            }

                            if (errorRec)
                            {
                                handler.Shutdown(SocketShutdown.Both);
                                break;
                            }

                            Console.WriteLine($"Text Received: {data}");
                        }
                    }

                    Socket.Shutdown(SocketShutdown.Both);
                }, CT.Token);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Something went wrong: " + ex.StackTrace);
            }
        }
        
        public void Stop()
        {
            CT.Cancel();
        }
    }
}
