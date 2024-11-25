using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Theme_Park_Tracker
{
    public class Profile
    {
        private int id;
        private string username, email, password;
        private List<Visit> visits = new List<Visit>();

        public Profile(int id, string username, string email, string password, bool needsEncrypting)
        {
            this.id = id;
            this.username = username;
            this.email = email;
            this.password = password;
            if (needsEncrypting)
            {
                this.password = EncryptPassword(password);
            }
        }

        public int GetID()
        {
            return id;
        }

        public string GetUsername()
        {
            return username;
        }

        public string GetEmail()
        {
            return email;
        }

        public string GetPassword()
        {
            return password;
        }

        public List<Visit> GetVisits()
        {
            return visits;
        }

        public bool VerifyPassword(string password)
        {
            if (this.password == EncryptPassword(password))
            {
                return true;
            }
            return false;
        }

        public string EncryptPassword(string password)
        {
            string chars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890!\"£$% ^&*()_+-={ }[]~#:;@'<,>.?/|\\¬`¦ ";
            int total = 1, check = 1;
            foreach (char c in password)
            {
                total += (chars.IndexOf(c) * total * check);
                check++;
            }
            return total.ToString();
        }

        public void SetVisits(List<Visit> visits)
        {
            this.visits = visits;
        }

        public bool CheckIfDidVisit(Visit visit)
        {
            if (visits.Contains(visit))
            {
                return true;
            }
            return false;
        }

        public void UpdateInfo(string username, string email, string password)
        {
            this.username = username;
            this.email = email;
            this.password = EncryptPassword(password);
        }

        public void AddVisit(Visit visit)
        {
            visits.Add(visit);
        }

        public void RemoveVisit(Visit visit)
        {
            visits.Remove(visit);
        }
    }
}