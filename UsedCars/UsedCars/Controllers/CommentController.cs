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

        public IActionResult Delete_Comment([FromRoute(Name = "commentid")] int commentid)
        {
            IDAO.Delete("comments", commentid);
            return CheckReturn(IDAO.GetCommentByID(commentid));
        }

        [HttpPost]
        public IActionResult Editor([FromForm(Name = "title")] string title, [FromForm(Name = "message")] string message, [FromForm(Name = "id")] int commentid)
        {
            IDAO.EditComment(commentid, title, message);
            return CheckReturn(IDAO.GetCommentByID(commentid));
        }

        private IActionResult CheckReturn(CommentModel comment)
        {
            if (comment.User_ID != null)
                return RedirectToAction("User_Profile", "User", new { id = comment.User_ID });
            else if (comment.Shop_ID != null)
                return RedirectToAction("Shop_Profile", "Shop", new { id = comment.Shop_ID });
            else
                return RedirectToAction("Vehicle_Profile", "Vehicle", new { id = comment.Vehicle_ID });
        }


    }
}