using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labs.MvcLover.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Labs.MvcLover.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WooController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(new Woo());
        }
    }
}
