using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public abstract class Attraction
    {
        private int id;
        private string openingName;
        private Park park;
        private RideType rideType;
        private List<AttractionRename> renames = new List<AttractionRename>();
        private DateOnly openingDate;

        public Attraction(int id, string openingName, DateOnly openingDate, Park park, RideType rideType)
        {
            this.id = id;
            this.openingName = openingName;
            this.openingDate = openingDate;
            this.park = park;
            this.rideType = rideType;
        }

        public abstract List<string> GetElements();

        public int GetID()
        {
            return id;
        }

        public string GetName(DateOnly date)
        {
            DateOnly closestDate = openingDate;
            string closestName = openingName;
            foreach (AttractionRename rename in renames)
            {
                if (rename.GetDate() > closestDate && rename.GetDate() <= date)
                {
                    closestDate = rename.GetDate();
                    closestName = rename.GetName();
                }
            }

            return closestName;
        }

        public RideType GetRideType()
        {
            return rideType;
        }

        public Park GetPark()
        {
            return park;
        }

        public List<AttractionRename> GetRenames()
        {
            return renames;
        }

        public void AddRename(AttractionRename rename)
        {
            renames.Add(rename);
        }

        public void SetName(string name)
        {
            openingName = name;
        }

        public void SetRideType(RideType rideType)
        {
            this.rideType = rideType;
        }

        public void SetRenames(List<AttractionRename> renames)
        {
            this.renames = renames;
        }

        public DateOnly GetOpeningDate()
        {
            return openingDate;
        }

        public void SetOpeningDate(DateOnly openingDate)
        {
            this.openingDate = openingDate;
        }

        public string GetOpeningName()
        {
            return openingName;
        }
    }
}