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
        public DateTime Foundation_Date { get; private set; }
        public String Name { get; private set; }
        public String Address { get; private set; }
        public String Description { get; private set; }

        public List<PictureModel> Pictures = new List<PictureModel>();
        public List<CommentModel> Comments = new List<CommentModel>();
        public List<PurchaseModel> Purchases = new List<PurchaseModel>();

        public ShopModel(int id, string name, int ownerid, string address)
        {
            ID = id;
            Name = name;
            Owner_ID = ownerid;
            Address = address;
        }

        public ShopModel(int id, string name, string description, DateTime foundation_date, int ownerid, string address, int views)
        {
            ID = id;
            Name = name;
            Description = description;
            Foundation_Date = foundation_date;
            Owner_ID = ownerid;
            Address = address;
            Views = views;
        }

        public void IncreaseViews() { Views++; }
        
        public void SetFoundationDate(DateTime new_date) { Foundation_Date = new_date; }
        public void SetName(string new_name) { Name = new_name; }
        public void SetAddress(string new_address) { Address = new_address; }
        public void SetDescription(string text) { Description = text; }

        public String GetFirstPicture() { return Pictures.Count == 0 ? null : Pictures[0].Route; }

        public void AddPicture(PictureModel picture) { Pictures.Add(picture); }
        public void AddComment(CommentModel comment) { Comments.Add(comment); }
        public void AddPurchase(PurchaseModel purchase) { Purchases.Add(purchase); }
    }
}
