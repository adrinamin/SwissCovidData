
using Newtonsoft.Json;

namespace CovidData.Entities
{
    public class CovidInfection
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        public string sex { get; set; }
        public string location { get; set; }
        public int date { get; set; }
    }
}
