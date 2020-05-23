using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedCars.Models
{
    public class UserModel
    {
        public int ID { get; }
        public int Wallet { get; private set; }
        public int Views { get; private set; }
        public long Registration_Date { get; }
        public long Birth_Date { get; private set; }
        public bool? Gender { get; private set; }
        public String Name { get; private set; }
        public String Email { get; private set; }
        public String Rank { get; private set; }
        public String Password { get; private set; }
        public String Introduction { get; private set; }

        public List<VehicleModel> Vehicles = new List<VehicleModel>();
        public List<ShopModel> Shops = new List<ShopModel>();
        public List<PictureModel> Pictures = new List<PictureModel>();
        public List<CommentModel> Comments = new List<CommentModel>();
        public List<LikeModel> Likes = new List<LikeModel>();

        public UserModel(int id, string name, long registration_date, string email, string password)
        {
            ID = id;
            Name = name;
            Registration_Date = registration_date;
            Email = email;
            Password = password;
        }

        public UserModel(int id, string name, long registration_date, bool? gender, /*DateTime birth_date,*/ string email, string password, int wallet, string rank, int views, string introduction)
        {
            ID = id;
            Name = name;
            Registration_Date = registration_date;
            Gender = gender;
            //Birth_Date = birth_date;
            Email = email;
            Password = password;
            Wallet = wallet;
            Rank = rank;
            Views = views;
            Introduction = introduction;
        }

        public String GetDate(long date) 
        {
            string result = new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(date.ToString())).ToString();
            if (result != "1/1/1970 12:00:00 AM")
                return result;
            return null;
        }

        public void DecreaseWalletAmount(int amount) { Wallet -= amount; }
        public void IncreaseViews() { Views++; }

        public String GetFirstPicture() { return Pictures.Count == 0 ? null : Pictures[0].Route; }

        public void AddVehicle(VehicleModel vehicle) { Vehicles.Add(vehicle); }
        public void RemoveVehicle(VehicleModel vehicle) { Vehicles.Remove(vehicle); }

        public void AddShop(ShopModel shop) { Shops.Add(shop); }
        public void RemoveShop(ShopModel shop) { Shops.Remove(shop); }

        public void AddLike(LikeModel like) { Likes.Add(like); }
        public void RemoveLike(LikeModel like) { Likes.Remove(like); }

        public void AddComment(CommentModel comment) { Comments.Add(comment); }
        public void RemoveComment(CommentModel comment) { Comments.Remove(comment); }

        public void AddPicture(PictureModel picture) { Pictures.Add(picture); }

        public void Update(string name, string email, string password, long milisec, bool? gender, int wallet, string introduction)
        {
            Name = name;
            Email = email;
            Password = password;
            Birth_Date = milisec;
            Gender = gender;
            Wallet += wallet;
            Introduction = introduction;
        }
    }
}
