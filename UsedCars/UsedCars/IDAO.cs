using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedCars.Models;

namespace UsedCars
{
    public sealed class IDAO
    {
        static IDAO instance = null;
        public static IDAO Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new IDAO();
                }
                return instance;
            }
        }

        public List<UserModel> Users = new List<UserModel>();
        public List<VehicleModel> Vehicles = new List<VehicleModel>();
        public List<ShopModel> Shops = new List<ShopModel>();
        public List<CommentModel> Comments = new List<CommentModel>();
        public List<PurchaseModel> Purchases = new List<PurchaseModel>();
        public List<PictureModel> Pictures = new List<PictureModel>();

        private IDAO()
        {
            //LoadFiles();
        }

        public String GetBrandCount(string brand)
        {
            int count = 0;
            foreach (VehicleModel vehicle in Vehicles)
            {
                if (vehicle.Brand.Equals(brand))
                    count++;
            }
            return $"{brand}({count})";
        }

        public String GetModelCount(string model)
        {
            int count = 0;
            foreach (VehicleModel vehicle in Vehicles)
            {
                if (vehicle.Model.Equals(model))
                    count++;
            }
            return $"{model}({count})";
        }

        private void LoadFiles()
        {
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM users", conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Users.Add
                        (
                            new UserModel
                            (
                            int.Parse(reader["id"].ToString()),
                            reader["name"].ToString(),
                            Convert.ToInt64(reader["registration_date"].ToString()),
                            CheckBoolOrNull(reader["gender"].ToString()),
                            reader["email"].ToString(),
                            reader["password"].ToString(),
                            int.Parse(reader["wallet"].ToString()),
                            reader["rank"].ToString(),
                            int.Parse(reader["views"].ToString()),
                            reader["introduction"].ToString()
                            )
                        );
                    }
                }
            }
        }

        private bool? CheckBoolOrNull(string value)
        {
            if (value == "null")
                return null;
            return value == "True";
        }

        private int? CheckIntOrNull(string value)
        {
            if (value == "null")
                return null;
            return int.Parse(value);
        }
    }
}
