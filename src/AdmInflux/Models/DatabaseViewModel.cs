using System.Collections.Generic;
using AdmInflux.Client.Models;

namespace AdmInflux.Models
{
    public class DatabaseViewModel
    {
        public string Name { get; set; }
        public List<Measurement> Measurements { get; set; }
    }
}