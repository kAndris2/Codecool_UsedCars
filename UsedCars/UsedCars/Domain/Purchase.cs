using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedCars.Domain
{
    public class Purchase
    {
        public int ID { get; }
        public int Shop_ID { get; }
        public int Amount { get; }
        public int Year { get; }
        public String Brand { get; }

        public Purchase(int id, int shopid, int amount, int year, string brand)
        {
            ID = id;
            Shop_ID = shopid;
            Amount = amount;
            Year = year;
            Brand = brand;
        }
    }
}
