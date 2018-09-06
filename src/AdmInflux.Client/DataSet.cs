using Newtonsoft.Json;

namespace AdmInflux.Client
{
    public class DataSet
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("columns")]
        public string[] Columns { get; set; }
        
        [JsonProperty("values")]
        public string[][] Values { get; set; }
    }
}