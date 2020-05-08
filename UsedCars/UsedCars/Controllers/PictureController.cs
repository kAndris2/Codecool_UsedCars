using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UsedCars.Controllers
{
    public class PictureController : Controller
    {
        IDAO IDAO;
        IHostingEnvironment _env;

        public PictureController(IHostingEnvironment environment)
        {
            IDAO = IDAO.Singleton;
            _env = environment;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int id, [FromForm(Name = "type")] string type)
        {

            var imagePath = @"\pics\Uploads\";
            var uploadPath = _env.WebRootPath + imagePath;

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var uniqFileName = Guid.NewGuid().ToString();
            var filename = Path.GetFileName(uniqFileName + "." + file.FileName.Split(".").Last().ToLower());
            string fullPath = uploadPath + filename;

            imagePath = imagePath + @"\";
            string filePath = @".." + Path.Combine(imagePath, filename);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            IDAO.CreatePicture(filePath, type, id);

            if (type == "shop")
                return RedirectToAction("Shop_Profile", "Shop", IDAO.GetShopByID(id));
            else if (type == "vehicle")
                return RedirectToAction("Vehicle_Profile", "Vehicle", IDAO.GetVehicleByID(id));
            else if (type == "user")
                return RedirectToAction("User_Profile", "User", IDAO.GetUserByID(id));

            throw new ArgumentException("Something went wrong at picture upload!");
        }
    }
}