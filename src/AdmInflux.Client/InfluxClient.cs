using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AdmInflux.Client.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AdmInflux.Client
{
    public class InfluxClient
    {
        private readonly HttpClient _client;
        private readonly InfluxOptions _options;

        public InfluxClient(HttpClient client, IOptions<InfluxOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        public async ValueTask<List<Database>> ListDatabases(string server)
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

        public async Task<List<Measurement>> ListMeasurements(string server, string database)
        {
            if (string.IsNullOrWhiteSpace(server)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(server));
            if (string.IsNullOrWhiteSpace(database)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));
            
            if (!_options.TryGetUri(server, out string uri)) return null;
            var query = $"{uri}/query?db={database}&q={Uri.EscapeUriString("SHOW MEASUREMENTS")}";
            var responseMessage = await _client.GetAsync(query).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response>(json);
            var series = response.Results?.FirstOrDefault()?.DataSets?.FirstOrDefault(s => s.Name == "measurements");
            return series?.Values.Select(v => new Measurement { Server = server, Database = database, Name = v[0]}).ToList();
        }
        
        public async Task<List<Series>> ListSeries(string server, string database, string measurement)
        {
            if (string.IsNullOrWhiteSpace(server)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(server));
            if (string.IsNullOrWhiteSpace(database)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));
            
            if (!_options.TryGetUri(server, out string uri)) return null;
            var query = $"{uri}/query?db={database}&q={Uri.EscapeUriString($"SHOW SERIES FROM \"{measurement}\"")}";
            var responseMessage = await _client.GetAsync(query).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }
            var json = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response>(json);
            var series = response.Results?.FirstOrDefault()?.DataSets?.FirstOrDefault();
            return series?.Values.Select(v => new Series {Key = v[0]}).ToList();
        }
    }
}
