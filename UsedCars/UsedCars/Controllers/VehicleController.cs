using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UsedCars.Controllers
{
    public class VehicleController : Controller
    {
        IDAO IDAO = IDAO.Singleton;

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create_Vehicle([FromForm(Name = "brand")] string brand, [FromForm(Name = "model")] string model, [FromForm(Name = "type")] string type, [FromForm(Name = "fuel")] string fuel,
            [FromForm(Name = "odometer")] int odometer, [FromForm(Name = "year")] int year, [FromForm(Name = "validity")] string validity, [FromForm(Name = "price")] int price,
            [FromForm(Name = "cylinder")] int cylinder, [FromForm(Name = "shopid")] int shopid, [FromForm(Name = "description")] string description)
        {
            IDAO.CreateVehicle(brand,
                               model,
                               type,
                               fuel,
                               odometer,
                               year,
                               validity == "Valid",
                               price,
                               cylinder,
                               shopid,
                               description);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Vehicle_Profile/{id}")]
        public IActionResult Vehicle_Profile(int id)
        {
            return View(IDAO.GetVehicleByID(id));
        }

        [HttpGet("Vehicle_Edit/{id}")]
        public IActionResult Vehicle_Edit(int id)
        {
            return View(IDAO.GetVehicleByID(id));
        }

        [HttpPost]
        public IActionResult Editor([FromForm(Name = "brand")] string brand, [FromForm(Name = "model")] string model, [FromForm(Name = "type")] string type, [FromForm(Name = "fuel")] string fuel,
            [FromForm(Name = "odometer")] int odometer, [FromForm(Name = "year")] int year, [FromForm(Name = "validity")] string validity, [FromForm(Name = "price")] int price,
            [FromForm(Name = "cylinder")] int cylinder, [FromForm(Name = "description")] string description, [FromForm(Name = "id")] int id)
        {
            IDAO.EditVehicle(id,
                             brand,
                             model,
                             type,
                             fuel,
                             odometer,
                             year,
                             validity == "Valid",
                             price,
                             cylinder,
                             description);
            return View("Vehicle_Profile", IDAO.GetVehicleByID(id));
        }

        [HttpGet("Delete_Vehicle/{id}")]
        public IActionResult Delete_Vehicle(int id)
        {
            IDAO.Delete("vehicles", id);
            return RedirectToAction("Index", "Home");
        }
    }
}