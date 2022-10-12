using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DotJira
{
    public class Team
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
