using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfoServer
{
    public class UdpClientService : IDisposable
    {
        private readonly string _id;
        private readonly int _port;
        private readonly UdpClient _client;
        private readonly CancellationTokenSource _cancel;
        private readonly Task _responder;
        
        public UdpClientService(string id, int port)
        {
            _id = id;
            _port = port;
            
            _client = new UdpClient();
            _client.Client.Bind(new IPEndPoint(IPAddress.Any, _port));
            
            _cancel = new CancellationTokenSource();
            
            _responder = Task.Run(Responder, _cancel.Token);
        }

        private void Responder()
        {
            while (!_cancel.IsCancellationRequested)
            {
                var from = new IPEndPoint(0, 0);
                var recvBuffer = _client.Receive(ref from);
                var text = Encoding.UTF8.GetString(recvBuffer);
                Console.WriteLine(from.Address.ToString() + ": " + text);

                if (text != "Resolve-" + _id)
                {
                    Thread.Sleep(100);
                    continue;
                }
                
                Console.WriteLine("Send to " + @from.Address + ": Is-" + _id);
                var bytes = Encoding.UTF8.GetBytes("Is-" + _id);
                _client.Send(bytes, bytes.Length, from.Address.ToString(), _port);
            }
        }

        public void Dispose()
        {
            _cancel.Cancel();
            _responder.Wait(1000);
            _client.Dispose();
        }
        
    }
}