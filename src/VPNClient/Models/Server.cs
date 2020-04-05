using Newtonsoft.Json;

namespace VPNClient.Models
{
    public class Server
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        private int _distance;

        [JsonProperty(PropertyName = "distance")]
        public int Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                DistanceWithUnits = string.Join(" ", _distance, "km");
            }
        }

        public string DistanceWithUnits { get; set; }
    }
}
