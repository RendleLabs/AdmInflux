using System.Net.Http;
using Microsoft.Extensions.Options;

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

        private RetentionPolicies _retentionPolicies;
        public RetentionPolicies RetentionPolicies =>
            _retentionPolicies ?? (_retentionPolicies = new RetentionPolicies(_client, _options));

        private DatabasesClient _databases;
        public DatabasesClient Databases =>
            _databases ?? (_databases = new DatabasesClient(_client, _options));

        private SeriesClient _series;
        public SeriesClient Series =>
            _series ?? (_series = new SeriesClient(_client, _options));

        private MeasurementsClient _measurements;
        public MeasurementsClient Measurements =>
            _measurements ?? (_measurements = new MeasurementsClient(_client, _options));
    }
}