using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public class Park
    {
        private int id;
        private string name;
        private List<Attraction> attractions = new List<Attraction>();

        public Park(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public void SetAttractions(List<Attraction> attractions)
        {
            this.attractions = attractions;
        }

        public List<Attraction> GetAttractions()
        {
            return attractions;
        }

        public void AddAttraction(Attraction attraction)
        {
            attractions.Add(attraction);
        }

        public int GetID()
        {
            return id;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void RemoveAttraction(Attraction attraction)
        {
            attractions.Remove(attraction);
        }
    }
}