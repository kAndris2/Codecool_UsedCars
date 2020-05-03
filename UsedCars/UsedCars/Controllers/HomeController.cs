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

        public IActionResult Search([FromQuery(Name = "brand")] string brand, [FromQuery(Name = "model")] string model)
        {
            return View();
        }
    }
}
