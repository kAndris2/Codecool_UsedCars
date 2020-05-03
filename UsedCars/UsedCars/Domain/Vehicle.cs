using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedCars.Domain
{
    public class Vehicle
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

        public Vehicle(int id, string brand, string model, int vintage, string type, int price, string fuel, int cylinder, int odometer, string description, bool validity, int? shopid, int? userid)
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
        }

        public Vehicle(int id, string brand, string model, int vintage, string type, int price, string fuel, int cylinder, int odometer, string description, int? shopid, int? userid, int views, int votes, bool validity)
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
        }

        public void IncreaseViews() { Views++; }
        public void IncreaseVotes() { Votes++; }
        public void DecreaseVotes() { Votes--; }

        public void SetDescription(string description) { Description = description; }
        public void SetPrice(int new_price) { Price = new_price; }
        public void SetOdometer(int value) { Odometer = value; }
    }
}
