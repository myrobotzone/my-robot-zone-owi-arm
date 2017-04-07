using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using my_robot_zone_robot_server;
using my_robot_zone_robot_server_owi_arm.Handlers;
using owi_arm_dotnet;
using owi_arm_dotnet_usb;

namespace my_robot_zone_robot_server_owi_arm
{
    public class OwiArmMessageHandler : IMessageHandler
    {
        private readonly IOwiArm _arm;
        private readonly ILogger _logger;
        private IOwiCommand _command;
        private readonly List<MRZMessageToOwiArmCommandMapper> _mrzMessageToOwiArmCommandMappers;

        public OwiArmMessageHandler(ILogger logger)
        {
            _logger = logger;
            var factory = new OwiFactory();
            _arm = factory.CreateArm(new LibUsbOwiConnection());
            _command = factory.CreateCommand();

            _mrzMessageToOwiArmCommandMappers = new List<MRZMessageToOwiArmCommandMapper>()
            {
                new MRZMessageToOwiArmCommandMapper(0, "0", command => command.LedOff()),
                new MRZMessageToOwiArmCommandMapper(0, "1", command => command.LedOn()),
                new MRZMessageToOwiArmCommandMapper(1, "0", command => command.GripperClose()),
                new MRZMessageToOwiArmCommandMapper(1, "1", command => command.GripperStop()),
                new MRZMessageToOwiArmCommandMapper(1, "2", command => command.GripperOpen()),
                new MRZMessageToOwiArmCommandMapper(2, "0", command => command.WristDown()),
                new MRZMessageToOwiArmCommandMapper(2, "1", command => command.WristStop()),
                new MRZMessageToOwiArmCommandMapper(2, "2", command => command.WristUp()),
                new MRZMessageToOwiArmCommandMapper(3, "0", command => command.ElbowDown()),
                new MRZMessageToOwiArmCommandMapper(3, "1", command => command.ElbowStop()),
                new MRZMessageToOwiArmCommandMapper(3, "2", command => command.ElbowUp()),
                new MRZMessageToOwiArmCommandMapper(4, "0", command => command.ShoulderDown()),
                new MRZMessageToOwiArmCommandMapper(4, "1", command => command.ShoulderStop()),
                new MRZMessageToOwiArmCommandMapper(4, "2", command => command.ShoulderUp()),
                new MRZMessageToOwiArmCommandMapper(5, "0", command => command.BaseRotateClockwise()),
                new MRZMessageToOwiArmCommandMapper(5, "1", command => command.BaseRotateStop()),
                new MRZMessageToOwiArmCommandMapper(5, "2", command => command.BaseRotateCounterClockwise()),
                new MRZMessageToOwiArmCommandMapper(6, "0", command => command.StopAllMovements()),
                new MRZMessageToOwiArmCommandMapper(6, "1", command => command.ShoulderUp().ElbowDown()),
                new MRZMessageToOwiArmCommandMapper(6, "2", command => command.ShoulderDown().ElbowUp()),
            };
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

        public async Task HandleMessageAsync(MRZMessage message)
        {
            try
            {
                foreach (var mrzMessageToOwiArmCommandMapper in _mrzMessageToOwiArmCommandMappers)
                {
                    _command = mrzMessageToOwiArmCommandMapper.Map(message, _command);
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
    }
}