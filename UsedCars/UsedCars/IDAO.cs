using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        private IDAO()
        {
            LoadFiles();
        }

        //-User FUNCTIONS------------------------------------------------------------------------------------------------
        public String GetFormattedGender(bool? gender)
        {
            if (gender == true)
                return "Male";
            else if (gender == false)
                return "Female";

            return "N/A";
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

        public void EditUser(int id, string name, string email, string password, string birth, bool? gender, int wallet, string introduction)
        {
            UserModel user = GetUserByID(id);
            DateTime date = DateTime.ParseExact(birth.Replace("-", "/"), "yyyy/MM/dd", CultureInfo.InvariantCulture);
            long milisec = (long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds;
            user.Update(name, email, password, milisec, gender, wallet, introduction);

            string sqlstr = "UPDATE users " +
                            "SET name = @name, email = @email, password = @password, birth_date = @birth, gender = @gender, wallet = @wallet, introduction = @introduction " +
                            "WHERE id = @id";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.Parameters.AddWithValue("password", password);
                    cmd.Parameters.AddWithValue("birth", milisec);
                    cmd.Parameters.AddWithValue("gender", gender == null ? GiveDBNull() : gender);
                    cmd.Parameters.AddWithValue("wallet", user.Wallet);
                    cmd.Parameters.AddWithValue("introduction", introduction);
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
                    cmd.Parameters.AddWithValue("rank", "Member");
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

        //-Vehicle FUNCTIONS------------------------------------------------------------------------------------------------
        public void CreateVehicle(string brand, string model, string type, string fuel, string type_des, int odometer, int vintage, bool validity, int price, int cylinder, int performance, int shopid, string description)
        {
            int id = 0;
            long milisec = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            string sqlstr = "INSERT INTO vehicles " +
                                "(brand, model, type, fuel, type_designation, odometer, vintage, validity, price, cylinder_capacity, performance, shop_id, description, registration_date) " +
                                "VALUES " +
                                    "(@brand, @model, @type, @fuel, @type_designation, @odometer, @vintage, @validity, @price, @cylinder, @performance, @shopid, @description, @register)";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("brand", brand);
                    cmd.Parameters.AddWithValue("model", model);
                    cmd.Parameters.AddWithValue("type", type);
                    cmd.Parameters.AddWithValue("fuel", fuel);
                    cmd.Parameters.AddWithValue("type_designation", type_des);
                    cmd.Parameters.AddWithValue("odometer", odometer);
                    cmd.Parameters.AddWithValue("vintage", vintage);
                    cmd.Parameters.AddWithValue("validity", validity);
                    cmd.Parameters.AddWithValue("price", price);
                    cmd.Parameters.AddWithValue("cylinder", cylinder);
                    cmd.Parameters.AddWithValue("performance", performance);
                    cmd.Parameters.AddWithValue("shopid", shopid);
                    cmd.Parameters.AddWithValue("description", description);
                    cmd.Parameters.AddWithValue("register", milisec);
                    cmd.ExecuteNonQuery();
                }
                id = int.Parse(GetLastIDFromTable(conn, "vehicles"));
            }

            VehicleModel vehicle = new VehicleModel
            (
                id,
                brand,
                model,
                vintage,
                type,
                price,
                fuel,
                type_des,
                cylinder,
                performance,
                odometer,
                description,
                validity,
                shopid,
                null,
                milisec
            );
            AddVehicleTo(vehicle, shopid, null);
        }

        public void AddVehicleTo(VehicleModel vehicle, int? shopid, int? userid)
        {
            if (userid != null)
            {
                GetUserByID(int.Parse($"{userid}")).AddVehicle(vehicle);
            }
            else
            {
                foreach (UserModel user in Users)
                {
                    foreach (ShopModel shop in user.Shops)
                    {
                        if (shop.ID.Equals(shopid))
                            shop.AddVehicle(vehicle);
                    }
                }
            }
        }

        public void EditVehicle(int id, string brand, string model, string type, string fuel, string type_des, int odometer, int vintage, bool validity, int price, int cylinder, int performance, string description)
        {
            string sqlstr = "UPDATE vehicles " +
                                "SET brand = @brand, model = @model, type = @type, fuel = @fuel, odometer = @odometer, vintage = @vintage, " +
                                    "validity = @validity, price = @price, cylinder_capacity = @cylinder, performance = @performance, description = @description " +
                                "WHERE id = @id";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("brand", brand);
                    cmd.Parameters.AddWithValue("model", model);
                    cmd.Parameters.AddWithValue("type", type);
                    cmd.Parameters.AddWithValue("fuel", fuel);
                    cmd.Parameters.AddWithValue("odometer", odometer);
                    cmd.Parameters.AddWithValue("vintage", vintage);
                    cmd.Parameters.AddWithValue("validity", validity);
                    cmd.Parameters.AddWithValue("price", price);
                    cmd.Parameters.AddWithValue("cylinder", cylinder);
                    cmd.Parameters.AddWithValue("performance", performance);
                    cmd.Parameters.AddWithValue("description", description);
                    cmd.ExecuteNonQuery();
                }
            }
            GetVehicleByID(id).Update(brand, model, type, fuel, type_des, odometer, vintage, validity, price, cylinder, performance, description);
        }

        public List<VehicleModel> Search(string brand, string model, string type, string fuel, string tdes, string vfrom, string vto, int pfrom, int pto, int ofrom, int oto, int cfrom, int cto)
        {
            List<VehicleModel> Result = new List<VehicleModel>();
            int maxPoints = 13;
            int vehPoints = 0;

            foreach (VehicleModel vehicle in GetVehicles())
            {
                if (brand != "all")
                {
                    if (vehicle.Brand.Equals(brand))
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (model != "all")
                {
                    if (vehicle.Model.Equals(model))
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (type != "all")
                {
                    if (vehicle.Type.Equals(type))
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (fuel != "all")
                {
                    if (vehicle.Fuel.Equals(fuel))
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (tdes != null)
                {
                    if (vehicle.Type_Designation.Contains(tdes))
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (vfrom != "all")
                {
                    if (vehicle.Vintage >= int.Parse(vfrom))
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (vto != "all")
                {
                    if (vehicle.Vintage <= int.Parse(vto))
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (pfrom != 0)
                {
                    if (vehicle.Price >= pfrom)
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (pto != 0)
                {
                    if (vehicle.Price <= pto)
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (ofrom != 0)
                {
                    if (vehicle.Odometer >= ofrom)
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (oto != 0)
                {
                    if (vehicle.Odometer <= pto)
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (cfrom != 0)
                {
                    if (vehicle.Cylinder_Capacity >= cfrom)
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (oto != 0)
                {
                    if (vehicle.Cylinder_Capacity <= cto)
                        vehPoints++;
                }
                else
                    vehPoints++;

                if (vehPoints == maxPoints)
                    Result.Add(vehicle);

                vehPoints = 0;
            }
            return Result;
        }

        private List<VehicleModel> GetVehicles()
        {
            List<VehicleModel> Vehicles = new List<VehicleModel>();

            foreach (UserModel user in Users)
            {
                foreach (ShopModel shop in user.Shops)
                {
                    Vehicles.AddRange(shop.Vehicles);
                }
            }

            return Vehicles;
        }

        public VehicleModel GetVehicleByID(int id)
        {
            foreach (UserModel user in Users)
            {
                if (user.Vehicles.Count >= 1)
                {
                    user.Vehicles.FirstOrDefault(v => v.ID == id);
                }
                foreach (ShopModel shop in user.Shops)
                {
                    if (shop.Vehicles.Count >= 1)
                    {
                        return shop.Vehicles.FirstOrDefault(v => v.ID == id);
                    }
                }
            }
            throw new ArgumentException($"Invalid Vehicle ID! - ('{id}')");
        }

        public String GetCapacity(string value)
        {
            string result = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (i == 0)
                    result += value[i] + ".";
                else if (i == 1)
                {
                    result += value[i];
                    break;
                }
            }
            return result;
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
                    cmd.Parameters.AddWithValue("userid", table_name == "user" ? ownerid : GiveDBNull());
                    cmd.Parameters.AddWithValue("vehicleid", table_name == "vehicle" ? ownerid : GiveDBNull());
                    cmd.Parameters.AddWithValue("shopid", table_name == "shop" ? ownerid : GiveDBNull());
                    cmd.ExecuteNonQuery();
                }
                id = int.Parse(GetLastIDFromTable(conn, "comments"));
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
                    cmd.Parameters.AddWithValue("userid", table_name == "user" ? ownerid : GiveDBNull());
                    cmd.Parameters.AddWithValue("vehicleid", table_name == "vehicle" ? ownerid : GiveDBNull());
                    cmd.Parameters.AddWithValue("shopid", table_name == "shop" ? ownerid : GiveDBNull());
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
                GetVehicleByID(int.Parse($"{vehicleid}")).AddPicture(picture);
            }
            else if (shopid != null)
            {
                GetShopByID(int.Parse($"{shopid}")).AddPicture(picture);
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

        public List<ShopModel> GetShops()
        {
            List<ShopModel> Shops = new List<ShopModel>();

            foreach (UserModel user in Users)
            {
                Shops.AddRange(user.Shops);
            }

            return Shops;
        }

        public void EditShop(int id, string name, string address, string description, string webpage)
        {
            GetShopByID(id).Update(name, address, description, webpage);
            string sqlstr = "UPDATE shops " +
                                "SET name = @name, address = @address, description = @description, webpage = @webpage " +
                            "WHERE id = @id";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("address", address);
                    cmd.Parameters.AddWithValue("description", description);
                    cmd.Parameters.AddWithValue("webpage", webpage);
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

        //-Common FUNCTIONS---------------------------------------------------------------------------------------------------
        public Dictionary<string, int> GetValueCount(string title)
        {
            Dictionary<string, int> data = new Dictionary<string, int>();

            foreach (ShopModel shop in GetShops())
            {
                foreach (VehicleModel vehicle in shop.Vehicles)
                {
                    if (title == "brand")
                    {
                        if (!data.ContainsKey(vehicle.Brand))
                            data.Add(vehicle.Brand, 1);
                        else
                            data[vehicle.Brand]++;
                    }
                    else if (title == "model")
                    {
                        if (!data.ContainsKey(vehicle.Model))
                            data.Add(vehicle.Model, 1);
                        else
                            data[vehicle.Model]++;
                    }
                    else if (title == "type")
                    {
                        if (!data.ContainsKey(vehicle.Type))
                            data.Add(vehicle.Type, 1);
                        else
                            data[vehicle.Type]++;
                    }
                    else if (title == "fuel")
                    {
                        if (!data.ContainsKey(vehicle.Fuel))
                            data.Add(vehicle.Fuel, 1);
                        else
                            data[vehicle.Fuel]++;
                    }
                }
            }
            return data;
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

        public Object GiveDBNull() { return DBNull.Value; }

        public void Delete(string table, int id)
        {
            string sqlstr = $"DELETE FROM {table} " +
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

            if (table == "shops")
            {
                ShopModel shop = GetShopByID(id);
                UserModel user = GetUserByID(shop.Owner_ID);
                user.RemoveShop(shop);
            }
            else if (table == "vehicles")
            {
                foreach (UserModel user in Users)
                {
                    if (user.Vehicles.Count >= 1)
                    {
                        user.RemoveVehicle(user.Vehicles.FirstOrDefault(v => v.ID == id));
                    }
                    foreach (ShopModel shop in user.Shops)
                    {
                        if (shop.Vehicles.Count >= 1)
                        {
                            shop.RemoveVehicle(shop.Vehicles.FirstOrDefault(v => v.ID == id));
                        }
                    }
                }
            }
        }

        public void UpdateViews(string table, int id)
        {
            int views = 0;
            if (table == "shops")
            {
                ShopModel shop = GetShopByID(id);
                shop.IncreaseViews();
                views = shop.Views;
            }
            else if (table == "vehicles")
            {
                VehicleModel vehicle = GetVehicleByID(id);
                vehicle.IncreaseViews();
                views = vehicle.Views;
            }
            else if (table == "users")
            {
                UserModel user = GetUserByID(id);
                user.IncreaseViews();
                views = user.Views;
            }

            string sqlstr = $"UPDATE {table} " +
                           "SET views = @views " +
                           "WHERE id = @id";
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sqlstr, conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("views", views);
                    cmd.ExecuteNonQuery();
                }
            }
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
                        int.Parse(reader["views"].ToString()),
                        Convert.ToInt64(reader["registration_date"].ToString()),
                        reader["webpage"].ToString()
                        );
                        GetUserByID(shop.Owner_ID).AddShop(shop);
                    }
                }
            }

            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM vehicles", conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        VehicleModel vehicle = new VehicleModel
                        (
                        int.Parse(reader["id"].ToString()),
                        reader["brand"].ToString(),
                        reader["model"].ToString(),
                        int.Parse(reader["vintage"].ToString()),
                        reader["type"].ToString(),
                        int.Parse(reader["price"].ToString()),
                        reader["fuel"].ToString(),
                        reader["type_designation"].ToString(),
                        int.Parse(reader["cylinder_capacity"].ToString()),
                        int.Parse(reader["performance"].ToString()),
                        int.Parse(reader["odometer"].ToString()),
                        reader["description"].ToString(),
                        CheckIntOrNull(reader["shop_id"].ToString()),
                        CheckIntOrNull(reader["user_id"].ToString()),
                        int.Parse(reader["views"].ToString()),
                        int.Parse(reader["votes"].ToString()),
                        reader["validity"].ToString() == "True",
                        Convert.ToInt64(reader["registration_date"].ToString())
                        );
                        AddVehicleTo(vehicle, vehicle.Shop_ID, vehicle.User_ID);
                    }
                }
            }

            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM comments", conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CommentModel comment = new CommentModel
                        (
                        int.Parse(reader["id"].ToString()),
                        int.Parse(reader["owner_id"].ToString()),
                        reader["title"].ToString(),
                        reader["message"].ToString(),
                        Convert.ToInt64(reader["submission_time"].ToString()),
                        CheckIntOrNull(reader["user_id"].ToString()),
                        CheckIntOrNull(reader["vehicle_id"].ToString()),
                        CheckIntOrNull(reader["shop_id"].ToString())
                        );
                        AddCommentTo(comment, comment.User_ID, comment.Vehicle_ID, comment.Shop_ID);
                    }
                }
            }

            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM pictures", conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        PictureModel picture = new PictureModel
                        (
                        int.Parse(reader["id"].ToString()),
                        reader["route"].ToString(),
                        CheckIntOrNull(reader["user_id"].ToString()),
                        CheckIntOrNull(reader["vehicle_id"].ToString()),
                        CheckIntOrNull(reader["shop_id"].ToString())
                        );
                        AddPictureTo(picture, picture.User_ID, picture.Vehicle_ID, picture.Shop_ID);
                    }
                }
            }
        }

        private bool? CheckBoolOrNull(string value)
        {
            if (value == "")
                return null;
            return value == "True";
        }

        private int? CheckIntOrNull(string value)
        {
            if (!int.TryParse(value, out int x))
                return null;
            else
                return int.Parse(value);
        }
    }
}
