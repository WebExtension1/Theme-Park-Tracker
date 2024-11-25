using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public class Visit
    {
        private int id;
        private DateOnly date;
        private Profile profile;
        private Park park;
        private List<VisitAttraction> attractions = new List<VisitAttraction>();

        public Visit(int id, DateOnly date, Profile profile, Park park)
        {
            this.id = id;
            this.date = date;
            this.profile = profile;
            this.park = park;
        }

        public int GetID()
        {
            return id;
        }

        public DateOnly GetDate()
        {
            return date;
        }

        public Profile GetProfile()
        {
            return profile;
        }

        public Park GetPark()
        {
            return park;
        }

        public List<VisitAttraction> GetAttractions()
        {
            return attractions;
        }

        public int GetAttractionCount()
        {
            return attractions.Count;
        }

        public int GetUniqueAttractionCount()
        {
            HashSet<Attraction> attractionsHashset = new HashSet<Attraction>();
            foreach (VisitAttraction visit in attractions)
            {
                attractionsHashset.Add(visit.GetAttraction());
            }
            return attractionsHashset.Count;
        }

        public void AddAttraction(VisitAttraction attraction)
        {
            attractions.Add(attraction);
        }

        public void SetPark(Park park)
        {
            this.park = park;
        }

        public void SetDate(DateOnly date)
        {
            this.date = date;
        }

        public void SetAttractions(List<VisitAttraction> attractions)
        {
            this.attractions = attractions;
        }

        public void RemoveVisitAttraction(VisitAttraction visitAttractionToRemove)
        {
            int removedOrder = visitAttractionToRemove.GetOrder();
            foreach (VisitAttraction visitAttraction in attractions)
            {
                if (visitAttraction.GetOrder() > removedOrder)
                {
                    visitAttraction.ReduceOrder();
                }
            }
            attractions.Remove(visitAttractionToRemove);
        }
    }
}