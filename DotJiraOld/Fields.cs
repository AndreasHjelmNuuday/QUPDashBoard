using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DotJira
{
    public class Fields
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("assignee")]
        public Assignee Assignee { get; set; }

        [JsonProperty(Constants.TEAM_CUSTOM_FIELD_ID)]        
        public string Team { get; set; }

        [JsonProperty(Constants.RAG_CUSTOM_FIELD_ID)]
        public RAG RAG { get; set; }

        [JsonProperty(Constants.ISSUE_TYPE_ID)]
        public IssueType Type { get; set; }

        [JsonProperty(Constants.PARENT_CUSTOM_FIELD_ID)]
        public string Parent { get; set; }

        [JsonProperty("issuelinks")]
        public List<LinkedIssue> linkedIssues { get; set; } = new List<LinkedIssue>();
    }
}
