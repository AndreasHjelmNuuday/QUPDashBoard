using Newtonsoft.Json;

namespace DotJira
{
    public class LinkType
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("inward")]
        public string inward { get; set; }

        [JsonProperty("outward")]
        public string outward { get; set; }
    }
}