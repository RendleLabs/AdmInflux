using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AdmInflux.Client.Models;
using Newtonsoft.Json;

namespace AdmInflux.Client
{
    public class DatabasesClient
    {
        private readonly HttpClient _client;
        private readonly InfluxOptions _options;

        public DatabasesClient(HttpClient client, InfluxOptions options)
        {
            _client = client;
            _options = options;
        }
        
        public async ValueTask<IList<Database>> List(string server)
        {
            if (!_options.TryGetUri(server, out string uri)) return null;
            var query = $"{uri}/query?q={Uri.EscapeUriString("SHOW DATABASES")}";
            var responseMessage = await _client.GetAsync(query).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response>(json);
            var series = response.Results?.FirstOrDefault()?.DataSets?.FirstOrDefault(s => s.Name == "databases");
            return series?.Values.Select(v => new Database {Server = server, Name = v[0]}).ToList();
        }
    }
}