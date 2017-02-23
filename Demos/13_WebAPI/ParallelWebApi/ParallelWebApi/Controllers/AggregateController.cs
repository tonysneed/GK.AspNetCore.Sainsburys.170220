using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HelloWebApi.Services;
using HelloWebApi.Models;

namespace HelloWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AggregateController : Controller
    {
        private const int _count = 10;

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tasks = new Task<ServiceResult>[_count];
            var services = new RemoteService[_count];

            for (int i = 0; i < services.Length; i++)
            {
                services[i] = new RemoteService();
                tasks[i] = services[i].GetDataAsync(i + 1);

                // Don't call Task.Run! Creates another thread
                //Task.Run(() => tasks.)

                // Don't block the thread!
                //tasks[i].Wait();
                //var result = tasks[i].Result;
            }

            // Don't block the thread!!!
            //Task.WaitAll(tasks);

            // This will continue WITHOUT blocking the thread
            var results = await Task.WhenAll(tasks);
            return Ok(results);

            //var result = await Task.FromResult(new ServiceResult[_count]);
            //return Ok(result);
        }
    }
}
