using BaSocket;
using System.Threading;

namespace MyRobotZone.UWP
{
    public sealed partial class MainPage
    {
        //WebSocketServer server;

        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new MyRobotZone.App());

            //this.server = new WebSocketServer(new TcpListenerFacotry(), new SHA1());
            //var source = new CancellationTokenSource();
            //server.StartAsync(source.Token);
        }
    }
}
