using System.Collections.Generic;
using AdmInflux.Client.Models;

namespace AdmInflux.Models
{
    public class DatabaseViewModel
    {
        public string Server { get; set; }
        public string Name { get; set; }
        public IList<Cardinality> SeriesCardinality { get; set; }
        public IList<Measurement> Measurements { get; set; }
        public IList<RetentionPolicy> RetentionPolicies { get; set; }
    }
}