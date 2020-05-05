using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UsedCars.Models;

namespace UsedCars.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IDAO IDAO = IDAO.Singleton;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(IDAO);
        }

        public IActionResult Search([FromQuery(Name = "brand")] string brand, [FromQuery(Name = "model")] string model, [FromQuery(Name = "type")] string type,
            [FromQuery(Name = "fuel")] string fuel, [FromQuery(Name = "vintage-from")] int vfrom, [FromQuery(Name = "vintage-to")] int vto,
            [FromQuery(Name = "purchase-from")] int pfrom, [FromQuery(Name = "purchase-to")] int pto, [FromQuery(Name = "odometer-from")] int ofrom,
            [FromQuery(Name = "odometer-to")] int oto, [FromQuery(Name = "cylinder-from")] int cfrom, [FromQuery(Name = "cylinder-to")] int cto)
        {
            return View(IDAO);
        }
    }
}
