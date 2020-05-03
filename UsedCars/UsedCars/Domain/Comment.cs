using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedCars.Domain
{
    public class Comment
    {
        public int ID { get; }
        public String Title { get; }
        public String Message { get; }
        public long Submission_Time { get; }
        public int? User_ID { get; }
        public int? Vehicle_ID { get; }
        public int? Shop_ID { get; }

        public Comment(int id, string title, string message, long submission_time, int? userid, int? vehicleid, int? shopid)
        {
            ID = id;
            Title = title;
            Message = message;
            Submission_Time = submission_time;
            User_ID = userid;
            Vehicle_ID = vehicleid;
            Shop_ID = shopid;
        }
    }
}
