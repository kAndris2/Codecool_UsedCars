using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsedCars.Models;

namespace UsedCars.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GarageController : ControllerBase
    {
        IDAO IDAO = IDAO.Singleton;

        [HttpGet("GetShopByID/{id}")]
        public ShopModel GetShopByID(int id)
        {
            return IDAO.GetShopByID(id);
        }

        [HttpGet("GetVehicles/{id}")]
        public List<VehicleModel> GetVehicles(int id)
        {
            List<VehicleModel> vehicles = new List<VehicleModel>();
            foreach(VehicleModel vehicle in IDAO.GetVehicles(true))
            {
                if (vehicle.Shop_ID.Equals(id))
                    vehicles.Add(vehicle);
            }
            return vehicles;
        }

        [HttpGet("GetVehicleFirstPicture/{id}")]
        public PictureModel GetVehicleFirstPicture(int id)
        {
            foreach (PictureModel picture in IDAO.GetPictures())
            {
                if (picture.Vehicle_ID.Equals(id))
                    return picture;
            }
            return null;
        }

        [HttpGet("GetCurrentUser")]
        public UserModel GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
                return IDAO.GetUserByID(int.Parse($"{User.FindFirst("Id").Value}"));
            return null;
        }

        [HttpPost("CreateVehicle")]
        public VehicleModel CreateVehicle([FromBody] VehicleModel vehicle)
        {
            return IDAO.CreateVehicle
            (
                vehicle.Brand,
                vehicle.Model,
                vehicle.Type,
                vehicle.Fuel,
                string.Join(",", vehicle.Type_Designation),
                vehicle.Odometer,
                vehicle.Vintage,
                vehicle.Validity,
                vehicle.Price,
                vehicle.Cylinder_Capacity,
                vehicle.Performance,
                (int)vehicle.Shop_ID,
                vehicle.Description
            );
        }
    }
}