using System.Threading.Tasks;
using AdmInflux.Client;
using AdmInflux.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdmInflux.Controllers
{
    [Route("databases")]
    public class DatabasesController : Controller
    {
        private readonly InfluxClient _client;

        public DatabasesController(InfluxClient client)
        {
            _client = client;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var measurements = await _client.ListMeasurements("localhost", name).ConfigureAwait(false);
            var vm = new DatabaseViewModel {Name = name, Measurements = measurements};
            return View(vm);
        }
    }
}