
using Newtonsoft.Json;

namespace CovidData.Web.Data
{
    public class CovidData
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        public string sex { get; set; }
        public string geoRegion { get; set; }
        public int datum { get; set; }
        public int pop { get; set; }
        public string type { get; set; }
        public string type_variant { get; set; }
        public string version { get; set; }
        public string datum_unit { get; set; }
        public string datum_dboardformated { get; set; }
        public bool timeframe_all { get; set; }
        public bool timeframe_phase2 { get; set; }
        public bool timeframe_phase2b { get; set; }
        public bool timeframe_28d { get; set; }
        public bool timeframe_14d { get; set; }        
        public string _rid { get; set; }
        public string _self { get; set; }
        public string _etag { get; set; }
        public string _attachments { get; set; }
        public int _ts { get; set; }
    }
}
