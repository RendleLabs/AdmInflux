using System.Threading.Tasks;
using AdmInflux.Client;
using AdmInflux.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdmInflux.Controllers
{
    [Route("databases")]
    public class MeasurementsController : Controller
    {
        private readonly InfluxClient _client;

        public MeasurementsController(InfluxClient client)
        {
            _client = client;
        }

        [HttpGet("{database}/{measurement}")]
        public async Task<IActionResult> Get(string database, string measurement)
        {
            var series = await _client.ListSeries("localhost", database, measurement).ConfigureAwait(false);
            var vm = new MeasurementViewModel {Name = measurement, Series = series};
            return View(vm);
        }
    }
}