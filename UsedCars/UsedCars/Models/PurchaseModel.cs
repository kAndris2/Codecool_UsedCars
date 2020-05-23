using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedCars.Models
{
    public class PurchaseModel : ICloneable
    {
        public int ID { get; }
        public int Shop_ID { get; }
        public int Amount { get; set; }
        public int Year { get; }
        public String Brand { get; }

        public PurchaseModel(int id, int shopid, int amount, int year, string brand)
        {
            ID = id;
            Shop_ID = shopid;
            Amount = amount;
            Year = year;
            Brand = brand;
        }

        public object Clone() { return this.MemberwiseClone(); }
    }
}
