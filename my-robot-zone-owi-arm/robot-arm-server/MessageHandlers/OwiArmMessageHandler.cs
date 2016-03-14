using owi_arm_dotnet;
using System;

namespace robot_arm_server.MessageHandlers
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

        public void Stop()
        {
            this.arm.SendCommand(command.StopAllMovements().LedOff());
            this.arm.Disconnect();
        }
    }
}
