using NetCoreServer;
using System;
using System.Text;

namespace Averia.Core.Server
{
    public sealed class TcpChatSession : TcpSession
    {
        public TcpChatSession(TcpServer server)
            : base(server)
        {
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"Chat TCP session with Id {Id} connected!");
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Chat TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Console.WriteLine("Incoming: " + message);

            // Multicast message to all connected sessions
            
            Server.Multicast(message);

            // If the buffer starts with '!' the disconnect the current session
            if (message == "!")
                Disconnect();
        }

        protected override void OnError(System.Net.Sockets.SocketError error)
        {
            Console.WriteLine($"Chat TCP session caught an error with code {error}");
        }
    }
}
