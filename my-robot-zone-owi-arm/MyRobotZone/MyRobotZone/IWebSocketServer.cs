using System;

namespace MyRobotZone
{
    public interface IWebSocketServer
    {
        event Action<string> MessageRecieved;

        void Start(IWebSocketServerConfiguration config);

        void Stop();
    }
}
