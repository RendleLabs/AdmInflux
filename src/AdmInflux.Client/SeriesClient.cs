using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AdmInflux.Client.Models;
using Newtonsoft.Json;

namespace AdmInflux.Client
{
    public class SeriesClient
    {
        private readonly HttpClient _client;
        private readonly InfluxOptions _options;

        public SeriesClient(HttpClient client, InfluxOptions options)
        {
            _client = client;
            _options = options;
        }

        public async Task<IList<Cardinality>> Cardinality(string server, string database)
        {
            if (string.IsNullOrWhiteSpace(server)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(server));
            if (string.IsNullOrWhiteSpace(database)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));
            
            if (!_options.TryGetUri(server, out string uri)) return null;
            
            var query = $"{uri}/query?q={Uri.EscapeUriString($"SHOW SERIES EXACT CARDINALITY ON \"{database}\"")}";
            var responseMessage = await _client.GetAsync(query).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }
            var json = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response>(json);
            var cardinalities = response.Results?.FirstOrDefault()?.DataSets?.Select(ds => new Cardinality
            {
                Name = ds.Name,
                Count = int.TryParse(ds.Values?[0]?[0], out int c) ? c : 0
            }).ToList();
            return cardinalities;
        }
        
        public async Task<List<Series>> List(string server, string database, string measurement)
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