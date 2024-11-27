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

            tasks.Add(Task.Run(async () =>
            {
                StreamWriter sw = new StreamWriter("Profiles.txt");
                foreach (Profile profile in profiles)
                {
                    await sw.WriteLineAsync($"{profile.GetID()}\n{profile.GetUsername()}\n{profile.GetEmail()}\n{profile.GetPassword()}");
                }
                await sw.WriteLineAsync("end");
                sw.Close();
            }));

            tasks.Add(Task.Run(async () =>
            {
                StreamWriter sw = new StreamWriter("Parks.txt");
                foreach (Park park in parks)
                {
                    await sw.WriteLineAsync($"{park.GetID()}\n{park.GetName()}");
                }
                await sw.WriteLineAsync("end");
                sw.Close();
            }));

            tasks.Add(Task.Run(async () =>
            {
                StreamWriter sw = new StreamWriter("Visits.txt");
                foreach (Visit visit in visits)
                {
                    await sw.WriteLineAsync($"{visit.GetID()}\n{visit.GetDate()}\n{visit.GetProfile().GetID()}\n{visit.GetPark().GetID()}");
                }
                await sw.WriteLineAsync("end");
                sw.Close();
            }));

            tasks.Add(Task.Run(async () =>
            {
                StreamWriter sw = new StreamWriter("Manufacturers.txt");
                foreach (Manufacturer manufacturer in manufacturers)
                {
                    await sw.WriteLineAsync($"{manufacturer.GetID()}\n{manufacturer.GetName()}");
                }
                await sw.WriteLineAsync("end");
                sw.Close();
            }));

            tasks.Add(Task.Run(async () =>
            {
                StreamWriter sw = new StreamWriter("RideTypes.txt");
                foreach (RideType rideType in rideTypes)
                {
                    await sw.WriteLineAsync($"{rideType.GetID()}\n{rideType.GetName()}\n{rideType.GetManufacturer().GetID()}");
                }
                await sw.WriteLineAsync("end");
                sw.Close();
            }));

            tasks.Add(Task.Run(async () =>
            {
                StreamWriter sw = new StreamWriter("Attractions.txt");
                foreach (Attraction attraction in attractions)
                {
                    string rideType = "None";
                    if (attraction.GetRideType() != null)
                    {
                        rideType = attraction.GetRideType().GetID().ToString();
                    }
                    await sw.WriteLineAsync($"{attraction.GetID()}\n{attraction.GetOpeningName()}\n{attraction.GetOpeningDate()}\n{attraction.GetPark().GetID()}\n{rideType}");
                    List<string> elements = attraction.GetElements();
                    switch (elements[0])
                    {
                        case "1":
                            Rollercoaster rollercoaster = (Rollercoaster)attraction;
                            await sw.WriteLineAsync($"1-{rollercoaster.GetTrackLength()}-{rollercoaster.GetTopSpeed()}-{rollercoaster.GetInversions()}");
                            break;
                        case "2":
                            DarkRide darkRide = (DarkRide)attraction;
                            await sw.WriteLineAsync($"2-{darkRide.GetTrackLength()}-{darkRide.GetTypeNum()}");
                            break;
                        case "3":
                            FlatRide flatRide = (FlatRide)attraction;
                            await sw.WriteLineAsync($"3-{flatRide.GetTypeNum()}");
                            break;

                    }
                }
                await sw.WriteLineAsync("end");
                sw.Close();
            }));

            tasks.Add(Task.Run(async () =>
            {
                StreamWriter sw = new StreamWriter("AttractionRenames.txt");
                foreach (Attraction attraction in attractions)
                {
                    foreach (AttractionRename rename in attraction.GetRenames())
                    {
                        await sw.WriteLineAsync($"{attraction.GetID()}\n{rename.GetDate()}\n{rename.GetName()}");
                    }
                }
                await sw.WriteLineAsync("end");
                sw.Close();
            }));

            tasks.Add(Task.Run(async () =>
            {
                StreamWriter sw = new StreamWriter("VisitAttractions.txt");
                foreach (Visit visit in visits)
                {
                    foreach (VisitAttraction attraction in visit.GetAttractions())
                    {
                        await sw.WriteLineAsync($"{visit.GetID()}\n{attraction.GetOrder()}\n{attraction.GetAttraction().GetID()}\n{attraction.GetTime()}\n{attraction.GetWaitTime()}");
                    }
                }
                await sw.WriteLineAsync("end");
                sw.Close();
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