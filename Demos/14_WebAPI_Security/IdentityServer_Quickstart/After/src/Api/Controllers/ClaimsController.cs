// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    public class ClaimsController : ControllerBase
    {
        [HttpGet("/api/hello")]
        public IActionResult Hello()
        {
            return new JsonResult(new[] { new { Message = "Hello from Web API" } });
        }

        [HttpGet("/api/claims")]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}