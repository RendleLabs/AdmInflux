using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdmInflux.Client
{
    public class Result
    {
        [JsonProperty("statement_id")]
        public long StatementId { get; set; }

        [JsonProperty("series")]
        public List<DataSet> DataSets { get; set; }
    }

    public class Response
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }
}