using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedCars.Models
{
    public class VehicleModel
    {
        public String Brand { get; }
        public String Model { get; }
        public String Type { get; }
        public String Fuel { get; }
        public String Description { get; private set; }
        public int ID { get; }
        public int Vintage { get; }
        public int Price { get; private set; }
        public int Cylinder_Capacity { get; }
        public int Odometer { get; private set; }
        public int Views { get; private set; }
        public int Votes { get; private set; }
        public bool Validity { get; }
        public int? Shop_ID { get; }
        public int? User_ID { get; }
        public long Registration_Date { get; }

        public List<PictureModel> Pictures = new List<PictureModel>();
        public List<CommentModel> Comments = new List<CommentModel>();

        public VehicleModel(int id, string brand, string model, int vintage, string type, int price, string fuel, int cylinder, int odometer, string description, bool validity, int? shopid, int? userid, long register)
        {
            ID = id;
            Brand = brand;
            Model = model;
            Vintage = vintage;
            Type = type;
            Price = price;
            Fuel = fuel;
            Cylinder_Capacity = cylinder;
            Odometer = odometer;
            Description = description;
            Validity = validity;
            Shop_ID = shopid;
            User_ID = userid;
            Registration_Date = register;
        }

        public VehicleModel(int id, string brand, string model, int vintage, string type, int price, string fuel, int cylinder, int odometer, string description, int? shopid, int? userid, int views, int votes, bool validity, long register)
        {
            ID = id;
            Brand = brand;
            Model = model;
            Vintage = vintage;
            Type = type;
            Price = price;
            Fuel = fuel;
            Cylinder_Capacity = cylinder;
            Odometer = odometer;
            Description = description;
            Shop_ID = shopid;
            User_ID = userid;
            Views = views;
            Votes = votes;
            Validity = validity;
            Registration_Date = register;
        }

        public DateTime GetDate() { return new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(Registration_Date.ToString())); }

        public void IncreaseViews() { Views++; }
        public void IncreaseVotes() { Votes++; }
        public void DecreaseVotes() { Votes--; }

        public void SetDescription(string description) { Description = description; }
        public void SetPrice(int new_price) { Price = new_price; }
        public void SetOdometer(int value) { Odometer = value; }
        
        public String GetFirstPicture() { return Pictures.Count == 0 ? null : Pictures[0].Route; }

        public void AddPicture(PictureModel picture) { Pictures.Add(picture); }
        public void AddComment(CommentModel comment) { Comments.Add(comment); }
    }
}
