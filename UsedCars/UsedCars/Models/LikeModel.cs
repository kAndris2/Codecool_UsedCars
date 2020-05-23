using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedCars.Models
{
    public class LikeModel
    {
        public int ID { get; }
        public int Owner_ID { get; }
        public long Submission_Time { get; }
        public int? User_ID { get; }
        public int? Shop_ID { get; }
        public int? Vehicle_ID { get; }
        public int? Comment_ID { get; }

        public LikeModel(int id, int ownerid, long date, int? userid, int? shopid, int? vehicleid, int? commentid)
        {
            ID = id;
            Owner_ID = ownerid;
            Submission_Time = date;
            User_ID = userid;
            Shop_ID = shopid;
            Vehicle_ID = vehicleid;
            Comment_ID = commentid;
        }

        public DateTime GetDate() { return new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(Submission_Time.ToString())); }
    }
}
