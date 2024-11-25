using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public class RideType
    {
        private int id;
        private string name;
        private Manufacturer manufacturer;

        public RideType(int id, string name, Manufacturer manufacturer)
        {
            this.id = id;
            this.name = name;
            this.manufacturer = manufacturer;
        }

        public int GetID()
        {
            return id;
        }

        public string GetName()
        {
            return name;
        }

        public Manufacturer GetManufacturer()
        {
            return manufacturer;
        }

        public void SetName(string name)
        {
            this.name = name;
        }
    }
}