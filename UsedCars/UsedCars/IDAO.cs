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
        public List<ShopModel> Shops = new List<ShopModel>();
        public List<VehicleModel> FoundVehicles = new List<VehicleModel>();

        private IDAO()
        {
            LoadFiles();
        }

        public String GetValueCount(string title, string value)
        {
            int count = 0;
            foreach (UserModel user in Users)
            {
                foreach (VehicleModel vehicle in user.Vehicles)
                {
                    if (value == "brand" && vehicle.Brand.Equals(title))
                        count++;
                    else if (value == "model" && vehicle.Model.Equals(title))
                        count++;
                    else if (value == "type" && vehicle.Type.Equals(title))
                        count++;
                    else if (value == "fuel" && vehicle.Fuel.Equals(title))
                        count++;
                }
            }
            return $"{title}({count})";
        }

        public ShopModel GetShopByID(int id)
        {
            foreach (ShopModel shop in Shops)
            {
                if (shop.ID.Equals(id))
                    return shop;
            }
            throw new ArgumentException($"Invalid Shop ID! - ('{id}')");
        }

        public UserModel GetUserByID(int id)
        {
            foreach (UserModel user in Users)
            {
                if (user.ID.Equals(id))
                    return user;
            }
            throw new ArgumentException($"Invalid User ID! - ('{id}')");
        }

        public void Register(string name, string email, string password)
        {
            int id = 0;
            long milisec = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            string sqlstr = "INSERT INTO users " +
                                "(name, registration_date, email, password, rank) " +
                                "VALUES " +
                                    "(@name, @date, @email, @password, @rank)";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("date", milisec);
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.Parameters.AddWithValue("password", password);
                    cmd.Parameters.AddWithValue("rank", "member");
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = new NpgsqlCommand("SELECT * FROM users", conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        id = int.Parse(reader["id"].ToString());
                    }
                }
            }
            Users.Add(new UserModel(id, name, milisec, email, password));
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
