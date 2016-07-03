
namespace MyRobotZone
{
    public interface IWebSocketServerConfiguration
    {
        string FilePath { get; }
        string Password { get; }
        int Port { get; }
        WebSocketSecurity Security { get; }
    }
}
