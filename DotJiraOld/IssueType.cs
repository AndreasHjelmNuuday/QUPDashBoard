using Newtonsoft.Json;

namespace DotJira
{
    public class IssueType
    {
        [JsonProperty(Constants.ISSUE_TYPE_NAME_ID)]
        public string Name { get; set; }

        [JsonProperty(Constants.ISSUE_TYPE_ICON_URL_ID)]
        public string IconUrl { get; set; }
    }
}