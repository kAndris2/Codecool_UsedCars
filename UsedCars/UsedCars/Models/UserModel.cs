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
        public bool? Gender { get; private set; }
        public DateTime Birth_Date { get; }
        public String Name { get; private set; }
        public String Email { get; private set; }
        public String Rank { get; private set; }
        public String Password { get; private set; }
        public String Introduction { get; private set; }

        public List<VehicleModel> Vehicles = new List<VehicleModel>();
        public List<ShopModel> Shops = new List<ShopModel>();
        public List<PictureModel> Pictures = new List<PictureModel>();
        public List<CommentModel> Comments = new List<CommentModel>();

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

        public DateTime GetDate() { return new DateTime(1970, 1, 1).AddMilliseconds(double.Parse(Registration_Date.ToString())); }

        public void IncreaseWalletAmount(int amount) { Wallet += amount; }
        public void DecreaseWalletAmount(int amount) { Wallet -= amount; }
        public void IncreaseViews() { Views++; }

        public void SetGender(bool gender) { Gender = gender; }
        public void SetName(string new_name) { Name = new_name; }
        public void SetEmail(string new_email) { Email = new_email; }
        public void SetPassword(string new_password) { Password = new_password; }
        public void SetRank(string new_rank) { Rank = new_rank; }
        public void SetIntroduction(string new_intro) { Introduction = new_intro; }

        public String GetFirstPicture() { return Pictures.Count == 0 ? null : Pictures[0].Route; }

        public void AddVehicle(VehicleModel vehicle) { Vehicles.Add(vehicle); }
        public void AddShop(ShopModel shop) { Shops.Add(shop); }
        public void AddPicture(PictureModel picture) { Pictures.Add(picture); }
        public void AddComment(CommentModel comment) { Comments.Add(comment); }

        public void RemoveShop(ShopModel shop) { Shops.Remove(shop); }
    }
}
