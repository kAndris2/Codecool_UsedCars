using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsedCars.Models;

namespace UsedCars.Controllers
{
    public class CommentController : Controller
    {
        IDAO IDAO = IDAO.Singleton;

        [HttpGet("Edit_Comment/{id}")]
        public IActionResult Edit_Comment(int id)
        {
            return View(IDAO.GetCommentByID(id));
        }

        [HttpPost]
        public IActionResult Delete_Comment([FromForm(Name = "commentid")] int commentid)
        {
            CommentModel comment = IDAO.GetCommentByID(commentid);
            IDAO.Delete("comments", commentid);

            if (comment.User_ID != null)
                return RedirectToAction("Index", "User", new { id = comment.User_ID });
            else if (comment.Shop_ID != null)
                return RedirectToAction("Index", "Shop", comment.Shop_ID);
            else
                return RedirectToAction("Index", "Vehicle", comment.Vehicle_ID);
        }
    }
}