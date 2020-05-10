using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedCars.Models
{
    public class ShopModel
    {
        public int ID { get; }
        public int Owner_ID { get; }
        public int Views { get; private set; }
        public String Name { get; private set; }
        public String Address { get; private set; }
        public String Description { get; private set; }
        public String WebPage { get; private set; }
        public long Registration_Date { get; }

        public List<PictureModel> Pictures = new List<PictureModel>();
        public List<CommentModel> Comments = new List<CommentModel>();
        public List<PurchaseModel> Purchases = new List<PurchaseModel>();
        public List<VehicleModel> Vehicles = new List<VehicleModel>();

        public ShopModel(int id, string name, string address, int ownerid, long regist)
        {
            ID = id;
            Name = name;
            Owner_ID = ownerid;
            Address = address;
            Registration_Date = regist;
        }

        public ShopModel(int id, string name, string description, int ownerid, string address, int views, long regist, string webpage)
        {
            ID = id;
            Name = name;
            Description = description;
            Owner_ID = ownerid;
            Address = address;
            Views = views;
            Registration_Date = regist;
            WebPage = webpage;
        }

        public DateTime GetDate() { return new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(Registration_Date.ToString())); }

        public void IncreaseViews() { Views++; }
        
        public void Update(string name, string address, string description, string webpage)
        {
            Name = name;
            Address = address;
            Description = description;
            WebPage = webpage;
        }

        public String GetFirstPicture() { return Pictures.Count == 0 ? null : Pictures[0].Route; }

        public void AddPicture(PictureModel picture) { Pictures.Add(picture); }
        public void AddComment(CommentModel comment) { Comments.Add(comment); }
        public void AddPurchase(PurchaseModel purchase) { Purchases.Add(purchase); }
        public void AddVehicle(VehicleModel vehicle) { Vehicles.Add(vehicle); }

        public void RemoveVehicle(VehicleModel vehicle) { Vehicles.Remove(vehicle); }
    }
}
