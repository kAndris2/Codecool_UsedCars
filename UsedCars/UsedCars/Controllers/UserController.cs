using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsedCars.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IDAO IDAO = IDAO.Singleton;

        public ActionResult Index([FromRoute(Name = "id")] int id)
        {
            return View("User_Profile", IDAO.GetUserByID(id));
        }

        [AllowAnonymous]
        [HttpGet("User_Profile/{id}")]
        public IActionResult User_Profile(int id)

        {
            return View(IDAO.GetUserByID(id));
        }

        [AllowAnonymous]
        public IActionResult User_List()
        {
            return View(IDAO);
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

        [HttpPost]
        public IActionResult User_Comment([FromForm(Name = "title")] string title, [FromForm(Name = "message")] string message, [FromForm(Name = "ownerid")] int ownerid, [FromForm(Name = "userid")] int userid)
        {
            IDAO.CreateComment("user", userid, title, message, ownerid);
            return View("User_Profile", IDAO.GetUserByID(userid));
        }

        public IActionResult User_Garage([FromForm(Name = "userid")] int userid)
        {
            return View(IDAO.GetUserByID(userid));
        }
    }
}