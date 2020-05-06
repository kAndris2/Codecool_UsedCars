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
        public List<VehicleModel> FoundVehicles = new List<VehicleModel>();

        private IDAO()
        {
            LoadFiles("all");
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

        public UserModel GetUserByID(int id)
        {
            foreach (UserModel user in Users)
            {
                if (user.ID.Equals(id))
                    return user;
            }
            throw new ArgumentException($"Invalid User ID! - ('{id}')");
        }

        public UserModel GetUserByEmail(string email)
        {
            foreach (UserModel user in Users)
            {
                if (user.Email.Equals(email))
                    return user;
            }
            throw new ArgumentException($"Invalid User email! - ('{email}')");
        }

        public String GetLastIDFromTable(NpgsqlConnection connection, string table)
        {
            string value = "";
            using (var cmd = new NpgsqlCommand($"SELECT * FROM {table}", connection))
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    value = reader["id"].ToString();
                }
            }
            return value;
        }

        public Object GiveDBNull(int? value)
        {
            return DBNull.Value;
        }

        //-Comment FUNCTIONS------------------------------------------------------------------------------------------------
        public void CreateComment(string table_name, int? recordid, string title, string message, int? ownerid)
        {
            int id = 0;
            long milisec = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            string sqlstr = "INSERT INTO comments " +
                                "(title, message, submission_time, owner_id, user_id, vehicle_id, shop_id) " +
                                "VALUES " +
                                    "(@title, @message, @date, @ownerid, @userid, @vehicleid, @shopid)";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("title", title);
                    cmd.Parameters.AddWithValue("message", message);
                    cmd.Parameters.AddWithValue("date", milisec);
                    cmd.Parameters.AddWithValue("ownerid", ownerid);
                    cmd.Parameters.AddWithValue("userid", table_name == "user" ? ownerid : GiveDBNull(null));
                    cmd.Parameters.AddWithValue("vehicleid", table_name == "vehicle" ? ownerid : GiveDBNull(null));
                    cmd.Parameters.AddWithValue("shopid", table_name == "shop" ? ownerid : GiveDBNull(null));
                    cmd.ExecuteNonQuery();
                }
                id = int.Parse(GetLastIDFromTable(conn, "shops"));
            }

            CommentModel comment = new CommentModel
            (
                id,
                int.Parse($"{ownerid}"),
                title,
                message,
                milisec,
                table_name == "user" ? recordid : null,
                table_name == "vehicle" ? recordid : null,
                table_name == "shop" ? recordid : null
            );
            AddCommentTo(comment, comment.User_ID, comment.Vehicle_ID, comment.Shop_ID);
        }

        private void AddCommentTo(CommentModel comment, int? userid, int? vehicleid, int? shopid)
        {
            if (userid != null)
            {
                GetUserByID(int.Parse($"{userid}")).AddComment(comment);
            }
            else if (vehicleid != null)
            {
                foreach (UserModel user in Users)
                {
                    foreach (VehicleModel vehicle in user.Vehicles)
                    {
                        if (vehicle.ID.Equals(vehicleid))
                            vehicle.AddComment(comment);
                    }
                }
            }
            else if (shopid != null)
            {
                foreach (UserModel user in Users)
                {
                    foreach (ShopModel shop in user.Shops)
                    {
                        if (shop.ID.Equals(shopid))
                            shop.AddComment(comment);
                    }
                }
            }
        }

        //-Picture FUNCTIONS------------------------------------------------------------------------------------------------
        public void CreatePicture(string route, string table_name, int? ownerid)
        {
            int id = 0;
            long milisec = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            string sqlstr = "INSERT INTO pictures " +
                                "(route, user_id, vehicle_id, shop_id) " +
                                "VALUES " +
                                    "(@route, @userid, @vehicleid, @shopid)";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("route", route);
                    
                    if (table_name == "user")
                        cmd.Parameters.AddWithValue("userid", ownerid);
                    else
                        cmd.Parameters.AddWithValue("userid", DBNull.Value);

                    if (table_name == "vehicle")
                        cmd.Parameters.AddWithValue("vehicleid", ownerid);
                    else
                        cmd.Parameters.AddWithValue("vehicleid", DBNull.Value);

                    if (table_name == "shop")
                        cmd.Parameters.AddWithValue("shopid", ownerid);
                    else
                        cmd.Parameters.AddWithValue("shopid", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
                id = int.Parse(GetLastIDFromTable(conn, "pictures"));
            }

            PictureModel picture = new PictureModel
            (
                id,
                route,
                table_name == "user" ? ownerid : null,
                table_name == "vehicle" ? ownerid : null,
                table_name == "shop" ? ownerid : null
            );

            AddPictureTo(picture, picture.User_ID, picture.Vehicle_ID, picture.Shop_ID);
        }

        private void AddPictureTo(PictureModel picture, int? userid, int? vehicleid, int? shopid)
        {
            if (userid != null)
            {
                GetUserByID(int.Parse($"{userid}")).AddPicture(picture);
            }
            else if (vehicleid != null)
            {
                foreach (UserModel user in Users)
                {
                    foreach (VehicleModel vehicle in user.Vehicles)
                    {
                        if (vehicle.ID.Equals(vehicleid))
                            vehicle.AddPicture(picture);
                    }
                }
            }
            else if (shopid != null)
            {
                foreach (UserModel user in Users)
                {
                    foreach (ShopModel shop in user.Shops)
                    {
                        if (shop.ID.Equals(shopid))
                            shop.AddPicture(picture);
                    }
                }
            }
        }

        //-SHOP FUNCTIONS---------------------------------------------------------------------------------------------------
        public ShopModel GetShopByID(int id)
        {
            foreach (UserModel user in Users)
            {
                foreach (ShopModel shop in user.Shops)
                {
                    if (shop.ID.Equals(id))
                        return shop;
                }
            }
            throw new ArgumentException($"Invalid Shop ID! - ('{id}')");
        }

        public void DeleteShop(int id)
        {
            ShopModel shop = GetShopByID(id);
            UserModel user = GetUserByID(shop.Owner_ID);
            user.RemoveShop(shop);

            string sqlstr = "DELETE FROM shops " +
                           "WHERE id = @id";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EditShop(string name, string address, string description, int id)
        {
            GetShopByID(id).Update(name, address, description);
            string sqlstr = "UPDATE shops " +
                           "SET name = @name, address = @address, description = @description " +
                           "WHERE id = @id";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("address", address);
                    cmd.Parameters.AddWithValue("description", description);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateShop(string name, string address, string email)
        {
            UserModel owner = GetUserByEmail(email);
            int id = 0;
            long milisec = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            string sqlstr = "INSERT INTO shops " +
                                "(name, address, owner_id, registration_date) " +
                                "VALUES " +
                                    "(@name, @address, @ownerid, @date)";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("address", address);
                    cmd.Parameters.AddWithValue("ownerid", owner.ID);
                    cmd.Parameters.AddWithValue("date", milisec);
                    cmd.ExecuteNonQuery();
                }
                id = int.Parse(GetLastIDFromTable(conn, "shops"));
            }
            owner.AddShop(new ShopModel(id, name, address, owner.ID, milisec));
        }

        public void UpdateViews(string table, int id)
        {
            ShopModel shop = GetShopByID(id);
            shop.IncreaseViews();

            string sqlstr = $"UPDATE {table} " +
                           "SET views = @views " +
                           "WHERE id = @id";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("views", shop.Views);
                    cmd.ExecuteNonQuery();
                }
            }
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

        private void LoadFiles(string run)
        {
            if (run == "all" || run == "users")
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

            if (run == "all" || run == "shops")
            {
                using (var conn = new NpgsqlConnection(Program.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM shops", conn))
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ShopModel shop = new ShopModel
                            (
                            int.Parse(reader["id"].ToString()),
                            reader["name"].ToString(),
                            reader["description"].ToString(),
                            int.Parse(reader["owner_id"].ToString()),
                            reader["address"].ToString(),
                            int.Parse(reader["views"].ToString())
                            );
                            GetUserByID(shop.Owner_ID).AddShop(shop);
                        }
                    }
                }
            }

            if (run == "all" || run == "pictures")
            {
                using (var conn = new NpgsqlConnection(Program.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM pictures", conn))
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            int? userid, vehicleid, shopid;
                            /////////////////////////////////////////////////////////////////
                            if (!int.TryParse(reader["user_id"].ToString(), out int x))
                                userid = null;
                            else
                                userid = int.Parse(reader["user_id"].ToString());
                            //---------------------------------------------------------------
                            if (!int.TryParse(reader["vehicle_id"].ToString(), out int y))
                                vehicleid = null;
                            else
                                vehicleid = int.Parse(reader["vehicle_id"].ToString());
                            //---------------------------------------------------------------
                            if (!int.TryParse(reader["shop_id"].ToString(), out int z))
                                shopid = null;
                            else
                                shopid = int.Parse(reader["shop_id"].ToString());
                            /////////////////////////////////////////////////////////////////
                            PictureModel picture = new PictureModel
                            (
                            int.Parse(reader["id"].ToString()),
                            reader["route"].ToString(),
                            userid,
                            vehicleid,
                            shopid
                            );
                            AddPictureTo(picture, userid, vehicleid, shopid);
                        }
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
