using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedCars.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace UsedCars.Controllers
{
    [Route("[controller]")]
    public class LikeController
    {
        IDAO Singleton = IDAO.Singleton;

        [HttpPost("Create")]
        public void Create(string[] likedata)
        {
            Singleton.CreateLike(int.Parse(likedata[0]), int.Parse(likedata[1]), likedata[2]);
        }

        [HttpPost("Delete")]
        public void Delete(string[] likedata)
        {
            Singleton.DeleteLike(int.Parse(likedata[0]), int.Parse(likedata[1]), likedata[2]);
        }

        [HttpPost("GetComments")]
        public List<CommentModel> GetComments()
        {
            return Singleton.GetComments();
        }

        [HttpPost("GetLikes")]
        public List<LikeModel> GetLikes()
        {
            return Singleton.GetLikes();
        }
    }
}
