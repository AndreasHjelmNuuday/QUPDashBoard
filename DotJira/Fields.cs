﻿using System;
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

        [JsonProperty(Constants.DESCRIPTION_FIELD_ID)]
        public String Description { get; set; }

        [JsonProperty(Constants.ASSIGNEE_ID)]
        public Assignee Assignee { get; set; }

        [JsonProperty(Constants.TEAM_CUSTOM_FIELD_ID)]        
        public Team Team { get; set; }

        [JsonProperty(Constants.RAG_CUSTOM_FIELD_ID)]
        public RAG RAG { get; set; }

        [JsonProperty(Constants.RAG_COMMENT_CUSTOM_FIELD_ID)]
        public string RAGComment { get; set; }

        [JsonProperty(Constants.ISSUE_TYPE_ID)]
        public IssueType Type { get; set; }

        [JsonProperty(Constants.KEY_RESULT_1_CUSTOM_FIELD_ID)]
        public string KeyResult1 { get; set; }

        [JsonProperty(Constants.KEY_RESULT_2_CUSTOM_FIELD_ID)]
        public string KeyResult2 { get; set; }

        [JsonProperty(Constants.KEY_RESULT_3_CUSTOM_FIELD_ID)]
        public string KeyResult3 { get; set; }

        [JsonProperty(Constants.KEY_RESULT_4_CUSTOM_FIELD_ID)]
        public string KeyResult4 { get; set; }

        [JsonProperty(Constants.PARENT_CUSTOM_FIELD_ID)]
        public string Parent { get; set; }

        [JsonProperty(Constants.EPIC_LINK_CUSTOM_FIELD_ID)]
        public string EpicLink { get; set; }

        [JsonProperty("issuelinks")]
        public List<LinkedIssue> linkedIssues { get; set; } = new List<LinkedIssue>();
        
        public List<KeyResult> SplitKeyResults()
        {            
            return KeyResultHelper.SplitKeyResults(this);
        }
    }
}
