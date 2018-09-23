using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdmInflux.Client;
using Microsoft.AspNetCore.Mvc;
using AdmInflux.Models;
using Microsoft.Extensions.Options;

namespace AdmInflux.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly InfluxClient _client;
        private readonly InfluxOptions _options;

        public HomeController(InfluxClient client, IOptions<InfluxOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var servers = await Task.WhenAll(_options.Servers.Select(s => LoadServer(s.Name))).ConfigureAwait(false);
            
            foreach (var server in _options.Servers)
            {
                var databases = await _client.Databases.List("localhost").ConfigureAwait(false);
            }
            var vm = new HomeViewModel {Servers = servers};
            return View(vm);
        }

        private async Task<ServerViewModel> LoadServer(string server)
        {
            var databases = await _client.Databases.List(server);
            return new ServerViewModel
            {
                Name = server,
                Databases = databases.Select(d => new DatabaseViewModel
                {
                    Server = d.Server,
                    Name = d.Name
                }).ToList()
            };
        }

        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
