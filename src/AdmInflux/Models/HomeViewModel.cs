using System.Collections.Generic;
using AdmInflux.Client;
using AdmInflux.Client.Models;

namespace AdmInflux.Models
{
    public class HomeViewModel
    {
        public IList<ServerViewModel> Servers { get; set; }
    }
}