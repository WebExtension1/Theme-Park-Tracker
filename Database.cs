using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // cd OneDrive - Sheffield Hallam University\Y2\Systems Programming\Theme Park Tracker\bin\Debug\net6.0-windows
        //
        // Command Line Modes
        //
        // 1 - Normal
        // 2 - Debug - All
        // 3 - Debug - Load
        // 4 - Debug - Save
        // 5 - Debug - Delete
        // 6 - Reset
        //
        private static int mode;
        public static string debugMessage = null;
        static public void SetMode(int newMode)
        {
            mode = newMode;
        }
        static public int GetMode()
        {
            return mode;
        }

        // Getting Profile data
        public static Profile GetProfileByID(int id)
        {
            Profile selectedProfile = profiles.AsParallel().Where(profile => profile.GetID() == id).FirstOrDefault();
            return selectedProfile;
        }
        public static Profile GetProfileByUsername(string username)
        {
            Profile selectedProfile = profiles.AsParallel().Where(profile => profile.GetUsername() == username).FirstOrDefault();
            return selectedProfile;
        }
        public static Profile GetProfileByEmail(string email)
        {
            Profile selectedProfile = profiles.AsParallel().Where(profile => profile.GetEmail() == email).FirstOrDefault();
            return selectedProfile;
        }

        // Getting Park data
        public static Park GetParkByID(int id)
        {
            Park selectedPark = parks.AsParallel().Where(park => park.GetID() == id).FirstOrDefault();
            return selectedPark;
        }

        // Getting Visit data
        public static Visit GetVisitByID(int id)
        {
            Visit selectedVisit = visits.AsParallel().Where(visit => visit.GetID() == id).FirstOrDefault();
            return selectedVisit;
        }
        public static List<Visit> GetVisitsByProfileID(int id)
        {
            List<Visit> selectedVisits = visits.AsParallel().Where(visit => visit.GetProfile().GetID() == id).ToList();
            return selectedVisits;
        }
        public static List<Visit> GetVisitsByParkID(int id)
        {
            List<Visit> selectedVisits = visits.AsParallel().Where(visit => visit.GetPark().GetID() == id).ToList();
            return selectedVisits;
        }
        
        // Getting Manufacturer data
        public static Manufacturer GetManufacturerByID(int id)
        {
            Manufacturer selectedManufacturer = manufacturers.AsParallel().Where(manufacturer => manufacturer.GetID() == id).FirstOrDefault();
            return selectedManufacturer;
        }

        // Getting RideType data
        public static RideType GetRideTypeByID(int id)
        {
            RideType selectedRideType = rideTypes.AsParallel().Where(rideType => rideType.GetID() == id).FirstOrDefault();
            return selectedRideType;
        }
        public static List<RideType> GetRideTypesByManufacturerID(int id)
        {
            List<RideType> selectedRideTypes = rideTypes.AsParallel().Where(rideType => rideType.GetManufacturer().GetID() == id).ToList();
            return selectedRideTypes;
        }

        // Getting Attraction data
        public static Attraction GetAttractionByID(int id)
        {
            Attraction selectedAttraction = attractions.AsParallel().Where(attraction => attraction.GetID() == id).FirstOrDefault();
            return selectedAttraction;
        }
        public static List<Attraction> GetAttractionByPark(int id)
        {
            List<Attraction> selectedAttractions = attractions.AsParallel().Where(attraction => attraction.GetPark().GetID() == id).ToList();
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
        public static async Task SaveData()
        {
            List<Task> tasks = new List<Task>();

            tasks.Add(Task.Run(() =>
            {
                // Saves all Profile information
                FileStream fs = new FileStream("Profiles.dat", FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                foreach (Profile profile in profiles)
                {
                    bw.Write(profile.GetID());
                    bw.Write(profile.GetUsername());
                    bw.Write(profile.GetEmail());
                    bw.Write(profile.GetPassword());
                }

                bw.Close();
                fs.Close();
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all Park information
                FileStream fs = new FileStream("Parks.dat", FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                foreach(Park park in parks)
                {
                    bw.Write(park.GetID());
                    bw.Write(park.GetName());
                }

                bw.Close();
                fs.Close();
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all Visit information
                FileStream fs = new FileStream("Visits.dat", FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                foreach (Visit visit in visits)
                {
                    bw.Write(visit.GetID());
                    bw.Write(visit.GetDate().ToString());
                    bw.Write(visit.GetProfile().GetID());
                    bw.Write(visit.GetPark().GetID());
                }

                bw.Close();
                fs.Close();
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all Manufacturer information
                FileStream fs = new FileStream("Manufacturers.dat", FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                foreach (Manufacturer manufacturer in manufacturers)
                {
                    bw.Write(manufacturer.GetID());
                    bw.Write(manufacturer.GetName());
                }

                bw.Close();
                fs.Close();
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all Ride Type information
                FileStream fs = new FileStream("RideTypes.dat", FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                foreach (RideType rideType in rideTypes)
                {
                    bw.Write(rideType.GetID());
                    bw.Write(rideType.GetName());
                    bw.Write(rideType.GetManufacturer().GetID());
                }

                bw.Close();
                fs.Close();
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all Attraction information
                FileStream fs = new FileStream("Attractions.dat", FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                foreach (Attraction attraction in attractions)
                {
                    string rideType = "None";
                    if (attraction.GetRideType() != null)
                    {
                        rideType = attraction.GetRideType().GetID().ToString();
                    }

                    bw.Write(attraction.GetID());
                    bw.Write(attraction.GetOpeningName());
                    bw.Write(attraction.GetOpeningDate().ToString());
                    bw.Write(attraction.GetPark().GetID());
                    bw.Write(rideType);

                    List<string> elements = attraction.GetElements();
                    switch (elements[0])
                    {
                        case "1":
                            Rollercoaster rollercoaster = (Rollercoaster)attraction;
                            bw.Write($"1-{rollercoaster.GetTrackLength()}-{rollercoaster.GetTopSpeed()}-{rollercoaster.GetInversions()}");
                            break;
                        case "2":
                            DarkRide darkRide = (DarkRide)attraction;
                            bw.Write($"2-{darkRide.GetTrackLength()}-{darkRide.GetTypeNum()}");
                            break;
                        case "3":
                            FlatRide flatRide = (FlatRide)attraction;
                            bw.Write($"3-{flatRide.GetTypeNum()}");
                            break;

                    }
                }

                bw.Close();
                fs.Close();
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all Attraction Rename information
                FileStream fs = new FileStream("AttractionRenames.dat", FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                foreach (Attraction attraction in attractions)
                {
                    foreach (AttractionRename rename in attraction.GetRenames())
                    {
                        bw.Write(attraction.GetID());
                        bw.Write(rename.GetDate().ToString());
                        bw.Write(rename.GetName());
                    }
                }

                bw.Close();
                fs.Close();
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all Visit Attraction information
                FileStream fs = new FileStream("VisitAttractions.dat", FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                foreach (Visit visit in visits)
                {
                    foreach (VisitAttraction attraction in visit.GetAttractions())
                    {
                        bw.Write(visit.GetID());
                        bw.Write(attraction.GetOrder());
                        bw.Write(attraction.GetAttraction().GetID());
                        bw.Write(attraction.GetTime().ToString());
                        bw.Write(attraction.GetWaitTime());
                    }
                }

                bw.Close();
                fs.Close();
            }));

            await Task.WhenAll(tasks);
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