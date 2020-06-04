using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsedCars.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        private IDAO IDAO = IDAO.Singleton;

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Shop_List()
        {
            return View(IDAO);
        }

        public IActionResult Create_Shop([FromForm(Name = "name")] string name, [FromForm(Name = "address")] string address, [FromForm(Name = "userid")] int userid)
        {
            IDAO.CreateShop(name, address, userid);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet("Shop_Profile/{id}")]
        public IActionResult Shop_Profile(int id)
        {
            return View(IDAO.GetShopByID(id));
        }

        [HttpGet("Shop_Edit/{id}")]
        public IActionResult Shop_Edit(int id)
        {
            return View(IDAO.GetShopByID(id));
        }

        public IActionResult Editor([FromForm(Name = "name")] string name, [FromForm(Name = "address")] string address, [FromForm(Name = "description")] string description, [FromForm(Name = "id")] int id, [FromForm(Name = "webpage")] string webpage)
        {
            IDAO.EditShop(id, name, address, description, webpage);
            return View("Shop_Profile", IDAO.GetShopByID(id));
        }

        [HttpGet("Delete_Shop/{id}")]
        public IActionResult Delete_Shop(int id)
        {
            IDAO.Delete("shops", id);
            return View("Shop_List", IDAO);
        }

        [HttpPost]
        public IActionResult Shop_Comment([FromForm(Name = "title")] string title, [FromForm(Name = "message")] string message, [FromForm(Name = "ownerid")] int ownerid, [FromForm(Name = "shopid")] int shopid)
        {
            IDAO.CreateComment("shop", shopid, title, message, ownerid);
            return View("Shop_Profile", IDAO.GetShopByID(shopid));
        }

        [HttpGet("Shop_Garage/{id}")]
        public IActionResult Shop_Garage(int id)
        {
            return View();
        }

        [HttpGet("Shop_Sales/{id}")]
        public IActionResult Shop_Sales(int id)
        {
            return View(IDAO.GetShopByID(id));
        }
    }
}