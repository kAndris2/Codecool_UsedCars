using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedCars.Domain
{
    public class Shop
    {
        public int ID { get; }
        public int Owner_ID { get; }
        public int Views { get; private set; }
        public DateTime Foundation_Date { get; private set; }
        public String Name { get; private set; }
        public String Address { get; private set; }
        public String Description { get; private set; }

        public Shop(int id, string name, int ownerid, string address)
        {
            ID = id;
            Name = name;
            Owner_ID = ownerid;
            Address = address;
        }

        public Shop(int id, string name, string description, DateTime foundation_date, int ownerid, string address, int views)
        {
            ID = id;
            Name = name;
            Description = description;
            Foundation_Date = foundation_date;
            Owner_ID = ownerid;
            Address = address;
            Views = views;
        }

        public void IncreaseViews() { Views++; }
        
        public void SetFoundationDate(DateTime new_date) { Foundation_Date = new_date; }
        public void SetName(string new_name) { Name = new_name; }
        public void SetAddress(string new_address) { Address = new_address; }
        public void SetDescription(string text) { Description = text; }
    }
}
