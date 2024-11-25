using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public static class Database
    {
        // Logged in profile
        public static Profile profile;

        // List of all data read from the files
        public static List<Profile> profiles = new List<Profile>();
        public static List<Park> parks = new List<Park>();
        public static List<Visit> visits = new List<Visit>();
        public static List<Manufacturer> manufacturers = new List<Manufacturer>();
        public static List<RideType> rideTypes = new List<RideType>();
        public static List<Attraction> attractions = new List<Attraction>();

        // Getting Profile data
        public static Profile GetProfileByID(int id)
        {
            IEnumerable<Profile> selectedProfileIEnumerable = profiles.Where(profile => profile.GetID() == id);
            List<Profile> selectedProfile = selectedProfileIEnumerable.ToList();
            return selectedProfile[0];
        }
        public static Profile GetProfileByUsername(string username)
        {
            IEnumerable<Profile> selectedProfileIEnumerable = profiles.Where(profile => profile.GetUsername() == username);
            List<Profile> selectedProfile = selectedProfileIEnumerable.ToList();
            if (selectedProfile.Count > 0)
            {
                return selectedProfile[0];
            }
            return null;
        }
        public static Profile GetProfileByEmail(string email)
        {
            IEnumerable<Profile> selectedProfileIEnumerable = profiles.Where(profile => profile.GetEmail() == email);
            List<Profile > selectedProfile = selectedProfileIEnumerable.ToList();
            if (selectedProfile.Count > 0)
            {
                return selectedProfile[0];
            }
            return null;
        }

        // Getting Park data
        public static Park GetParkByID(int id)
        {
            IEnumerable<Park> selectedParkIEnumerable = parks.Where(park => park.GetID() == id);
            List<Park> selectedPark= selectedParkIEnumerable.ToList();
            return selectedPark[0];
        }

        // Getting Visit data
        public static Visit GetVisitByID(int id)
        {
            IEnumerable<Visit> selectedVisitIEnumerable = visits.Where(visit => visit.GetID() == id);
            List<Visit> selectedVisit = selectedVisitIEnumerable.ToList();
            return selectedVisit[0];
        }
        public static List<Visit> GetVisitsByProfileID(int id)
        {
            IEnumerable<Visit> selectedVisitsIEnumerable = visits.Where(visit => visit.GetProfile().GetID() == id);
            List<Visit> selectedVisits = selectedVisitsIEnumerable.ToList();
            return selectedVisits;
        }
        public static List<Visit> GetVisitsByParkID(int id)
        {
            IEnumerable<Visit> selectedVisitsIEnumerable = visits.Where(visit => visit.GetPark().GetID() == id);
            List<Visit> selectedVisits = selectedVisitsIEnumerable.ToList();
            return selectedVisits;
        }
        
        // Getting Manufacturer data
        public static Manufacturer GetManufacturerByID(int id)
        {
            IEnumerable<Manufacturer> selectedManufacturerIEnumerable = manufacturers.Where(manufacturer => manufacturer.GetID() == id);
            List<Manufacturer> selectedManufacturer = selectedManufacturerIEnumerable.ToList();
            return selectedManufacturer[0];
        }

        // Getting RideType data
        public static RideType GetRideTypeByID(int id)
        {
            IEnumerable<RideType> selectedRideTypeIEnumerable = rideTypes.Where(rideType => rideType.GetID() == id);
            List<RideType> selectedRideType = selectedRideTypeIEnumerable.ToList();
            return selectedRideType[0];
        }
        public static List<RideType> GetRideTypesByManufacturerID(int id)
        {
            IEnumerable<RideType> selectedRideTypesIEnumerable = rideTypes.Where(rideType => rideType.GetManufacturer().GetID() == id);
            List<RideType> selectedRideTypes = selectedRideTypesIEnumerable.ToList();
            return selectedRideTypes;
        }

        // Getting Attraction data
        public static Attraction GetAttractionByID(int id)
        {
            IEnumerable<Attraction> selectedAttractionIEnumerable = attractions.Where(attraction => attraction.GetID() == id);
            List<Attraction> selectedAttraction = selectedAttractionIEnumerable.ToList();
            return selectedAttraction[0];
        }
        public static List<Attraction> GetAttractionByPark(int id)
        {
            IEnumerable<Attraction> selectedAttractionsIEnumerable = attractions.Where(attraction => attraction.GetPark().GetID() == id);
            List<Attraction> selectedAttractions = selectedAttractionsIEnumerable.ToList();
            return selectedAttractions;
        }

        // Verify valid profile info
        public static bool VerifyInfo(string username, string email, string password)
        {
            IEnumerable<Profile> selectedProfilesIEnumerable = profiles.Where(profileCheck => (profileCheck.GetUsername() == username || profileCheck.GetEmail() == email) && profileCheck.GetID() != profile.GetID());
            List<Profile> selectedProfiles = selectedProfilesIEnumerable.ToList();
            if (selectedProfiles.Count > 0)
            {
                MessageBox.Show("Duplicate information in the username/email field");
                return false;
            }
            if (password == "")
            {
                MessageBox.Show("Not a valid password");
                return false;
            }
            return true;
        }

        // Writing data to the file
        public static void SaveData()
        {
            StreamWriter sw;

            sw = new StreamWriter("Profiles.txt");
            foreach (Profile profile in profiles)
            {
                sw.WriteLine($"{profile.GetID()}\n{profile.GetUsername()}\n{profile.GetEmail()}\n{profile.GetPassword()}");
            }
            sw.WriteLine("end");
            sw.Close();

            sw = new StreamWriter("Parks.txt");
            foreach (Park park in parks)
            {
                sw.WriteLine($"{park.GetID()}\n{park.GetName()}");
            }
            sw.WriteLine("end");
            sw.Close();

            sw = new StreamWriter("Visits.txt");
            foreach (Visit visit in visits)
            {
                sw.WriteLine($"{visit.GetID()}\n{visit.GetDate()}\n{visit.GetProfile().GetID()}\n{visit.GetPark().GetID()}");
            }
            sw.WriteLine("end");
            sw.Close();

            sw = new StreamWriter("Manufacturers.txt");
            foreach (Manufacturer manufacturer in manufacturers)
            {
                sw.WriteLine($"{manufacturer.GetID()}\n{manufacturer.GetName()}");
            }
            sw.WriteLine("end");
            sw.Close();

            sw = new StreamWriter("RideTypes.txt");
            foreach (RideType rideType in rideTypes)
            {
                sw.WriteLine($"{rideType.GetID()}\n{rideType.GetName()}\n{rideType.GetManufacturer().GetID()}");
            }
            sw.WriteLine("end");
            sw.Close();

            sw = new StreamWriter("Attractions.txt");
            foreach (Attraction attraction in attractions)
            {
                string rideType = "None";
                if (attraction.GetRideType() != null)
                {
                    rideType = attraction.GetRideType().GetID().ToString();
                }
                sw.WriteLine($"{attraction.GetID()}\n{attraction.GetOpeningName()}\n{attraction.GetOpeningDate()}\n{attraction.GetPark().GetID()}\n{rideType}");
                List<string> elements = attraction.GetElements();
                switch (elements[0])
                {
                    case "1":
                        Rollercoaster rollercoaster = (Rollercoaster)attraction;
                        sw.WriteLine($"1-{rollercoaster.GetTrackLength()}-{rollercoaster.GetTopSpeed()}-{rollercoaster.GetInversions()}");
                        break;
                    case "2":
                        DarkRide darkRide = (DarkRide)attraction;
                        sw.WriteLine($"2-{darkRide.GetTrackLength()}-{darkRide.GetTypeNum()}");
                        break;
                    case "3":
                        FlatRide flatRide = (FlatRide)attraction;
                        sw.WriteLine($"3-{flatRide.GetTypeNum()}");
                        break;

                }
            }
            sw.WriteLine("end");
            sw.Close();

            sw = new StreamWriter("AttractionRenames.txt");
            foreach (Attraction attraction in attractions)
            {
                foreach (AttractionRename rename in attraction.GetRenames())
                {
                    sw.WriteLine($"{attraction.GetID()}\n{rename.GetDate()}\n{rename.GetName()}");
                }
            }
            sw.WriteLine("end");
            sw.Close();

            sw = new StreamWriter("VisitAttractions.txt");
            foreach (Visit visit in visits)
            {
                foreach (VisitAttraction attraction in visit.GetAttractions())
                {
                    sw.WriteLine($"{visit.GetID()}\n{attraction.GetOrder()}\n{attraction.GetAttraction().GetID()}\n{attraction.GetTime()}\n{attraction.GetWaitTime()}");
                }
            }
            sw.WriteLine("end");
            sw.Close();
        }

        static public int GetNextVisitID()
        {
            if (visits.Count >= 1)
            {
                return visits.Max(visit => visit.GetID()) + 1;
            }
            return 1;
        }
        static public int GetNextParkID()
        {
            if (parks.Count  >= 1)
            {
                return parks.Max(park => park.GetID()) + 1;
            }
            return 1;
        }

        static public int GetNextManufacturerID()
        {
            if (manufacturers.Count >= 1)
            {
                return manufacturers.Max(manufacturer => manufacturer.GetID()) + 1;
            }
            return 1;
        }

        static public int GetNextAttractionID()
        {
            if (attractions.Count() >= 1)
            {
                return attractions.Max(attraction => attraction.GetID()) + 1;
            }
            return 1;
        }

        static public int GetNextRideTypeID()
        {
            if (rideTypes.Count() >= 1)
            {
                return rideTypes.Max(rideType => rideType.GetID()) + 1;
            }
            return 1;
        }
        static public int GetNextProfileID()
        {
            if (profiles.Count >= 1)
            {
                return profiles.Max(profile => profile.GetID()) + 1;
            }
            return 1;
        }

        static public List<Attraction> GetAttractionsByType(string type)
        {
            if (type == "all")
            {
                return attractions;
            }
            return attractions.Where(attraction => attraction.GetElements()[0] == type).ToList();
        }
    }
}