using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AdmInflux.Client.Caches;
using AdmInflux.Client.Models;
using Newtonsoft.Json;

namespace AdmInflux.Client.Clients
{
    public class DatabaseClient
    {
        private readonly HttpClient _client;
        private readonly InfluxOptions _options;

        public DatabaseClient(HttpClient client, InfluxOptions options)
        {
            _client = client;
            _options = options;
        }

        public ValueTask<List<Database>> ListDatabases(string server)
        {
            return DatabaseCache.TryGet(server, out var databases)
                ? new ValueTask<List<Database>>(databases)
                : new ValueTask<List<Database>>(ListDatabasesAsync(server));
        }

        private async Task<List<Database>> ListDatabasesAsync(string server)
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
            var databases = series?.Values.Select(v => new Database {Name = v[0]}).ToList();
            DatabaseCache.Update(server, databases);
            return databases;
        }
    }
}