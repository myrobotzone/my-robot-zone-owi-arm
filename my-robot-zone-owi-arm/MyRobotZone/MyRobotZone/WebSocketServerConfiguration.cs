namespace MyRobotZone
{
    public enum WebSocketSecurity
    {
        tls,
    }

    public class WebSocketServerConfiguration : IWebSocketServerConfiguration
    {
        public string FilePath { get; private set; }
        public string Password { get; private set; }

        public int Port
        {
            get
            {
                return 5000;
            }
        }

        public WebSocketSecurity Security
        {
            get
            {
                return WebSocketSecurity.tls;
            }
        }

        public WebSocketServerConfiguration(string password, string filePath)
        {
            this.Password = password;
            this.FilePath = filePath;
        }
    }
}
