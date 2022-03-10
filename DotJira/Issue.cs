using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DotJira
{
    public class Issue
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("fields")]
        public Fields Fields { get; set; }        

        public List<Issue> Children { get; set; } = new List<Issue>();
        public List<Issue> IsImplementedBy { get; set; } = new List<Issue>();
        public Int32 DoneChildren { get; set; } = 0;
        public string Parent { get; set; }

        public KPI KPIs { get; set; }
    }
}
