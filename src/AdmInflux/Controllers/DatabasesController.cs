using System;
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
            var (measurements, retentionPolicies) = await Tasks.Multi(
                _client.Measurements.List(server, database),
                _client.RetentionPolicies.List(server, database)
                ).ConfigureAwait(false);
            
            var vm = new DatabaseViewModel
            {
                Server = server,
                Name = database, 
                Measurements = measurements ?? Array.Empty<Measurement>(),
                RetentionPolicies = retentionPolicies ?? Array.Empty<RetentionPolicy>()
            };
            return View(vm);
        }
    }
}