namespace my_robot_zone_robot_server
{
    public class MRZMessage
    {
        public MRZMessage(int featureId, string payload)
        {
            FeatureId = featureId;
            Payload = payload;
        }

        public int FeatureId { get; private set; }

        public string Payload { get; private set; }
    }
}
