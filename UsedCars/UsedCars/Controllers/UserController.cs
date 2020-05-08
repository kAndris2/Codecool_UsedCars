using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UsedCars.Controllers
{
    public class UserController : Controller
    {
        public IDAO IDAO = IDAO.Singleton;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("User_Profile/{id}")]
        public IActionResult User_Profile(int id)

        {
            return View(IDAO.GetUserByID(id));
        }

        [HttpGet("User_Edit/{id}")]
        public IActionResult User_Edit(int id)
        {
            return View(IDAO.GetUserByID(id));
        }

        [HttpPost]
        public IActionResult Editor([FromForm(Name = "name")] string name, [FromForm(Name = "email")] string email, [FromForm(Name = "password")] string password, [FromForm(Name = "birthdate")] string birth,
            [FromForm(Name = "gender")] string gender, [FromForm(Name = "wallet")] int wallet, [FromForm(Name = "introduction")] string introduction, [FromForm(Name = "id")] int id)
        {
            bool? genderValue = null;
            if (gender == "Male")
                genderValue = true;
            else if (gender == "Female")
                genderValue = false;

            IDAO.EditUser(id, name, email, password, birth, genderValue, wallet, introduction);
            return View("User_Profile", IDAO.GetUserByID(id));
        }
    }
}