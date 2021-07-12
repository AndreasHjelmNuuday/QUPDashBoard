using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DotJira
{
    public class LinkedIssue : Issue
    {
        [JsonProperty("type")]
        public LinkType LinkType {get;set;}

        [JsonProperty("inwardIssue")]
        public Issue InwardIssue { get; set; }

        [JsonProperty("outwardIssue")]
        public Issue OutwardIssue { get; set; }
    }
}
