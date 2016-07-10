using BaSocket;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRobotZone.WindowsDesktop
{




    class Program
    {
        static void Main(string[] args)
        {
            var webSocketServer = new WebSocketServer(new TcpServer(), new SHA1());
            CancellationToken token = new CancellationToken();
            webSocketServer.StartAsync(token);
            token.WaitHandle.WaitOne();
        }
    }
}
