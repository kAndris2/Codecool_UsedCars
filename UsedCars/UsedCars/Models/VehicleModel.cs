using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedCars.Models
{
    public class VehicleModel
    {
        public String Brand { get; set; }
        public String Model { get; set; }
        public String Type { get; set; }
        public String Fuel { get; set; }
        public String Description { get; set; }
        public String Type_Designation { get; set; }
        public int ID { get; }
        public int Vintage { get; set; }
        public int Price { get; set; }
        public int Cylinder_Capacity { get; set; }
        public int Performance { get; set; }
        public int Odometer { get; set; }
        public int Views { get; set; }
        public int Votes { get; set; }
        public bool Validity { get; set; }
        public int? Shop_ID { get; set; }
        public int? User_ID { get; set; }
        public long Registration_Date { get; }

        public List<PictureModel> Pictures = new List<PictureModel>();
        public List<CommentModel> Comments = new List<CommentModel>();
        public List<LikeModel> Likes = new List<LikeModel>();

        public VehicleModel(int id, string brand, string model, int vintage, string type, int price, string fuel, string type_des, int cylinder, int performance, int odometer, string description, bool validity, int? shopid, int? userid, long register)
        {
            ID = id;
            Brand = brand;
            Model = model;
            Vintage = vintage;
            Type = type;
            Price = price;
            Fuel = fuel;
            Type_Designation = type_des;
            Cylinder_Capacity = cylinder;
            Performance = performance;
            Odometer = odometer;
            Description = description;
            Validity = validity;
            Shop_ID = shopid;
            User_ID = userid;
            Registration_Date = register;
        }

        public VehicleModel(int id, string brand, string model, int vintage, string type, int price, string fuel, string type_des, int cylinder, int performance, int odometer, string description, int? shopid, int? userid, int views, int votes, bool validity, long register)
        {
            ID = id;
            Brand = brand;
            Model = model;
            Vintage = vintage;
            Type = type;
            Price = price;
            Fuel = fuel;
            Type_Designation = type_des;
            Cylinder_Capacity = cylinder;
            Performance = performance;
            Odometer = odometer;
            Description = description;
            Shop_ID = shopid;
            User_ID = userid;
            Views = views;
            Votes = votes;
            Validity = validity;
            Registration_Date = register;
        }

        public VehicleModel() { }

        public DateTime GetDate() { return new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(Registration_Date.ToString())); }
        public String GetFirstPicture() { return Pictures.Count == 0 ? null : Pictures[0].Route; }
        public String GetValidity() { return $"{(Validity == true ? "Valid" : "Invalid")}"; }

        public void IncreaseViews() { Views++; }
        public void IncreaseVotes() { Votes++; }
        public void DecreaseVotes() { Votes--; }

        public void Update(string brand, string model, string type, string fuel, string type_des, int odometer, int vintage, bool validity, int price, int cylinder, int performance, string description)
        {
            Brand = brand;
            Model = model;
            Type = type;
            Fuel = fuel;
            Odometer = odometer;
            Vintage = vintage;
            Validity = validity;
            Price = price;
            Cylinder_Capacity = cylinder;
            Performance = performance;
            Description = description;
            Type_Designation = type_des;
        }

        public void AddPicture(PictureModel picture) { Pictures.Add(picture); }

        public void AddComment(CommentModel comment) { Comments.Add(comment); }
        public void RemoveComment(CommentModel comment) { Comments.Remove(comment); }

        public void AddLike(LikeModel like) { Likes.Add(like); }
        public void RemoveLike(LikeModel like) { Likes.Remove(like); }

        public void Buyed(int userid)
        {
            Shop_ID = null;
            User_ID = userid;
        }
    }
}
