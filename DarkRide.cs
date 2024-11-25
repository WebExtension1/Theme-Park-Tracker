using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public class DarkRide : TrackedRide
    {
        private int type;

        public DarkRide(int id, string openingName, DateOnly openingDate, Park park, RideType rideType, int trackLength, int type) : base(id, openingName, openingDate, park, rideType, trackLength)
        {
            this.type = type;
        }

        public override List<string> GetElements()
        {
            List<string> elements = new List<string>();
            elements.Add("2");
            elements.Add(GetTrackLength().ToString());
            elements.Add(GetType());

            return elements;
        }

        public string GetType()
        {
            switch (type)
            {
                case 1:
                    return "Omnimover";
                case 2:
                    return "Trackless";
                case 3:
                    return "Boat";
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
