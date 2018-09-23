using System.Collections.Generic;

namespace AdmInflux.Models
{
    public class ServerViewModel
    {
        public string Name { get; set; }
        public List<DatabaseViewModel> Databases { get; set; }
    }
}