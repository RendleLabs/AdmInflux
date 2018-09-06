using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdmInflux.Client;
using Microsoft.AspNetCore.Mvc;
using AdmInflux.Models;

namespace AdmInflux.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly InfluxClient _client;

        public HomeController(InfluxClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var databases = await _client.ListDatabases("localhost").ConfigureAwait(false);
            var vm = new HomeViewModel {Databases = databases};
            return View(vm);
        }

        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
