using owi_arm_dotnet;
using System;

namespace my_robot_zone_robot_server.MessageHandlers
{
    public class OwiArmMessageHandler : IMessageHandler
    {
        readonly IOwiArm arm = new OwiArm();
        IOwiCommand command = new OwiCommand();
        readonly ILogger logger;

        public OwiArmMessageHandler(ILogger logger)
        {
            this.logger = logger;
        }

        public bool Start()
        {
            try
            {
                arm.Connect();
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

        public void HandleMessage(string message)
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
                this.arm.SendCommand(this.command);
            }
            catch (Exception e)
            {
                this.logger.Log("{0}", e.Message);
            }
        }

        public void Stop()
        {
            this.arm.SendCommand(command.StopAllMovements().LedOff());
            this.arm.Disconnect();
        }
    }
}
