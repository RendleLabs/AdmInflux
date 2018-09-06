using System.Collections.Generic;
using AdmInflux.Client.Models;

namespace AdmInflux.Models
{
    public class MeasurementViewModel
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Name { get; set; }
        public List<Series> Series { get; set; }
    }
}