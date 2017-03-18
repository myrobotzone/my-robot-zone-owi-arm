using System;
using System.Threading.Tasks;
using my_robot_zone_robot_server;
using owi_arm_dotnet;
using owi_arm_dotnet_usb;

namespace my_robot_zone_robot_server_owi_arm
{
    public class OwiArmMessageHandler : IMessageHandler
    {
        private readonly IOwiArm _arm;
        private readonly ILogger _logger;
        private IOwiCommand _command;

        public OwiArmMessageHandler(ILogger logger)
        {
            _logger = logger;
            var factory = new OwiFactory();
            _arm = factory.CreateArm(new LibUsbOwiConnection());
            _command = factory.CreateCommand();
        }

        public async Task<bool> StartAsync()
        {
            try
            {
                await _arm.ConnectAsync();
            }
            catch (Exception e)
            {
                _logger.Log("{0}", e.Message);
                return false;
            }

            _logger.Log("Robot arm server is listening for commands");

            return true;
        }

        public async Task HandleMessageAsync(string message)
        {
            try
            {
                var parts = message.Split(':');
                var featureId = (FeatureId) Enum.Parse(typeof(FeatureId), parts[0]);
                var value = int.Parse(parts[1]);
                switch (featureId)
                {
                    case FeatureId.Led:
                        if (value == 0)
                            _command = _command.LedOff();
                        else
                            _command = _command.LedOn();
                        break;
                    case FeatureId.Gripper:
                        if (value == 0)
                            _command.GripperClose();
                        else if (value == 1)
                            _command.GripperStop();
                        else
                            _command.GripperOpen();
                        break;
                    case FeatureId.Wrist:
                        if (value == 0)
                            _command.WristDown();
                        else if (value == 1)
                            _command.WristStop();
                        else
                            _command.WristUp();
                        break;
                    case FeatureId.Elbow:
                        if (value == 0)
                            _command.ElbowDown();
                        else if (value == 1)
                            _command.ElbowStop();
                        else
                            _command.ElbowUp();
                        break;
                    case FeatureId.Shoulder:
                        if (value == 0)
                            _command.ShoulderDown();
                        else if (value == 1)
                            _command.ShoulderStop();
                        else
                            _command.ShoulderUp();
                        break;
                    case FeatureId.Base:
                        if (value == 0)
                            _command.BaseRotateClockwise();
                        else if (value == 1)
                            _command.BaseRotateStop();
                        else
                            _command.BaseRotateCounterClockwise();
                        break;
                    default:
                        break;
                }
                await _arm.SendCommandAsync(_command);
            }
            catch (Exception e)
            {
                _logger.Log("{0}", e.Message);
            }
        }

        public async Task StopAsync()
        {
            await _arm.SendCommandAsync(_command.StopAllMovements().LedOff());
            await _arm.DisconnectAsync();
        }

        private enum FeatureId
        {
            Led,
            Gripper,
            Wrist,
            Elbow,
            Shoulder,
            Base
        }
    }
}