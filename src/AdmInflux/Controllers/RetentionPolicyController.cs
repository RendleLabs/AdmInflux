using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AdmInflux.Client;
using AdmInflux.Client.Models;
using AdmInflux.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdmInflux.Controllers
{
    public class RetentionPolicyController : Controller
    {
        private readonly InfluxClient _client;

        public RetentionPolicyController(InfluxClient client)
        {
            _client = client;
        }

        [HttpGet("{server}/{database}/rp/create")]
        public ActionResult New(string server, string database)
        {
            var vm = new NewRetentionPolicyViewModel
            {
                Server = server,
                Database = database
            };
            return View(vm);
        }

        [HttpPost("{server}/{database}/rp/create")]
        public async Task<ActionResult> Post(string server, string database,
            [FromForm] NewRetentionPolicyViewModel model)
        {
            var success = await _client.RetentionPolicies.Create(server, database, new RetentionPolicy
            {
                Name = model.Name,
                Duration = model.Duration,
                ShardGroupDuration = model.ShardDuration,
                Default = model.MakeDefault
            });

            return success
                ? RedirectToAction("Get", "Databases", new {server, name = database})
                : RedirectToAction("New", new {server, database});
        }
    }
}