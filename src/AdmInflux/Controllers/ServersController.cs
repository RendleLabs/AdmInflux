using System;
using System.Linq;
using System.Threading.Tasks;
using AdmInflux.Client;
using AdmInflux.Client.Models;
using AdmInflux.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdmInflux.Controllers
{
    [Route("servers")]
    public class ServersController : Controller
    {
        private readonly InfluxClient _client;

        public ServersController(InfluxClient client)
        {
            _client = client;
        }

        [HttpGet("{server}")]
        public async Task<IActionResult> Get(string server)
        {
            var databases = await _client.Databases.List(server);
            var dvms = databases.Select(d => new DatabaseViewModel
            {
                Server = server,
                Name = d.Name
            }).ToList();
            
            await Task.WhenAll(dvms.Select(d => Load(server, d)));
            
            var vm = new ServerViewModel
            {
                Name = server,
                Databases = dvms
            };
            return View(vm);
        }

        private async Task Load(string server, DatabaseViewModel database)
        {
            database.SeriesCardinality =
                (await _client.Series.Cardinality(server, database.Name)) ?? Array.Empty<Cardinality>();
        }
    }
}