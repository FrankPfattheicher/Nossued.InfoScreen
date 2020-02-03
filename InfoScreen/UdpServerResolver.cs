using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// ReSharper disable ConvertToUsingDeclaration

namespace InfoScreen
{
    public class UdpServerResolver
    {
        private readonly string _id;
        private readonly int _port;

        public UdpServerResolver(string id, int port)
        {
            _id = id;
            _port = port;
        }

        public string Resolve()
        {
            using (var client = new UdpClient())
            {
                client.Client.Bind(new IPEndPoint(IPAddress.Any, _port));

                for(var retry = 0; retry < 1000; retry++)
                {
                    var data = Encoding.UTF8.GetBytes("Resolve-" + _id);
                    client.Send(data, data.Length, "255.255.255.255", _port);

                    for (var wait = 0; wait < 10; wait++)
                    {
                        var text = "";
                        var from = new IPEndPoint(0, 0);
                    
                        var asyncResult = client.BeginReceive( null, null );
                        asyncResult.AsyncWaitHandle.WaitOne(100);
                        if (asyncResult.IsCompleted)
                        {
                            try
                            {
                                var recvBuffer = client.EndReceive( asyncResult, ref from );
                                text = Encoding.UTF8.GetString(recvBuffer);
                                Console.WriteLine(@from.Address + ": " + text);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error: " + ex.Message);
                            }
                        } 
                        else
                        {
                            Console.WriteLine("Warning: Timeout!");
                            break;
                        }                    

                        if (text != "Is-" + _id)
                        {
                            Thread.Sleep(100);
                            continue;
                        }

                        return from.Address.ToString();
                    }
                }

                return null;
            }
        }
        
    }
}
