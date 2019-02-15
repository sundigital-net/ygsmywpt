using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Entity;

namespace MyWebApi.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        private MyDbContext _context;
        public TestController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}