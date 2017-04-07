using System;
using my_robot_zone_robot_server;
using owi_arm_dotnet;

namespace my_robot_zone_robot_server_owi_arm.Handlers
{
    class MRZMessageToOwiArmCommandMapper
    {
        private readonly int _featureId;
        private readonly string _paylad;
        private readonly Func<IOwiCommand, IOwiCommand> _mappingFunction;

        public MRZMessageToOwiArmCommandMapper(int featureId, string paylad, Func<IOwiCommand, IOwiCommand> mappingFunction)
        {
            _featureId = featureId;
            _paylad = paylad;
            _mappingFunction = mappingFunction;
        }

        public IOwiCommand Map(MRZMessage message, IOwiCommand currentCommand)
        {
            if (IsMessageMapNeeded(message))
            {
                return _mappingFunction.Invoke(currentCommand);
            }
            return currentCommand;
        }

        private bool IsMessageMapNeeded(MRZMessage message)
        {
            return _featureId == message.FeatureId && _paylad == message.Payload;
        }
    }
}
