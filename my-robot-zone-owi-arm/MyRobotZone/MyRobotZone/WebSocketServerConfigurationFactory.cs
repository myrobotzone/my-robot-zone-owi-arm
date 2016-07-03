using my_robot_zone_robot_server;

namespace MyRobotZone
{
    public class WebSocketServerConfigurationFactory : IWebSocketServerConfigurationFactory
    {
        private readonly IFileSystem fileSystem;
        private readonly ISSLCertificateGenerator generator;

        public WebSocketServerConfigurationFactory(IFileSystem fileSystem, ISSLCertificateGenerator generator)
        {
            this.fileSystem = fileSystem;
            this.generator = generator;
        }

        public IWebSocketServerConfiguration CreateConfiguration()
        {
            var config = new WebSocketServerConfiguration("password", fileSystem.GetTempPath());
            if (this.fileSystem.FileExists(config.FilePath) == false)
            {
                using (var stream = this.generator.Generate(config.Password))
                {
                    this.fileSystem.WriteAllBytes(config.FilePath, stream.ToArray());
                }
            }
            return config;
        }
    }
}
