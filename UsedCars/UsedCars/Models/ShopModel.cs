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
        public long Registration_Date { get; }

        public List<PictureModel> Pictures = new List<PictureModel>();
        public List<CommentModel> Comments = new List<CommentModel>();
        public List<PurchaseModel> Purchases = new List<PurchaseModel>();

        public ShopModel(int id, string name, string address, int ownerid, long regist)
        {
            ID = id;
            Name = name;
            Owner_ID = ownerid;
            Address = address;
            Registration_Date = regist;
        }

        public ShopModel(int id, string name, string description, /*DateTime foundation_date,*/ int ownerid, string address, int views)
        {
            ID = id;
            Name = name;
            Description = description;
            //Foundation_Date = foundation_date;
            Owner_ID = ownerid;
            Address = address;
            Views = views;
        }

        public DateTime GetDate() { return new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(Registration_Date.ToString())); }

        public void IncreaseViews() { Views++; }
        
        public void SetFoundationDate(DateTime new_date) { Foundation_Date = new_date; }
        public void Update(string name, string address, string description)
        {
            Name = name;
            Address = address;
            Description = description;
        }

        public String GetFirstPicture() { return Pictures.Count == 0 ? null : Pictures[0].Route; }

        public void AddPicture(PictureModel picture) { Pictures.Add(picture); }
        public void AddComment(CommentModel comment) { Comments.Add(comment); }
        public void AddPurchase(PurchaseModel purchase) { Purchases.Add(purchase); }
    }
}
