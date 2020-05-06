using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedCars.Models
{
    public class CommentModel
    {
        public int ID { get; }
        public int Owner_ID { get; }
        public String Title { get; }
        public String Message { get; }
        public long Submission_Time { get; }
        public int? User_ID { get; }
        public int? Vehicle_ID { get; }
        public int? Shop_ID { get; }

        public CommentModel(int id, int owenerid, string title, string message, long submission_time, int? userid, int? vehicleid, int? shopid)
        {
            ID = id;
            Owner_ID = owenerid;
            Title = title;
            Message = message;
            Submission_Time = submission_time;
            User_ID = userid;
            Vehicle_ID = vehicleid;
            Shop_ID = shopid;
        }

        public DateTime GetDate() { return new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(Submission_Time.ToString())); }
    }
}
