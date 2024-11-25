using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public class Manufacturer
    {
        private int id;
        private string name;
        private List<RideType> rideTypes = new List<RideType>();

        public Manufacturer(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int GetID()
        {
            return id;
        }

        public string GetName()
        {
            return name;
        }

        public void SetRideTypes(List<RideType> rideTypes)
        {
            this.rideTypes = rideTypes;
        }

        public List<RideType> GetRideTypes()
        {
            return rideTypes;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void AddRideType(RideType rideType)
        {
            rideTypes.Add(rideType);
        }

        public void RemoveRideType(RideType rideType)
        {
            rideTypes.Remove(rideType);
        }
    }
}