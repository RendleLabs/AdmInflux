using System;
using System.Linq;
using System.Threading.Tasks;
using AdmInflux.Client;
using AdmInflux.Client.Models;
using AdmInflux.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdmInflux.Controllers
{
    [Route("servers/{server}/databases")]
    public class DatabasesController : Controller
    {
        private readonly InfluxClient _client;

        public DatabasesController(InfluxClient client)
        {
            _client = client;
        }

        [HttpGet("{database}")]
        public async Task<IActionResult> Get(string server, string database)
        {
            var series = await _client.Series.Cardinality(server, database);

            series.Select(s => new MeasurementSeriesViewModel {Measurement = s.Name, Count = s.Count});

            var (measurements, retentionPolicies) = await Tasks.Multi(
                _client.Series.Cardinality(server, database),
                _client.RetentionPolicies.List(server, database)
            ).ConfigureAwait(false);

            var vm = new DatabaseViewModel
            {
                Server = server,
                Name = database,
                Measurements = measurements?.Select(s => new MeasurementSeriesViewModel {Measurement = s.Name, Count = s.Count}).ToArray() ??
                               Array.Empty<MeasurementSeriesViewModel>(),
                RetentionPolicies = retentionPolicies ?? Array.Empty<RetentionPolicy>()
            };
            return View(vm);
        }
    }
}