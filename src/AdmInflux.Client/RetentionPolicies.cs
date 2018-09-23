using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AdmInflux.Client.Models;
using Newtonsoft.Json;

namespace AdmInflux.Client
{
    public class RetentionPolicies
    {
        private readonly HttpClient _client;
        private readonly InfluxOptions _options;

        public RetentionPolicies(HttpClient client, InfluxOptions options)
        {
            _client = client;
            _options = options;
        }

        public async Task<IList<RetentionPolicy>> List(string server, string database)
        {
            if (string.IsNullOrWhiteSpace(server)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(server));
            if (string.IsNullOrWhiteSpace(database)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));
            
            if (!_options.TryGetUri(server, out string uri)) return null;
            var query = $"{uri}/query?db={database}&q={Uri.EscapeUriString($"SHOW RETENTION POLICIES")}";
            var responseMessage = await _client.GetAsync(query).ConfigureAwait(false);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }
            var json = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response>(json);
            var policies = response.Results?.FirstOrDefault()?.DataSets?.FirstOrDefault();
            return policies?.Values.Select(v => new RetentionPolicy
            {
                Name = v[0],
                Duration = v[1],
                ShardGroupDuration = v[2],
                ReplicaN = int.TryParse(v[3], out int replicaN) ? replicaN : 1,
                Default = bool.TryParse(v[4], out bool d) && d
            }).ToList();
        }

        public async Task<bool> Create(string server, string database, RetentionPolicy policy)
        {
            if (string.IsNullOrWhiteSpace(server)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(server));
            if (string.IsNullOrWhiteSpace(database)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));
            
            if (!_options.TryGetUri(server, out string uri)) return false;
            var command = $"CREATE RETENTION POLICY \"{policy.Name}\" ON \"{database}\" DURATION {policy.Duration} REPLICATION 1";
            if (!string.IsNullOrWhiteSpace(policy.ShardGroupDuration))
            {
                command += $" SHARD DURATION {policy.ShardGroupDuration}";
            }

            if (policy.Default)
            {
                command += " DEFAULT";
            }
            var requestUri = $"{uri}/query?q={Uri.EscapeUriString(command)}";
            var responseMessage = await _client.GetAsync(requestUri).ConfigureAwait(false);
            return responseMessage.IsSuccessStatusCode;
        }
    }
}