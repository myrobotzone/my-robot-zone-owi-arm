using robot_arm_server;
using System;

namespace my_robot_zone_owi_arm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start the server!");

            Console.ReadKey();
            Console.WriteLine();

            var logger = new ConsoleLogger();
            var robotArmServer = new RobotArmServer(logger);
            if (!robotArmServer.Start())
            {
                logger.Log("Failed to start server");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            robotArmServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }

        class ConsoleLogger : ILogger
        {
            public void Log(string format, params object[] args)
            {
                Console.WriteLine(string.Format(format, args));
            }
        }
    }
}
