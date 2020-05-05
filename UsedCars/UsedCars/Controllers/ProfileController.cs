using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UsedCars.Controllers
{
    public class ProfileController : Controller
    {
        public IDAO IDAO = IDAO.Singleton;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("/user/{id}")]
        public IActionResult User(int id)

        {
            return View("UserPage", IDAO.GetUserByID(id));
        }
    }
}