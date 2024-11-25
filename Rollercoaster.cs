 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public class Rollercoaster : TrackedRide
    {
        private int topSpeed, inversions;

        public Rollercoaster(int id, string openingName, DateOnly openingDate, Park park, RideType rideType, int trackLength, int topSpeed, int inversions) : base(id, openingName, openingDate, park, rideType, trackLength)
        {
            this.topSpeed = topSpeed;
            this.inversions = inversions;
        }

        public override List<string> GetElements()
        {
            List<string> elements = new List<string>();
            elements.Add("1");
            elements.Add(GetTrackLength().ToString());
            elements.Add(topSpeed.ToString());
            elements.Add(inversions.ToString());

            return elements;
        }

        public int GetTopSpeed()
        {
            return topSpeed;
        }

        public int GetInversions()
        {
            return inversions;
        }

        public void SetTopSpeed(int topSpeed)
        {
            this.topSpeed = topSpeed;
        }

        public void SetInversions(int inversions)
        {
            this.inversions = inversions;
        }
    }
}
