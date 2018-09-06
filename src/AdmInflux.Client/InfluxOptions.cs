using System;
using System.Linq;

namespace AdmInflux.Client
{
    public class InfluxOptions
    {
        public InfluxServer[] Servers { get; set; }

        public bool TryGetUri(string name, out string uri)
        {
            if (Servers == null || Servers.Length == 0)
            {
                uri = null;
                return false;
            }

            uri = Servers.FirstOrDefault(s => name.Equals(s.Name, StringComparison.OrdinalIgnoreCase))?.Uri.Trim('/');
            return uri != null;
        }
    }
}