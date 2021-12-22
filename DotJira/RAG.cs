using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotJira
{
    public class RAG
    {        
        public RAG(string v)
        {
            this.Value = "black";

            if (v.ToLower().Equals("g"))
            {
                this.Value = "green";
            }
            else if (v.ToLower().Equals("a"))
            {
                this.Value = "amber";
            }
            else if (v.ToLower().Equals("r"))
            {
                this.Value = "red";
            }            
        }

        public RAG()
        {
            Value = "black";
        }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("disabled")]
        public string Disabled { get; set; }
    }
}
