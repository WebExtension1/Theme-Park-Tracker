using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public class VisitAttraction
    {
        private Attraction attraction;
        private TimeOnly time;
        private int order, waitTime;

        public VisitAttraction(Attraction attraction, int order, TimeOnly time, int waitTime)
        {
            this.attraction = attraction;
            this.order = order;
            this.time = time;
            this.waitTime = waitTime;
        }

        public Attraction GetAttraction()
        {
            return attraction;
        }

        public TimeOnly GetTime()
        {
            return time;
        }

        public int GetOrder()
        {
            return order;
        }

        public int GetWaitTime()
        {
            return waitTime;
        }

        public void ReduceOrder()
        {
            order--;
        }
    }
}