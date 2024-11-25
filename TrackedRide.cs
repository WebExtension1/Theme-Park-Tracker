using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public abstract class TrackedRide : Attraction
    {
        private int trackLength;

        public TrackedRide(int id, string openingName, DateOnly openingDate, Park park, RideType rideType, int trackLength) : base(id, openingName, openingDate, park, rideType)
        {
            this.trackLength = trackLength;
        }

        public int GetTrackLength()
        {
            return trackLength;
        }

        public void SetTrackLength(int trackLength)
        {
            this.trackLength = trackLength;
        }
    }
}
