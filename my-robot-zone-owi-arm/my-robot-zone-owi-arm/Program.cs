using owi_arm_dotnet;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_robot_zone_owi_arm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start the server!");

            Console.ReadKey();
            Console.WriteLine();

            var appServer = new AppServer();

            //Setup the appServer
            if (!appServer.Setup(5000)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            IOwiArm arm = new OwiArm();
            IOwiCommand command = new OwiCommand();

            try
            {
                arm.Connect();
            }
            catch(Exception e)
            {
                appServer.Stop();
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }

            var newSessionHandler = new SessionHandler<AppSession>(appServer_NewSessionConnected);
            appServer.NewSessionConnected += newSessionHandler;

            var newRequestReceived = new RequestHandler<AppSession, StringRequestInfo>((session, requestInfo) => appServer_NewRequestReceived(session, requestInfo, arm, command));
            appServer.NewRequestReceived += newRequestReceived;

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();
            arm.SendCommand(command.StopAllMovements().LedOff());
            arm.Disconnect();

            appServer.NewSessionConnected -= newSessionHandler;
            appServer.NewRequestReceived -= newRequestReceived;

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }

        private static void appServer_NewSessionConnected(AppSession session)
        {
            Console.WriteLine(string.Format("Session connected: {0}", session.RemoteEndPoint.Address));
        }

        private static void appServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo, IOwiArm arm, IOwiCommand command)
        {
            Console.WriteLine(string.Format("Received message {0}", requestInfo.Body));
            arm.SendCommand(command.LedOn());
        }
    }
}
