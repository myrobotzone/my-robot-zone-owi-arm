using my_robot_zone_robot_server;
using owi_arm_dotnet;
using owi_arm_dotnet_usb;
using System;
using System.Threading.Tasks;

namespace my_robot_zone_robot_server_owi_arm
{
    public class OwiArmMessageHandler : IMessageHandler
    {
        readonly IOwiArm arm;
        IOwiCommand command;
        readonly ILogger logger;

        public OwiArmMessageHandler(ILogger logger)
        {
            this.logger = logger;
            var factory = new OwiFactory();
            this.arm = factory.CreateArm(new LibUsbOwiConnection());
            this.command = factory.CreateCommand();
        }

        public async Task<bool> StartAsync()
        {
            try
            {
                await arm.ConnectAsync();
            }
            catch (Exception e)
            {
                this.logger.Log("{0}", e.Message);
                return false;
            }

            this.logger.Log("Robot arm server is listening for commands");

            return true;
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

        public async Task HandleMessageAsync(string message)
        {
            //this.logger.Log("Received message {0}", message);

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
                        if (value == 0)
                            this.command.GripperClose();
                        else if (value == 1)
                            this.command.GripperStop();
                        else
                            this.command.GripperOpen();
                        break;
                    case FeatureId.Wrist:
                        if (value == 0)
                            this.command.WristDown();
                        else if (value == 1)
                            this.command.WristStop();
                        else
                            this.command.WristUp();
                        break;
                    case FeatureId.Elbow:
                        if (value == 0)
                            this.command.ElbowDown();
                        else if (value == 1)
                            this.command.ElbowStop();
                        else
                            this.command.ElbowUp();
                        break;
                    case FeatureId.Shoulder:
                        if (value == 0)
                            this.command.ShoulderDown();
                        else if (value == 1)
                            this.command.ShoulderStop();
                        else
                            this.command.ShoulderUp();
                        break;
                    case FeatureId.Base:
                        if (value == 0)
                            this.command.BaseRotateClockwise();
                        else if (value == 1)
                            this.command.BaseRotateStop();
                        else
                            this.command.BaseRotateCounterClockwise();
                        break;
                    default:
                        break;
                }
                await this.arm.SendCommandAsync(this.command);
            }
            catch (Exception e)
            {
                this.logger.Log("{0}", e.Message);
            }
        }

        public async Task StopAsync()
        {
            await this.arm.SendCommandAsync(command.StopAllMovements().LedOff());
            await this.arm.DisconnectAsync();
        }
    }
}
