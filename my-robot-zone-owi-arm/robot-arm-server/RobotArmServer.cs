using owi_arm_dotnet;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.WebSocket;
using System;

namespace robot_arm_server
{
    public class RobotArmServer : IRobotArmServer
    {
        WebSocketServer appServer = new WebSocketServer();
        IOwiArm arm = new OwiArm();
        IOwiCommand command = new OwiCommand();
        ILogger logger;

        public RobotArmServer(ILogger logger)
        {
            this.logger = logger;
        }

        public bool Start()
        {
            return StartSocketServer() && StartOwiArm();
        }

        public void Stop()
        {
            this.appServer.Stop();
            this.appServer.NewSessionConnected -= this.appServer_NewSessionConnected;
            this.appServer.NewMessageReceived -= this.appServer_NewRequestReceived;
            this.arm.SendCommand(command.StopAllMovements().LedOff());
            this.arm.Disconnect();
        }

        private bool StartSocketServer()
        {
            const int port = 5000;
            if (!this.appServer.Setup(port))
            {
                this.logger.Log("Failed to setup socket server to listen on port {0}", port);
                return false;
            }
            if (!this.appServer.Start())
            {
                this.logger.Log("Failed to start socket server");
                return false;
            }

            this.appServer.NewSessionConnected += this.appServer_NewSessionConnected;
            this.appServer.NewMessageReceived += this.appServer_NewRequestReceived;

            return true;
        }

        private bool StartOwiArm()
        {
            try
            {
                arm.Connect();
            }
            catch (Exception e)
            {
                this.logger.Log("{0}", e.Message);
                this.appServer.Stop();
                return false;
            }

            this.logger.Log("Robot arm server is listening for commands");

            return true;
        }

        void appServer_NewSessionConnected(WebSocketSession session)
        {
            this.logger.Log("Session connected: {0}", session.RemoteEndPoint.Address);
        }

        enum FeatureId
        {
            Led,
            Gripper,
            Wrist,
            Elbow,
            Shoulder,
            Base

        }

        void appServer_NewRequestReceived(WebSocketSession session, string message)
        {
            this.logger.Log("Received message {0}", message);

            try
            {
                var parts = message.Split(':');
                var featureId = (FeatureId)Enum.Parse(typeof(FeatureId), parts[0]);
                var value = int.Parse(parts[1]);
                switch (featureId)
                {
                    case FeatureId.Led:
                        if (value == 0)
                            this.command = this.command.LedOff();
                        else
                            this.command = this.command.LedOn();
                        break;
                    case FeatureId.Gripper:
                        break;
                    case FeatureId.Wrist:
                        break;
                    case FeatureId.Elbow:
                        break;
                    case FeatureId.Shoulder:
                        break;
                    case FeatureId.Base:
                        break;
                    default:
                        break;
                }
                this.arm.SendCommand(this.command);
            }
            catch (Exception e)
            {
                this.logger.Log("{0}", e.Message);
            }
        }
    }
}
