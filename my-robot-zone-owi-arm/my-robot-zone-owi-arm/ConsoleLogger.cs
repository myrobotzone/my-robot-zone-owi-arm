using System;
using my_robot_zone_robot_server;

namespace my_robot_zone_robot_server_owi_arm
{
    internal class ConsoleLogger : ILogger
    {
        public void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}