using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public class AttractionRename
    {
        private DateOnly day;
        private string newName;

        public AttractionRename(DateOnly day, string newName)
        {
            this.day = day;
            this.newName = newName;
        }

        public DateOnly GetDate()
        {
            return day;
        }

        public string GetName()
        {
            return newName;
        }
    }
}