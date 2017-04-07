using System.Linq;

namespace my_robot_zone_robot_server
{
    public class MRZMessageFactory
    {
        public MRZMessage Create(string rawMessage)
        {
            var parts = rawMessage.Split(':');
            var featureId = int.Parse(parts.First());
            var payload = parts.Last();
            return new MRZMessage(featureId, payload);
        }
    }
}