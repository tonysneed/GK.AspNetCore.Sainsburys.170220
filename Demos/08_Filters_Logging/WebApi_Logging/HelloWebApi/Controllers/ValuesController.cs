using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace HelloWebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/throw
        [HttpGet("/api/throw")]
        public IEnumerable<string> Throw()
        {
            // Don't handle exceptions in controllers!
            //try
            //{
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            throw new Exception("Doh!");
        }
    }
}
