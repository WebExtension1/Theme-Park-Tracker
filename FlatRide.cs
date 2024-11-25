using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public class FlatRide : Attraction
    {
        private int type;

        public FlatRide(int id, string openingName, DateOnly openingDate, Park park, RideType rideType, int type) : base(id, openingName, openingDate, park, rideType)
        {
            this.type = type;
        }

        public override List<string> GetElements()
        {
            List<string> elements = new List<string>();
            elements.Add("3");
            elements.Add(GetType());

            return elements;
        }

        public string GetType()
        {
            switch (type)
            {
                case 1:
                    return "Pendulum";
                case 2:
                    return "Rotation";
                case 3:
                    return "Tower";
                case 4:
                    return "Other";
                default:
                    return "Unknown";
            }
        }

        public string GetTypeNum()
        {
            return type.ToString();
        }

        public void SetType(int type)
        {
            this.type = type;
        }
    }
}
