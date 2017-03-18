using System.IO;
using System.Security.Cryptography.X509Certificates;
using SuperSocket.SocketBase.Config;
using SuperSocket.WebSocket;

namespace my_robot_zone_robot_server
{
    public class RobotServer : IRobotServer
    {
        private readonly WebSocketServer _appServer = new WebSocketServer();
        private readonly ISSLCertificateGenerator _certificateGenerator = new SSLCertificateGenerator();
        private readonly ILogger _logger;
        private readonly IMessageHandler _messageHander;
        private readonly ISystemUtils _systemUtils = new SystemUtils();

        public RobotServer(IMessageHandler messageHandler, ILogger logger)
        {
            _messageHander = messageHandler;
            _logger = logger;
        }

        public bool Start()
        {
            if (!StartSocketServer())
                return false;

            if (!_messageHander.StartAsync().Result)
            {
                _appServer.Stop();
                return false;
            }

            return true;
        }

        public void Stop()
        {
            _appServer.Stop();
            _appServer.NewSessionConnected -= appServer_NewSessionConnected;
            _appServer.NewMessageReceived -= appServer_NewRequestReceived;
            _messageHander.StopAsync();
        }

        private bool StartSocketServer()
        {
            var certficateFile = Path.Combine(_systemUtils.GetTempPath(), "myrobotzone.pfx");
            const string password = "myrobotzone";

            GenerateCertficate(certficateFile, password);

            var config = CreateConfiguration(certficateFile, password);

            if (!_appServer.Setup(config))
            {
                _logger.Log("Failed to setup socket server to listen on port {0}", config.Port);
                return false;
            }
            _logger.Log("Starting server on localhost:{0}", config.Port);
            if (!_appServer.Start())
            {
                _logger.Log("Failed to start socket server");
                return false;
            }

            _appServer.NewSessionConnected += appServer_NewSessionConnected;
            _appServer.NewMessageReceived += appServer_NewRequestReceived;

            return true;
        }

        private static IServerConfig CreateConfiguration(string certficateFile, string password)
        {
            IServerConfig config = new ServerConfig
            {
                Port = 5000,
                Security = "tls",
                Certificate = new CertificateConfig
                {
                    FilePath = certficateFile,
                    Password = password,
                    KeyStorageFlags = X509KeyStorageFlags.MachineKeySet
                }
            };
            return config;
        }

        private void GenerateCertficate(string certficateFile, string password)
        {
            _logger.Log("Generating {0} (this can take a few seconds)...", certficateFile);
            if (_systemUtils.FileExists(certficateFile))
                return;
            _certificateGenerator.Generate(certficateFile, password);
        }

        private void appServer_NewSessionConnected(WebSocketSession session)
        {
            _logger.Log("Session connected: {0}", session.RemoteEndPoint.Address);
        }

        private void appServer_NewRequestReceived(WebSocketSession session, string message)
        {
            _messageHander.HandleMessageAsync(message);
        }
    }
}