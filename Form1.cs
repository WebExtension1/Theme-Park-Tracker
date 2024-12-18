using System;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Theme_Park_Tracker
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            // PC
            // cd OneDrive - Sheffield Hallam University\Y2\Systems Programming\Theme Park Tracker\bin\Debug\net6.0-windows
            //
            // Latptop
            // cd source\repos\WebExtension1\Theme-Park-Tracker\bin\Debug\net6.0-windows
            //
            string[] files = { "AttractionRenames.dat", "Attractions.dat", "Manufacturers.dat", "Parks.dat", "Profiles.dat", "RideTypes.dat", "VisitAttractions.dat", "Visits.dat" };
            
            if (args.Length > 0)
            {
                if (args[0].ToLower() == "reset")
                {
                    bool filesDeleted = false;
                    foreach (string file in files)
                    {
                        if (File.Exists(file))
                        {
                            filesDeleted = true;
                            File.Delete(file);
                        }
                    }
                    if (filesDeleted)
                    {
                        MessageBox.Show("File content reset");
                    }
                    else
                    {
                        MessageBox.Show("No files found, no changes have been made");
                    }
                }
            }

            InitializeComponent();
            InitializeDataAsync();
            Database.SetNextIDs();

            if (args.Length > 0)
            {
                if (args[0].ToLower() == "all")
                {
                    MessageBox.Show("Populating");
                    int amount = int.Parse(args[1].ToLower());
                    int secondaryAmount = 0;
                    try
                    {
                        secondaryAmount = int.Parse(args[2]);
                    }
                    catch { }
                    foreach (string fileName in files)
                    {
                        string file = fileName.Split('.')[0].ToLower();
                        PopulateData(file, amount, secondaryAmount);
                    }
                }
                else if (args[0].ToLower() != "reset")
                {
                    MessageBox.Show("Populating");
                    int secondaryAmount = 0;
                    for (int i = 0; i < args.Length; i += 2)
                    {
                        string file = args[i].ToLower();
                        int amount = int.Parse(args[i + 1]);
                        try
                        {
                            secondaryAmount = int.Parse(args[i + 2]);
                            i++;
                        }
                        catch { }
                        PopulateData(file, amount, secondaryAmount);
                    }
                }
            }
        }
        private void PopulateData(string file, int amount, int secondaryAmount)
        {
            Park park;
            Attraction attraction;
            Manufacturer manufacturer;
            Profile profile;
            int id;
            switch (file)
            {
                case "attractionrenames":
                    attraction = Database.attractions.FirstOrDefault();
                    if (attraction == null)
                    {
                        park = Database.parks.FirstOrDefault();
                        if (park == null)
                        {
                            id = Database.GetNextParkID();
                            park = new Park(id, "Renames Park");
                            Database.parks.Add(park);
                        }
                        attraction = new FlatRide(1, "Renames Attraction", DateOnly.FromDateTime(DateTime.Now), park, null, 1);
                        park.AddAttraction(attraction);
                        Database.attractions.Add(attraction);
                    }
                    for (int i = 0; i < amount; i++)
                    {
                        AttractionRename rename = new AttractionRename(DateOnly.FromDateTime(DateTime.Now), $"TestName - {i + 1}");
                        attraction.AddRename(rename);
                    }
                    break;
                case "attractions":
                    park = Database.parks.FirstOrDefault();
                    if (park == null)
                    {
                        park = new Park(1, "Attractions Park");
                        Database.parks.Add(park);
                    }
                    id = Database.GetNextAttractionID();
                    for (int i = 0; i < amount; i++)
                    {
                        attraction = new FlatRide(id + i, $"TestName - {id + i}", DateOnly.FromDateTime(DateTime.Now), park, null, 1);
                        park.AddAttraction(attraction);
                        Database.attractions.Add(attraction);
                    }
                    break;
                case "manufacturers":
                    id = Database.GetNextManufacturerID();
                    for (int i = 0; i < amount; i++)
                    {
                        manufacturer = new Manufacturer(id + i, $"TestName - {id + i}");
                        Database.manufacturers.Add(manufacturer);
                    }
                    break;
                case "parks":
                    id = Database.GetNextParkID();
                    for (int i = 0; i < amount; i++)
                    {
                        park = new Park(id + i, $"TestName - {id + i}");
                        Database.parks.Add(park);
                    }
                    break;
                case "profiles":
                    id = Database.GetNextProfileID();
                    for (int i = 0; i < amount; i++)
                    {
                        profile = new Profile(id + i, $"TestName - {id + i}", $"testemail{id + i}@email.com", "test", true);
                        Database.profiles.Add(profile);
                    }
                    break;
                case "ridetypes":
                    manufacturer = Database.manufacturers.FirstOrDefault();
                    if (manufacturer == null)
                    {
                        manufacturer = new Manufacturer(1, $"RideTypes Manufacturer");
                        Database.manufacturers.Add(manufacturer);
                    }
                    id = Database.GetNextRideTypeID();
                    for (int i = 0; i < amount; i++)
                    {
                        RideType rideType = new RideType(id + i, $"TestName - {id + i}", manufacturer);
                        manufacturer.AddRideType(rideType);
                        Database.rideTypes.Add(rideType);
                    }
                    break;
                case "visitattractions":
                    MessageBox.Show("No dummy data created for 'VisitAttraction'. This is because the same functionality for testing purposes can be achieved with 'Visits'");
                    break;
                case "visits":
                    attraction = Database.attractions.FirstOrDefault();
                    park = Database.parks.FirstOrDefault();
                    if (attraction == null)
                    {
                        if (park == null)
                        {
                            park = new Park(1, "Visits Park");
                            Database.parks.Add(park);
                        }
                        attraction = new FlatRide(1, "Visits Attraction", DateOnly.FromDateTime(DateTime.Now), park, null, 1);
                        park.AddAttraction(attraction);
                        Database.attractions.Add(attraction);
                    }
                    profile = Database.profiles.FirstOrDefault();
                    if (profile == null)
                    {
                        profile = new Profile(1, "Visit Profile", "profile@email.com", "test", true);
                        Database.profiles.Add(profile);
                    }
                    id = Database.GetNextVisitID();
                    for (int i = 0; i < amount; i++)
                    {
                        Visit visit = new Visit(id + i, DateOnly.FromDateTime(DateTime.Now), profile, park);
                        Database.visits.Add(visit);
                        profile.AddVisit(visit);
                        for (int links = 0; links < secondaryAmount; links++)
                        {
                            VisitAttraction visitAttraction = new VisitAttraction(attraction, links + 1, TimeOnly.FromDateTime(DateTime.Now), 100);
                            visit.AddAttraction(visitAttraction);
                        }
                    }
                    break;
            }
        }
        private async Task InitializeDataAsync()
        {
            await ReadFiles();
            await LinkForeignElements();
            DisplayLogin(null, null);
        }
        static async Task ReadFiles()
        {
            List<Task> tasks = new List<Task>();

            tasks.Add(Task.Run(() =>
            {
                // Loads all Profile information
                if (!File.Exists("Profiles.dat"))
                {
                    File.Create("Profiles.dat");
                }
                else
                {
                    FileStream fs = new FileStream("Profiles.dat", FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        int id = br.ReadInt32();
                        string username = br.ReadString();
                        string email = br.ReadString();
                        string password = br.ReadString();
                        Database.profiles.Add(new Profile(id, username, email, password, false));
                    }

                    br.Close();
                    fs.Close();
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all Park information
                if (!File.Exists("Parks.dat"))
                {
                    File.Create("Parks.dat");
                }
                else
                {
                    FileStream fs = new FileStream("Parks.dat", FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        int id = br.ReadInt32();
                        string name = br.ReadString();
                        Database.parks.Add(new Park(id, name));
                    }

                    br.Close();
                    fs.Close();
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all the Manufacturer information
                if (!File.Exists("Manufacturers.dat"))
                {
                    File.Create("Manufacturers.dat");
                }
                else
                {
                    FileStream fs = new FileStream("Manufacturers.dat", FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        int id = br.ReadInt32();
                        string name = br.ReadString();
                        Database.manufacturers.Add(new Manufacturer(id, name));
                    }

                    br.Close();
                    fs.Close();
                }
            }));

            // Awaits the first set of tasks being executed so they can be referenced by other tasks
            await Task.WhenAll(tasks);
            tasks.Clear();

            tasks.Add(Task.Run(() =>
            {
                // Loads all the Visit information
                if (!File.Exists("Visits.dat"))
                {
                    File.Create("Visits.dat");
                }
                else
                {
                    FileStream fs = new FileStream("Visits.dat", FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        int id = br.ReadInt32();
                        DateOnly date = DateOnly.Parse("January 01, 1900");
                        try
                        {
                            date = DateOnly.Parse(br.ReadString());
                        }
                        catch { }
                        Profile profile = Database.GetProfileByID(br.ReadInt32());
                        Park park = Database.GetParkByID(br.ReadInt32());
                        Database.visits.Add(new Visit(id, date, profile, park));
                    }

                    br.Close();
                    fs.Close();
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all Ride Type information
                if (!File.Exists("RideTypes.dat"))
                {
                    File.Create("RideTypes.dat");
                }
                else
                {
                    FileStream fs = new FileStream("RideTypes.dat", FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        int id = br.ReadInt32();
                        string name = br.ReadString();
                        Manufacturer manufacturer = Database.GetManufacturerByID(br.ReadInt32());
                        Database.rideTypes.Add(new RideType(id, name, manufacturer));
                    }

                    br.Close();
                    fs.Close();
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all Attraction information
                if (!File.Exists("Attractions.dat"))
                {
                    File.Create("Attractions.dat");
                }
                else
                {
                    FileStream fs = new FileStream("Attractions.dat", FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        int id = br.ReadInt32();
                        string openingName = br.ReadString();
                        DateOnly openingDate = DateOnly.Parse(br.ReadString());
                        Park park = Database.GetParkByID(br.ReadInt32());

                        string rideTypeString = br.ReadString();
                        RideType rideType = null;
                        if (rideTypeString != "None")
                        {
                            rideType = Database.GetRideTypeByID(int.Parse(rideTypeString));
                        }

                        Attraction attraction = new FlatRide(id, openingName, openingDate, park, rideType, 1);
                        string[] attractionLine = br.ReadString().Split('-');
                        int trackLength, topSpeed, inversions, type;
                        switch (attractionLine[0])
                        {
                            case "1":
                                trackLength = int.Parse(attractionLine[1]);
                                topSpeed = int.Parse(attractionLine[2]);
                                inversions = int.Parse(attractionLine[3]);
                                attraction = new Rollercoaster(id, openingName, openingDate, park, rideType, trackLength, topSpeed, inversions);
                                break;
                            case "2":
                                trackLength = int.Parse(attractionLine[1]);
                                type = int.Parse(attractionLine[2]);
                                attraction = new DarkRide(id, openingName, openingDate, park, rideType, trackLength, type);
                                break;
                            case "3":
                                type = int.Parse(attractionLine[1]);
                                attraction = new FlatRide(id, openingName, openingDate, park, rideType, type);
                                break;
                        }
                        Database.attractions.Add(attraction);
                    }

                    br.Close();
                    fs.Close();
                }
            }));

            // Awaits the second set of tasks being executed so they can be referenced by other tasks
            await Task.WhenAll(tasks);

            tasks.Add(Task.Run(() =>
            {
                // Loads all Attraction Rename information
                if (!File.Exists("AttractionRenames.dat"))
                {
                    File.Create("AttractionRenames.dat");
                }
                else
                {
                    FileStream fs = new FileStream("AttractionRenames.dat", FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        Attraction attraction = Database.GetAttractionByID(br.ReadInt32());
                        DateOnly date = DateOnly.Parse("January 01, 1900");
                        try
                        {
                            date = DateOnly.Parse(br.ReadString());
                        }
                        catch { }
                        string newName = br.ReadString();
                        attraction.AddRename(new AttractionRename(date, newName));
                    }

                    br.Close();
                    fs.Close();
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                // Loads all Visit Sttraction information
                if (!File.Exists("VisitAttractions.dat"))
                {
                    File.Create("VisitAttractions.dat");
                }
                else
                {
                    FileStream fs = new FileStream("VisitAttractions.dat", FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        Visit visit = Database.GetVisitByID(br.ReadInt32());
                        int order = br.ReadInt32();
                        Attraction attraction = Database.GetAttractionByID(br.ReadInt32());
                        TimeOnly time = TimeOnly.Parse(br.ReadString());
                        int waitTime = br.ReadInt32();
                        visit.AddAttraction(new VisitAttraction(attraction, order, time, waitTime));
                    }

                    br.Close();
                    fs.Close();
                }
            }));

            // Awaits the final set of tasks before finishing
            await Task.WhenAll(tasks);
        }
        static async Task LinkForeignElements()
        {
            List<Task> tasks = new List<Task>();

            tasks.Add(Task.Run(() =>
            {
                foreach (Profile profile in Database.profiles)
                {
                    profile.SetVisits(Database.GetVisitsByProfileID(profile.GetID()));
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                foreach (Park park in Database.parks)
                {
                    park.SetAttractions(Database.GetAttractionByPark(park.GetID()));
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                foreach (Manufacturer manufacturer in Database.manufacturers)
                {
                    manufacturer.SetRideTypes(Database.GetRideTypesByManufacturerID(manufacturer.GetID()));
                }
            }));

            // Awaits the tasks before finishing
            await Task.WhenAll(tasks);
        }

        public void DisplayLogin(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            // "Login" text
            ViewPanel.Controls.Add(CreateLabel("Login", null, new Point(90, 130), 40, FontStyle.Bold, null, null));

            // "Username" text
            ViewPanel.Controls.Add(CreateLabel("Username:", null, new Point(100, 230), 15, FontStyle.Regular, null, null));

            // "Password" text
            ViewPanel.Controls.Add(CreateLabel("Password:", null, new Point(100, 275), 15, FontStyle.Regular, null, null));

            // Username field
            ViewPanel.Controls.Add(CreateTextBox(null, "Username", new Point(210, 230), 200, false));

            // Password field
            ViewPanel.Controls.Add(CreateTextBox(null, "Password", new Point(210, 275), 200, true));

            // Login button
            ViewPanel.Controls.Add(CreateButton("Login", new Point(205, 320), LogInClicked, null));

            // Sign Up route
            ViewPanel.Controls.Add(CreateLabel("Create an account", null, new Point(190, 360), 9, FontStyle.Regular, DisplayCreateAccount, null));
        }
        public void LogInClicked(object sender, EventArgs e)
        {
            // Get Username
            TextBox textBox = (TextBox)ViewPanel.Controls.Find("Username", true)[0];
            string username = textBox.Text;

            // Get Password
            textBox = (TextBox)ViewPanel.Controls.Find("Password", true)[0];
            string password = textBox.Text;

            // Tries to find an account with that username
            Profile checkProfile = Database.GetProfileByUsername(username);
            if (checkProfile == null)
            {
                MessageBox.Show("User not found");
            }
            else
            {
                // Verified the password against the account
                bool validPassword = checkProfile.VerifyPassword(password);
                if (validPassword == true)
                {
                    Database.profile = checkProfile;
                    LoadFeed();
                }
                else
                {
                    MessageBox.Show("Invalid Password");
                }
            }
        }
        public void DisplayCreateAccount(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            // "Sign Up" text
            ViewPanel.Controls.Add(CreateLabel("Sign Up:", null, new Point(90, 130), 40, FontStyle.Bold, null, null));

            // "Username" text
            ViewPanel.Controls.Add(CreateLabel("Username:", null, new Point(100, 230), 15, FontStyle.Regular, null, null));

            // "Email" text
            ViewPanel.Controls.Add(CreateLabel("Email:", null, new Point(100, 275), 15, FontStyle.Regular, null, null));

            // "Password" text
            ViewPanel.Controls.Add(CreateLabel("Password:", null, new Point(100, 320), 15, FontStyle.Regular, null, null));

            // "Password" text for confirm password
            ViewPanel.Controls.Add(CreateLabel("Password:", null, new Point(100, 365), 15, FontStyle.Regular, null, null));

            // Username Field
            ViewPanel.Controls.Add(CreateTextBox(null, "Username", new Point(210, 230), 200, false));

            // Email Field
            ViewPanel.Controls.Add(CreateTextBox(null, "Email", new Point(210, 275), 200, false));

            // Password Field
            ViewPanel.Controls.Add(CreateTextBox(null, "Password1", new Point(210, 320), 200, true));

            // Password Field for confirm password
            ViewPanel.Controls.Add(CreateTextBox(null, "Password2", new Point(210, 365), 200, true));

            // Sign Up Button
            ViewPanel.Controls.Add(CreateButton("Sign Up", new Point(205, 410), SignUpClicked, null));

            // Login route
            ViewPanel.Controls.Add(CreateLabel("Login", null, new Point(223, 450), 9, FontStyle.Regular, DisplayLogin, null));
        }
        public void SignUpClicked(object sender, EventArgs e)
        {
            // Get Username
            TextBox textBox = (TextBox)ViewPanel.Controls.Find("Username", true)[0];
            string username = textBox.Text;

            // Get Email
            textBox = (TextBox)ViewPanel.Controls.Find("Email", true)[0];
            string email = textBox.Text;

            // Get Password1
            textBox = (TextBox)ViewPanel.Controls.Find("Password1", true)[0];
            string password1 = textBox.Text;

            // Get Password2
            textBox = (TextBox)ViewPanel.Controls.Find("Password2", true)[0];
            string password2 = textBox.Text;

            // Verifies all of the submitted information is valid
            if (Database.VerifyInfo(username, email, password1, password2, true))
            {
                Profile profile = new Profile(Database.GetNextProfileID(), username, email, password1, true);
                Database.profiles.Add(profile);
                Database.profile = profile;
                LoadFeed();
            }
        }

        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verifies the user has logged in
            if (Database.profile != null)
            {
                LoadProfile();
            }
        }
        public void LoadProfile()
        {
            ViewPanel.Controls.Clear();

            // Displays username at top of page
            ViewPanel.Controls.Add(CreateLabel(Database.profile.GetUsername(), null, new Point(12, 10), 30, FontStyle.Regular, null, null));

            // Button to delete Profile
            ViewPanel.Controls.Add(CreateButton("Delete Profile", new Point(1110, 44), DeleteProfile, null));

            // "Username" text
            ViewPanel.Controls.Add(CreateLabel("Username:", null, new Point(14, 80), 15, FontStyle.Regular, null, null));

            // Username field
            ViewPanel.Controls.Add(CreateTextBox(Database.profile.GetUsername(), "Username", new Point(130, 82), 200, false));

            // "Email" text
            ViewPanel.Controls.Add(CreateLabel("Email:", null, new Point(14, 110), 15, FontStyle.Regular, null, null));

            // Email field
            ViewPanel.Controls.Add(CreateTextBox(Database.profile.GetEmail(), "Email", new Point(130, 112), 200, false));

            // "Password" text
            ViewPanel.Controls.Add(CreateLabel("Password:", null, new Point(14, 140), 15, FontStyle.Regular, null, null));

            // Password field
            ViewPanel.Controls.Add(CreateTextBox(null, "Password1", new Point(130, 142), 200, true));

            // "Password" text
            ViewPanel.Controls.Add(CreateLabel("Password:", null, new Point(14, 170), 15, FontStyle.Regular, null, null));

            // Password field
            ViewPanel.Controls.Add(CreateTextBox(null, "Password2", new Point(130, 172), 200, true));

            // Update info button
            ViewPanel.Controls.Add(CreateButton("Update", new Point(110, 210), UpdateButtonClicked, null));

            // Checks to see if there are any other users in the system
            if (Database.profiles.Count > 1)
            {
                // "Other Users" label for list of other users
                ViewPanel.Controls.Add(CreateLabel("Other Users", null, new Point(14, 260), 15, FontStyle.Regular, null, null));

                // Left as serial as the systems scope would not make sense for an amount of users that is necessary for parallel to be more efficient
                int location = 290;
                foreach (Profile profile in Database.profiles)
                {
                    if (profile != Database.profile)
                    {
                        // Display other users name
                        ViewPanel.Controls.Add(CreateLabel(profile.GetUsername(), null, new Point(14, location), 10, FontStyle.Regular, ViewProfileClicked, profile));
                        location += 20;
                    }
                }
            }
        }
        private void UpdateButtonClicked(object sender, EventArgs e)
        {
            // Find all user detail fields from the form
            string username = ((TextBox)ViewPanel.Controls.Find("Username", true)[0]).Text;
            string email = ((TextBox)ViewPanel.Controls.Find("Email", true)[0]).Text;
            string password1 = ((TextBox)ViewPanel.Controls.Find("Password1", true)[0]).Text;
            string password2 = ((TextBox)ViewPanel.Controls.Find("Password2", true)[0]).Text;

            // Verifying the information
            if (Database.VerifyInfo(username, email, password1, password2, false))
            {
                // If the data is in a valid format, update it
                Database.profile.UpdateInfo(username, email, password1);
                MessageBox.Show("Information successfully updated");
            }
        }
        private void ViewProfileClicked(object sender, EventArgs e)
        {
            // Displays Visit information for clicked Profile
            Label label = (Label)sender;
            Profile profile = (Profile)label.Tag;
            LoadVisits(profile, false);
        }
        private void DeleteProfile(object sender, EventArgs e)
        {
            // Asks to confirm this action
            DialogResult confirmResult = MessageBox.Show($"This will permentantly delete this Profile, delete all Visits attached to this Profile, and Save the data\n\nAre you sure you want to delete this Profile?", "Confirm Delete", MessageBoxButtons.YesNo);
            
            // If confirmed, delete the data
            if (confirmResult == DialogResult.Yes)
            {
                // Gets the logged in profile
                Profile profile = Database.profile;

                // Remove all associated visits. VisitAttractions are only referenced in these so will be dropped too.
                foreach (Visit visit in profile.GetVisits())
                {
                    Database.visits.Remove(visit);
                }

                // Removes the profile from the database
                Database.profiles.Remove(profile);
                Database.profile = null;

                // Saves this change and returns to the login screen
                Database.SaveData();
                DisplayLogin(null, null);
            }
        }

        private void feedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verifies the user has logged in
            if (Database.profile != null)
            {
                LoadFeed();
            }
        }
        public void LoadFeed()
        {
            ViewPanel.Controls.Clear();

            // "Hello *user*" text
            ViewPanel.Controls.Add(CreateLabel($"Hello, {Database.profile.GetUsername()}", null, new Point(14, 10), 30, FontStyle.Regular, null, null));

            List<Visit> otherVisits = Database.visits.Where(visit => Database.profile.CheckIfDidVisit(visit) == false).ToList();

            if (otherVisits.Count() > 0)
            {
                int location = 90;

                List<Control> controls = new List<Control>();
                object lockObj = new object();

                // Parallel loops through each Visit
                Parallel.ForEach(otherVisits, visit =>
                {
                    Control profileLabel, summaryLabel;
                    int thisLocation;

                    // This locks this object, pausing all other threads that reach this point, allowing the shared location variable to be processed properly
                    lock (lockObj)
                    {
                        thisLocation = location;
                        location += 65;
                    }

                    // Visitor and park
                    profileLabel= CreateLabel($"{visit.GetProfile().GetUsername()} visited {visit.GetPark().GetName()} on {visit.GetDate()}", null, new Point(20, thisLocation), 15, FontStyle.Regular, null, null);

                    // Visit activity
                    summaryLabel = CreateLabel($"They got on {visit.GetAttractionCount()} ride{(visit.GetAttractionCount() == 1 ? "" : "s")}", null, new Point(20, thisLocation + 25), 10, FontStyle.Regular, null, null);

                    // Add controls to the list of controls to be added
                    lock (lockObj)
                    {
                        controls.Add(profileLabel);
                        controls.Add(summaryLabel);
                    }
                });

                // Add all controls
                Invoke(() =>
                {
                    // AddRange allows them to be added in bulk for further optimisation
                    ViewPanel.Controls.AddRange(controls.ToArray());
                });
            }
            else
            {
                // No activity message
                ViewPanel.Controls.Add(CreateLabel("No activity from other users!", null, new Point(50, 317), 10, FontStyle.Regular, null, null));
            }
        }

        private void visitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verifies the user has logged in
            if (Database.profile != null)
            {
                LoadVisits(Database.profile, true);
            }
        }
        public void LoadVisits(Profile profile, bool edit)
        {
            ViewPanel.Controls.Clear();

            int location = 14;

            // Edit is true if the visit belongs to the logged in user
            if (edit)
            {
                // New Visit button
                ViewPanel.Controls.Add(CreateButton("New Visit", new Point(14, 14), NewVisit, null));

                location = 74;
            }

            if (Database.visits.Count > 0)
            {
                // Parallel was quicker so I wanted to include it, but the location variable being shared was an issue, so I found this solution using lock

                List<Control> controls = new List<Control>();
                object lockObj = new object();

                // Parallel loops through each Visit
                Parallel.ForEach(profile.GetVisits(), visit =>
                {
                    Control parkLabel, summaryLabel, partition = null;
                    int thisLocation;

                    // This locks this object, pausing all other threads that reach this point, allowing the shared location variable to be processed properly
                    lock (lockObj)
                    {
                        thisLocation = location;
                        location += 50;
                    }

                    // Visit info with view route
                    parkLabel = CreateLabel($"{visit.GetPark().GetName()} - {visit.GetDate()}", null, new Point(14, thisLocation), 20, FontStyle.Regular, ViewVisitClicked, visit);

                    // Visit summary
                    summaryLabel = CreateLabel($"-- {visit.GetAttractionCount()} Ride{(visit.GetAttractionCount() == 1 ? "" : "s")} -- {visit.GetUniqueAttractionCount()} Unique Ride{(visit.GetUniqueAttractionCount() == 1 ? "" : "s")} --", null, new Point(1000, thisLocation + 8), 10, FontStyle.Regular, null, null);
                    
                    if (thisLocation != 14 && thisLocation != 74)
                    {
                        // Partition between Visits
                        partition = CreatePartition(thisLocation - 10);
                    }

                    // Add controls to the list of controls to be added
                    lock (lockObj)
                    {
                        controls.Add(parkLabel);
                        controls.Add(summaryLabel);
                        if (partition != null) controls.Add(partition);
                    }
                });

                // Add all controls
                Invoke(() =>
                {
                    // AddRange allows them to be added in bulk for further optimisation
                    ViewPanel.Controls.AddRange(controls.ToArray());
                });


                // TESTS
                //
                // At 1,000 Visits
                // Parallel: 7937
                // Serial: 11749
                //
                // At 100 Visits
                // Parallel: 335
                // Serial: 440
                //
                // At 10 Visits
                // Parallel: 43
                // Serial: 40
                //
                // Conclusion: Serial is only efficient at very small values, so I'll keep outputs like this as Parallel
                // The commented code below is what I used for the Serial test
                //

                //foreach (Visit visit in profile.GetVisits())
                //{
                //    // Visit info with view route
                //    ViewPanel.Controls.Add(CreateLabel($"{visit.GetPark().GetName()} - {visit.GetDate()}", null, new Point(14, location), 20, FontStyle.Regular, ViewVisitClicked, visit));

                //    // Visit summary
                //    ViewPanel.Controls.Add(CreateLabel($"-- {visit.GetAttractionCount()} Ride{(visit.GetAttractionCount() == 1 ? "" : "s")} -- {visit.GetUniqueAttractionCount()} Unique Ride{(visit.GetUniqueAttractionCount() == 1 ? "" : "s")} --", null, new Point(1000, location + 8), 10, FontStyle.Regular, null, null));

                //    if (location != 14 && location != 74)
                //    {
                //        // Partition between Visits
                //        ViewPanel.Controls.Add(CreatePartition(location - 10));
                //    }

                //    location += 50;
                //}
            }
            else
            {
                if (profile == Database.profile)
                {
                    // No activity message for user
                    ViewPanel.Controls.Add(CreateLabel("No Visits to display. Click 'New Visit' to get started!", null, new Point(50, 317), 10, FontStyle.Regular, null, null));
                }
                else
                {
                    // No activity message for other user
                    ViewPanel.Controls.Add(CreateLabel("This user doesn't have any visits yet!", null, new Point(50, 317), 10, FontStyle.Regular, null, null));
                }
            }
        }
        private void ViewVisitClicked(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            Visit visit = null;
            bool edit = false;

            // Visit can be reached through clicking to view, and by saving the details
            // So it needs to be able to get the visit from either
            try
            {
                Label clickedLabel = (Label)sender;
                visit = (Visit)clickedLabel.Tag;
            }
            catch
            {
                Button clickedButton = (Button)sender;
                visit = (Visit)clickedButton.Tag;
            }

            // Finds if the visit belongs to the logged in user
            edit = false;
            if (visit.GetProfile() == Database.profile)
            {
                edit = true;
            }

            // Edit is true if the visit belongs to the logged in user
            if (edit)
            {
                // Button to edit visit details
                ViewPanel.Controls.Add(CreateButton("Edit visit", new Point(1120, 14), EditVisit, visit));
            }

            // Park name
            ViewPanel.Controls.Add(CreateLabel(visit.GetPark().GetName(), null, new Point(14, 14), 20, FontStyle.Bold, null, null));

            // Visit date
            ViewPanel.Controls.Add(CreateLabel(visit.GetDate().ToString(), null, new Point(14, 43), 15, FontStyle.Bold, null, null));

            // Visitor name
            ViewPanel.Controls.Add(CreateLabel($"{visit.GetProfile().GetUsername()}'s visit", null, new Point(14, 65), 15, FontStyle.Regular, null, null));

            // Unique Rides count
            ViewPanel.Controls.Add(CreateLabel($"Unique Rides: {visit.GetUniqueAttractionCount()}", null, new Point(300, 25), 15, FontStyle.Regular, null, null));

            // Total Rides count
            ViewPanel.Controls.Add(CreateLabel($"Total Rides: {visit.GetAttractionCount()}", null, new Point(300, 50), 15, FontStyle.Regular, null, null));

            int location = 130;

            // Displays all Rides
            List<VisitAttraction> attractions = visit.GetAttractions().OrderBy(attraction => attraction.GetOrder()).ToList().ToList();
            if (attractions.Count > 0)
            {
                // "Rides" text
                ViewPanel.Controls.Add(CreateLabel("Rides", null, new Point(14, 103), 15, FontStyle.Bold, null, null));

                foreach (VisitAttraction attraction in attractions)
                {
                    // Ride order and name
                    ViewPanel.Controls.Add(CreateLabel($"{attraction.GetOrder()}. {attraction.GetAttraction().GetName(visit.GetDate())}", null, new Point(14, location), 13, FontStyle.Regular, ViewRideClicked, attraction.GetAttraction()));

                    if (location != 130)
                    {
                        // Partition between Rides
                        ViewPanel.Controls.Add(CreatePartition(location - 10));
                    }

                    // Gets wait time info
                    int waitTime = attraction.GetWaitTime();
                    int hours = waitTime / 60, minutes = waitTime % 60;
                    if (waitTime != -1)
                    {
                        // Displays wait time
                        ViewPanel.Controls.Add(CreateLabel($"Wait Time: {(hours != 0 ? $"{hours} hour{(hours == 1 ? "" : "s")}" : "")}{(hours != 0 && minutes != 0 ? " and " : "")}{(((minutes != -1 && hours == 0) || minutes != 0) ? $"{minutes} minute{(minutes == 1 ? "" : "s")}" : "")}", null, new Point(300, location + 2), 10, FontStyle.Regular, null, null));
                    }

                    // Displays wait time info
                    TimeOnly time = attraction.GetTime();
                    if (time != TimeOnly.Parse("3:00 am"))
                    {
                        // Displays enter time
                        ViewPanel.Controls.Add(CreateLabel($"Enter Time: {time}", null, new Point(600, location + 2), 10, FontStyle.Regular, null, null));
                    }

                    location += 30;
                }
            }
        }
        private void NewVisit(object sender, EventArgs e)
        {
            // You can only add a new visit if there are theme parks tracked
            if (Database.parks.Count > 0)
            {
                // Creates a new Visit with default values
                Visit visit = new Visit(Database.GetNextVisitID(), DateOnly.FromDateTime(DateTime.Now), Database.profile, Database.parks[0]);
                Button buttonClicked = (Button)sender;
                buttonClicked.Tag = visit;
                sender = (object)buttonClicked;
                EditVisit(sender, e);
            }
            else
            {
                MessageBox.Show("Please add a theme park");
            }
        }
        private void EditVisit(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            Button clickedButton = (Button)sender;
            Visit visit = (Visit)clickedButton.Tag;

            // Saves Visit
            ViewPanel.Controls.Add(CreateButton("Save Visit", new Point(1120, 14), SaveVisit, visit));

            // Deleted Visit
            ViewPanel.Controls.Add(CreateButton("Delete Visit", new Point(1120, 44), DeleteVisit, visit));

            // "Theme Park" text
            ViewPanel.Controls.Add(CreateLabel("Theme Park:", null, new Point(14, 14), 10, FontStyle.Regular, null, null));

            // ComboBox to display a list of theme parks
            ComboBox comboBox = new ComboBox();
            Database.parks.OrderBy(park => park.GetName());
            foreach (Park park in Database.parks)
            {
                comboBox.Items.Add(park.GetName());
                if (visit != null)
                {
                    if (park == visit.GetPark())
                    {
                        comboBox.SelectedIndex = comboBox.Items.Count - 1;
                    }
                }
            }
            comboBox.SelectedIndexChanged += ThemeParkChangedOnVisit;
            comboBox.Location = new Point(105, 12);
            comboBox.Size = new Size(200, 10);
            comboBox.Name = "Park";
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ViewPanel.Controls.Add(comboBox);

            // "Date" text
            ViewPanel.Controls.Add(CreateLabel("Date:", null, new Point(14, 35), 10, FontStyle.Regular, null, null));

            // DateTimePicker to pick the date of the visit
            DateTimePicker datePicker = new DateTimePicker();
            datePicker.Location = new Point(105, 33);
            if (visit != null)
            {
                datePicker.Value = visit.GetDate().ToDateTime(TimeOnly.Parse("10:00 PM"));
            }
            datePicker.Name = "Date";
            ViewPanel.Controls.Add(datePicker);

            // ComboBox to show attractions in that park
            int selectedPark = comboBox.SelectedIndex;
            List<Attraction> attractions = Database.GetAttractionByPark(Database.GetParkByID(selectedPark + 1).GetID()).OrderBy(attraction => attraction.GetID()).ToList();
            comboBox = new ComboBox();
            foreach (Attraction attraction in attractions)
            {
                comboBox.Items.Add(attraction.GetName(DateOnly.FromDateTime(DateTime.Now)));
            }
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
            comboBox.Location = new Point(105, 71);
            comboBox.Width = 200;
            comboBox.Name = "combo1";
            comboBox.Tag = Database.GetParkByID(selectedPark + 1);
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ViewPanel.Controls.Add(comboBox);

            // Adds Ride
            ViewPanel.Controls.Add(CreateButton("Add Ride", new Point(14, 70), AttractionClicked, comboBox));

            foreach (VisitAttraction attraction in visit.GetAttractions())
            {
                clickedButton.Tag = attraction;
                sender = (object)clickedButton;
                AttractionClicked(sender, e);
            }
        }
        public void SaveVisit(object sender, EventArgs e)
        {
            // Getting the visit that needs to be saved
            Button clickedButton = (Button)sender;
            Visit visit = (Visit)clickedButton.Tag;
            clickedButton.Tag = visit;
            sender = (object)clickedButton;

            // Get selected park
            ComboBox comboBox = (ComboBox)ViewPanel.Controls.Find("Park", true)[0];
            visit.SetPark(Database.GetParkByID(comboBox.SelectedIndex + 1));

            // Get selected date
            DateTimePicker datePicker = (DateTimePicker)ViewPanel.Controls.Find("Date", true)[0];
            visit.SetDate(DateOnly.FromDateTime(datePicker.Value));

            // Save each attraction to the visit
            List<VisitAttraction> attractions = new List<VisitAttraction>();
            foreach (Panel attractionPanel in ViewPanel.Controls.Find("Panel", true))
            {
                // Get the attraction
                Attraction attraction = (Attraction)attractionPanel.Controls.Find("Attraction", true)[0].Tag;

                // Get the order in the visit
                int order = ((attractionPanel.Location.Y - 112) / 40) + 1;

                // Get The wait time if selected
                int waitTime = -1;
                CheckBox checkBox = (CheckBox)attractionPanel.Controls.Find("WaitTimeCheckBox", true)[0];
                if (checkBox.Checked)
                {
                    NumericUpDown numericUpDown = (NumericUpDown)attractionPanel.Controls.Find("WaitTime", true)[0];
                    waitTime = Convert.ToInt32(Math.Round(numericUpDown.Value, 0));
                }

                // Get the entry time if selected
                TimeOnly time = TimeOnly.Parse("3:00:00");
                checkBox = (CheckBox)attractionPanel.Controls.Find("EntryTimeCheckBox", true)[0];
                if (checkBox.Checked)
                {
                    DateTimePicker dateTimePicker = (DateTimePicker)attractionPanel.Controls.Find("EntryTime", true)[0];
                    time = TimeOnly.Parse($"{dateTimePicker.Value.Hour}:{dateTimePicker.Value.Minute}:{dateTimePicker.Value.Second}");
                }

                // Adds the attraction to the list
                attractions.Add(new VisitAttraction(attraction, order, time, waitTime));
            }

            // Sets the visits attractions
            visit.SetAttractions(attractions);

            // Checks to see if it needs adding or if it's being updated
            if (!Database.visits.Contains(visit))
            {
                Database.visits.Add(visit);
                Database.profile.AddVisit(visit);
            }
            
            // Returns the details page for this visit
            ViewVisitClicked(sender, e);
        }
        public void DeleteVisit(object sender, EventArgs e)
        {
            // Gets the visit to be deleted
            Button button = (Button)sender;
            Visit visit = (Visit)button.Tag;

            // Asks to confirm this action
            DialogResult confirmResult = MessageBox.Show($"This will permentantly delete this Visit\n\nAre you sure you want to delete this Visit?", "Confirm Delete", MessageBoxButtons.YesNo);

            // If confirmed, delete the data
            if (confirmResult == DialogResult.Yes)
            {
                Database.visits.Remove(visit);
                Database.profile.RemoveVisit(visit);

                // Deletes the visit and returns to a list of all visits
                LoadVisits(Database.profile, true);
            }
        }
        public void AttractionClicked(object sender, EventArgs e)
        {
            // Gets the attraction being clicked
            Attraction attraction = null;
            VisitAttraction visitAttraction = null;
            Button clickedButton = (Button)sender;

            bool con = true;
            try
            {
                // Gets the attraction from a ComboBox if added manually
                ComboBox comboBox = (ComboBox)clickedButton.Tag;
                Park park = (Park)comboBox.Tag;
                List<Attraction> attractions = Database.GetAttractionByPark(park.GetID()).OrderBy(attraction => attraction.GetID()).ToList();
                attraction = attractions[comboBox.SelectedIndex];
            }
            catch (Exception ex1)
            {
                try
                {
                    // If the ComboBox can't be case, it must be a visit attraction
                    visitAttraction = (VisitAttraction)clickedButton.Tag;
                    attraction = visitAttraction.GetAttraction();
                }
                catch (Exception ex2)
                {
                    con = false;
                }
            }

            // If the attraction was successfully cast
            if (con)
            {
                // Creates a panel to display the info of the attraction
                Panel panel = new Panel();
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.Size = new Size(900, 35);
                panel.Location = new Point(14, (ViewPanel.Controls.Count - 8) * 40 + 112 + ViewPanel.AutoScrollPosition.Y);
                panel.Name = "Panel";
                ViewPanel.Controls.Add(panel);

                // Displays attractions name
                panel.Controls.Add(CreateLabel(attraction.GetName(DateOnly.FromDateTime(DateTime.Now)), "Attraction", new Point(9, 9), 10, FontStyle.Bold, null, attraction));

                // "Wait time" text
                panel.Controls.Add(CreateLabel("Wait time:", null, new Point(239, 9), 10, FontStyle.Regular, null, null));

                // Selects the wait time
                NumericUpDown numericUpDown = new NumericUpDown();
                numericUpDown.Location = new Point(309, 5);
                numericUpDown.Width = 40;
                numericUpDown.Maximum = 999;
                numericUpDown.Name = "WaitTime";
                panel.Controls.Add(numericUpDown);

                // A CheckBox to signify is Wait Time is being used
                CheckBox checkBox = new CheckBox();
                checkBox.Location = new Point(359, 6);
                checkBox.Tag = numericUpDown;
                checkBox.Name = "WaitTimeCheckBox";
                panel.Controls.Add(checkBox);

                // Sets wait time if the visit being loaded is already saved to a value
                if (visitAttraction != null)
                {
                    if (visitAttraction.GetWaitTime() != -1)
                    {
                        numericUpDown.Value = visitAttraction.GetWaitTime();
                        checkBox.Checked = true;
                    }
                }

                // "Entry time" text
                panel.Controls.Add(CreateLabel("Entry time:", null, new Point(469, 9), 10, FontStyle.Regular, null, null));

                // Selects the entry time
                DateTimePicker timePicker = new DateTimePicker();
                timePicker.Format = DateTimePickerFormat.Time;
                timePicker.Location = new Point(549, 5);
                timePicker.Width = 67;
                timePicker.ShowUpDown = true;
                timePicker.Name = "EntryTime";
                panel.Controls.Add(timePicker);

                // A CheckBox to signify is Entry Time is being used
                checkBox = new CheckBox();
                checkBox.Location = new Point(626, 10);
                checkBox.Tag = timePicker;
                checkBox.AutoSize = true;
                checkBox.Name = "EntryTimeCheckBox";
                panel.Controls.Add(checkBox);

                // Sets entry time if the visit being loaded is already saved to a value
                if (visitAttraction != null)
                {
                    if (visitAttraction.GetTime() != TimeOnly.Parse("3:00:00"))
                    {
                        timePicker.Value = DateOnly.FromDateTime(DateTime.Now).ToDateTime(visitAttraction.GetTime());

                        checkBox.Checked = true;
                    }
                }

                // Move item up text
                panel.Controls.Add(CreateButton("/\\", new Point(660, 5), MoveAttractionUp, panel));

                // Move item down text
                panel.Controls.Add(CreateButton("\\/", new Point(740, 5), MoveAttractionDown, panel));

                // Delete item text
                panel.Controls.Add(CreateButton("Delete", new Point(820, 5), DeleteAttraction, panel));
            }
        }
        public void MoveAttractionUp(object sender, EventArgs e)
        {
            // Gets parent panel
            Button clickedButton = (Button)sender;
            Panel panel = (Panel)clickedButton.Tag;
            int location = panel.Location.Y;
            location -= 40;

            // Gets a list of all panels in the form
            object[] panels = ViewPanel.Controls.Find("Panel", true);

            // Only needs to compare if there are more panels that it could interact with
            if (panels.Length > 1)
            {
                // Check position for each panel
                foreach (Control controlPanel in panels)
                {
                    // Cast the control to a panel
                    Panel panelCheck = (Panel)controlPanel;

                    // If the panel is in the intended position, swap coordinates
                    if (panelCheck.Location.Y == location)
                    {
                        panelCheck.Location = new Point(panelCheck.Location.X, panelCheck.Location.Y + 40);
                        panel.Location = new Point(panelCheck.Location.X, panelCheck.Location.Y - 40);
                    }
                }
            }
        }
        public void MoveAttractionDown(object sender, EventArgs e)
        {
            // Gets parent panel
            Button clickedButton = (Button)sender;
            Panel panel = (Panel)clickedButton.Tag;
            int location = panel.Location.Y;
            location += 40;

            // Gets a list of all panels in the form
            object[] panels = ViewPanel.Controls.Find("Panel", true);

            // Only needs to compare if there are more panels that it could interact with
            if (panels.Length > 1)
            {
                // Check position for each panel
                foreach (Control controlPanel in panels)
                {
                    // Cast the control to a panel
                    Panel panelCheck = (Panel)controlPanel;

                    // If the panel is in the intended position, swap coordinates
                    if (panelCheck.Location.Y == location)
                    {
                        panelCheck.Location = new Point(panelCheck.Location.X, panelCheck.Location.Y - 40);
                        panel.Location = new Point(panelCheck.Location.X, panelCheck.Location.Y + 40);
                    }
                }
            }
        }
        public void DeleteAttraction(object sender, EventArgs e)
        {
            // Gets parent panel;
            Button clickedButton = (Button)sender;
            Panel panel = (Panel)clickedButton.Tag;
            int location = panel.Location.Y;

            // Removes panel
            ViewPanel.Controls.Remove(panel);

            // Gets a list of all panels in the form
            object[] panels = ViewPanel.Controls.Find("Panel", true);

            // Checks position for each panel
            foreach (Control controlPanel in panels)
            {
                // Cast the control to a panel
                Panel panelCheck = (Panel)controlPanel;

                // If the panel is above the removed one, move it up
                if (panelCheck.Location.Y > location)
                {
                    panelCheck.Location = new Point(panelCheck.Location.X, panelCheck.Location.Y - 40);
                }
            }
        }
        public void ThemeParkChangedOnVisit(object sender, EventArgs e)
        {
            // Gets the updated park
            ComboBox comboBox = (ComboBox)sender;
            Park selectedPark = Database.GetParkByID(comboBox.SelectedIndex + 1);
            List<Attraction> attractions = Database.GetAttractionByPark(selectedPark.GetID()).OrderBy(attraction => attraction.GetID()).ToList();

            // Gets the list of attractions
            comboBox = (ComboBox)ViewPanel.Controls.Find("combo1", true)[0]; 
            Park originalPark = (Park)comboBox.Tag;
            
            // Checks if the park has changed
            if (selectedPark != originalPark)
            {
                // Clears the list
                comboBox.Items.Clear();
                comboBox.Tag = selectedPark;
                if (attractions.Count > 0)
                {
                    // Puts each ride in the park into the ComboBox
                    foreach (Attraction attraction in attractions)
                    {
                        comboBox.Items.Add(attraction.GetName(DateOnly.FromDateTime(DateTime.Now)));
                    }
                    comboBox.SelectedIndex = 0;
                }
                else
                {
                    // Makes sure a previous ride isn't left displayed in there
                    comboBox.Items.Add("");
                    comboBox.SelectedIndex = 0;
                }

                // Removes all rides currently in the visit
                object[] controls = Controls.Find("Panel", true);
                foreach (object control in controls)
                {
                    Panel panel = (Panel)control;
                    ViewPanel.Controls.Remove(panel);
                }
            }
        }

        private void themeParksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verifies the user has logged in
            if (Database.profile != null)
            {
                LoadThemeParks();
            }
        }
        public void LoadThemeParks()
        {
            ViewPanel.Controls.Clear();

            // New Park button
            ViewPanel.Controls.Add(CreateButton("New Park", new Point(14, 14), NewPark, null));

            if (Database.parks.Count != 0)
            {
                int location = 74;

                List<Control> controls = new List<Control>();
                object lockObj = new object();

                // Parallel loops through each Park
                Parallel.ForEach(Database.parks, park =>
                {
                    Control parkLabel, summaryLabel, partition = null;
                    int thisLocation;

                    // This locks this object, pausing all other threads that reach this point, allowing the shared location variable to be processed properly
                    lock (lockObj)
                    {
                        thisLocation = location;
                        location += 50;
                    }

                    // Park Name
                    parkLabel = CreateLabel(park.GetName(), null, new Point(14, thisLocation), 20, FontStyle.Regular, ViewThemeParkClicked, park);

                    // Rides in park count
                    int parkRides = Database.GetAttractionByPark(park.GetID()).Count;
                    summaryLabel = CreateLabel($"{parkRides} ride{(parkRides == 1 ? "" : "s")} tracked", null, new Point(1080, thisLocation + 8), 10, FontStyle.Regular, null, null);

                    if (thisLocation != 74)
                    {
                        // Partition between Parks
                        partition = CreatePartition(thisLocation - 10);
                    }

                    // Add controls to the list of controls to be added
                    lock (lockObj)
                    {
                        controls.Add(parkLabel);
                        controls.Add(summaryLabel);
                        if (partition != null) controls.Add(partition);
                    }
                });

                // Add all controls
                Invoke(() =>
                {
                    // AddRange allows them to be added in bulk for further optimisation
                    ViewPanel.Controls.AddRange(controls.ToArray());
                });
            }
            else
            {
                // No activity message
                ViewPanel.Controls.Add(CreateLabel("No Theme Parks to display. Click 'New Park' to get started!", null, new Point(50, 317), 10, FontStyle.Regular, null, null));
            }
        }
        public void ViewThemeParkClicked(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            Park park = null;

            // Gets the Park, trying for both places that could call this
            try
            {
                Label labelClicked = (Label)sender;
                park = (Park)labelClicked.Tag;
            }
            catch
            {
                try
                {
                    Button clickedButton = (Button)sender;
                    park = (Park)clickedButton.Tag;
                }
                catch { }
            }

            // Edit Park button
            ViewPanel.Controls.Add(CreateButton("Edit Park", new Point(1120, 14), EditPark, park));

            // Displays Park name
            ViewPanel.Controls.Add(CreateLabel(park.GetName(), null, new Point(14, 14), 20, FontStyle.Bold, null, null));

            // Total Rides count
            ViewPanel.Controls.Add(CreateLabel($"Total Rides: {(park.GetAttractions() == null ? "0" : park.GetAttractions().Count)}", null, new Point(700, 18), 15, FontStyle.Regular, null, null));

            // Total Visits count
            ViewPanel.Controls.Add(CreateLabel($"Total Visits: {(Database.GetVisitsByParkID(park.GetID()).Count == null ? "0" : Database.GetVisitsByParkID(park.GetID()).Count)}", null, new Point(900, 18), 15, FontStyle.Regular, null, null));

            int location = 92;

            if (park.GetAttractions() != null)
            {
                // Outputs a list of all Attractions tracked to the selected Park
                List<Attraction> attractions = park.GetAttractions().OrderBy(attraction => attraction.GetName(DateOnly.FromDateTime(DateTime.Now))).ToList();

                if (attractions.Count > 0)
                {
                    // "Rides" text
                    ViewPanel.Controls.Add(CreateLabel("Rides", null, new Point(14, 65), 15, FontStyle.Bold, null, null));

                    foreach (Attraction attraction in attractions)
                    {
                        // Displays Attraction name
                        ViewPanel.Controls.Add(CreateLabel($"{attraction.GetName(DateOnly.FromDateTime(DateTime.Now))}", null, new Point(14, location), 13, FontStyle.Regular, ViewRideClicked, attraction));

                        if (location != 92)
                        {
                            // Partition between Attractions
                            ViewPanel.Controls.Add(CreatePartition(location - 5));
                        }

                        // Ride type
                        ViewPanel.Controls.Add(CreateLabel($"{(attraction.GetElements()[0] == "1" ? "Rollercoaster" : $"{(attraction.GetElements()[0] == "3" ? "Flat Ride" : "Dark Ride")}")}", null, new Point(300, location + 2), 10, FontStyle.Regular, null, null));

                        location += 30;
                    }
                }
            }

            // Gets all Attractions in a Park
            HashSet<Attraction> allAttractions = Database.GetAttractionByPark(park.GetID()).ToHashSet();

            // Gets all Attractions in a Park that the user has visiited
            HashSet<Attraction> visitedAttractions = new HashSet<Attraction>();
            foreach (Visit visit in Database.profile.GetVisits())
            {
                if (visit.GetPark() == park)
                {
                    foreach (VisitAttraction visitAttraction in visit.GetAttractions())
                    {
                        visitedAttractions.Add(visitAttraction.GetAttraction());
                    }
                }
            }

            // Removes visited Attractions from all Attractions
            HashSet<Attraction> notVisited = new HashSet<Attraction>(allAttractions);
            notVisited.ExceptWith(visitedAttractions);

            if (notVisited.Count == 0)
            {
                // Displays all visited message
                ViewPanel.Controls.Add(CreateLabel("You have visited all of the rides tracked to this park", null, new Point(14, location), 10, FontStyle.Regular, null, null));
            }
            else
            {
                // Displays all visited Attractions
                ViewPanel.Controls.Add(CreateLabel("Rides you haven't been on", null, new Point(14, location), 15, FontStyle.Bold, null, null));
                location += 30;

                foreach (Attraction attraction in notVisited)
                {
                    // Missing Ride
                    ViewPanel.Controls.Add(CreateLabel($"{attraction.GetName(DateOnly.FromDateTime(DateTime.Now))}", null, new Point(14, location), 10, FontStyle.Regular, ViewRideClicked, attraction));
                    location += 20;
                }
            }
        }
        public void NewPark(object sender, EventArgs e)
        {
            // Creates a new Park with default values
            Park park = new Park(Database.GetNextParkID(), "");
            Button button = (Button)sender;
            button.Tag = park;
            sender = (object)button;
            EditPark(sender, e);
        }
        public void EditPark(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            Button clickedButton = (Button)sender;
            Park park = (Park)clickedButton.Tag;

            // Save Park button
            ViewPanel.Controls.Add(CreateButton("Save park", new Point(1120, 14), SavePark, park));

            // Delete Park button
            ViewPanel.Controls.Add(CreateButton("Delete park", new Point(1120, 44), DeletePark, park));

            // "Name" text
            ViewPanel.Controls.Add(CreateLabel("Name:", null, new Point(14, 14), 10, FontStyle.Regular, null, null));

            // Park name field
            ViewPanel.Controls.Add(CreateTextBox($"{(park == null ? "" : park.GetName())}", "Name", new Point(65, 12), 200, false));

            if (Database.parks.Contains(park))
            {
                // New Attraction Button
                ViewPanel.Controls.Add(CreateButton("New attraction", new Point(14, 50), NewRide, park));

                int location = 80;

                // Displays all Attractions tracked to the Park
                foreach (Attraction attraction in park.GetAttractions())
                {
                    // Panel for the Attraction
                    Panel panel = new Panel();
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Size = new Size(900, 35);
                    panel.Location = new Point(14, location);
                    ViewPanel.Controls.Add(panel);

                    // Display Attraction name
                    panel.Controls.Add(CreateLabel(attraction.GetName(DateOnly.FromDateTime(DateTime.Now)), null, new Point(9, 9), 10, FontStyle.Bold, null, attraction));

                    // Delete Attraction button
                    panel.Controls.Add(CreateButton("Delete", new Point(820, 5), RemoveRideFromPark, attraction));

                    location += 40;
                }
            }
            else
            {
                // Instruction to save park before adding attractions
                ViewPanel.Controls.Add(CreateLabel("Please save park before adding attractions", null, new Point(14, 50), 9, FontStyle.Regular, null, null));
            }
        }
        public void SavePark(object sender, EventArgs e)
        {
            // Gets Park being saved
            Button clickedButton = (Button)sender;
            Park park = (Park)clickedButton.Tag;

            // Updates name
            TextBox textBox = (TextBox)ViewPanel.Controls.Find("Name", true)[0];
            bool valid = textBox.Text.Trim() != "";
            if (valid) park.SetName(textBox.Text);

            // Adds the Park if it is being created
            bool newPark = false;
            if (!Database.parks.Contains(park)) newPark = true;

            if (!Database.parks.Contains(park) && valid)
            {
                Database.parks.Add(park);
            }

            if (!valid) MessageBox.Show("Invalid name");

            if (!valid && newPark) LoadThemeParks();
            else ViewThemeParkClicked(sender, e);
        }
        public void DeletePark(object sender, EventArgs e)
        {
            // Gets the Park being deleted
            Button button = (Button)sender;
            Park park = (Park)button.Tag;

            // Gets associated data
            List<Attraction> attractions = Database.GetAttractionByPark(park.GetID());
            List<Visit> visits = Database.GetVisitsByParkID(park.GetID());

            if (attractions.Count > 0 || visits.Count > 0)
            {
                // Asks to confirm delete if there are Visits and/or Attractions associated with the Park
                DialogResult confirmResult = MessageBox.Show($"This will\n- Delete {attractions.Count} Attraction{(attractions.Count > 0 ? "s" : "")}\n- Delete {visits.Count} Visit{(visits.Count > 0 ? "s" : "")}\n\nAre you sure you want to delete the Park {park.GetName()}?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    // Removes park and it's associated data
                    Database.parks.Remove(park);
                    foreach (Attraction attraction in attractions)
                    {
                        Database.attractions.Remove(attraction);
                    }
                    foreach (Visit visit in visits)
                    {
                        // VistAttractions are refered to in Visit, so are deleted along with the visit
                        Database.visits.Remove(visit);
                    }
                }
            }
            else
            {
                // Deletes it without asking if there is no associated data
                Database.parks.Remove(park);
            }

            LoadThemeParks();
        }
        public void RemoveRideFromPark(object sender, EventArgs e)
        {
            // Gets the Attraction and Park
            Button button = (Button)sender;
            Attraction attraction = (Attraction)button.Tag;
            Park park = attraction.GetPark();

            // Removes the Ride, leaving it unreferenced to be removed and not saved
            park.RemoveAttraction(attraction);
            Database.attractions.Remove(attraction);

            button.Tag = park;
            sender = (object)button;
            EditPark(sender, e);
        }

        private void manufacturersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verifies the user has logged in
            if (Database.profile != null)
            {
                LoadManufacturers();
            }
        }
        public void LoadManufacturers()
        {
            ViewPanel.Controls.Clear();

            // New Manufacturer button
            ViewPanel.Controls.Add(CreateButton("New Manufacturer", new Point(14, 14), NewManufacturer, null));

            if (Database.manufacturers.Count != 0)
            {
                int location = 74;

                List<Control> controls = new List<Control>();
                object lockObj = new object();

                // Parallel loops through each Manufacturer
                Parallel.ForEach(Database.manufacturers, manufacturer =>
                {
                    Control manufacturerLabel, summaryLabel, partition = null;
                    int thisLocation;

                    // This locks this object, pausing all other threads that reach this point, allowing the shared location variable to be processed properly
                    lock (lockObj)
                    {
                        thisLocation = location;
                        location += 50;
                    }

                    // Displays Manufacturer name
                    manufacturerLabel = CreateLabel(manufacturer.GetName(), null, new Point(14, thisLocation), 20, FontStyle.Regular, ViewManufacturerClicked, manufacturer);

                    // Manufacturers RideType count
                    int manufacturerModels = Database.GetRideTypesByManufacturerID(manufacturer.GetID()).Count;
                    summaryLabel = CreateLabel($"{manufacturerModels} model{(manufacturerModels == 1 ? "" : "s")} tracked", null, new Point(1085, thisLocation + 8), 10, FontStyle.Regular, null, null);

                    if (thisLocation != 74)
                    {
                        // Partition between Manufacturers
                        partition = CreatePartition(thisLocation - 10);
                    }

                    // Add controls to the list of controls to be added
                    lock (lockObj)
                    {
                        controls.Add(manufacturerLabel);
                        controls.Add(summaryLabel);
                        if (partition != null) controls.Add(partition);
                    }
                });

                // Add all controls
                Invoke(() =>
                {
                    // AddRange allows them to be added in bulk for further optimisation
                    ViewPanel.Controls.AddRange(controls.ToArray());
                });
            }
            else
            {
                // No activity message
                ViewPanel.Controls.Add(CreateLabel("No Manufacturers to display. Click 'New Manufacturer' to get started!", null, new Point(50, 317), 10, FontStyle.Regular, null, null));
            }

        }
        public void ViewManufacturerClicked(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            Manufacturer manufacturer = null;

            // Gets the Manufacturer, trying for both places that could call this
            try
            {
                Label labelClicked = (Label)sender;
                manufacturer = (Manufacturer)labelClicked.Tag;
            }
            catch
            {
                try
                {
                    Button clickedButton = (Button)sender;
                    manufacturer = (Manufacturer)clickedButton.Tag;
                }
                catch { }
            }

            // Edit Manufacturer button
            ViewPanel.Controls.Add(CreateButton("Edit Manufacturer", new Point(1080, 14), EditManufacturer, manufacturer));

            // Manufacturers name
            ViewPanel.Controls.Add(CreateLabel(manufacturer.GetName(), null, new Point(14, 14), 20, FontStyle.Bold, null, null));

            // Manufacturers RideType count
            ViewPanel.Controls.Add(CreateLabel($"Total Models: {(manufacturer.GetRideTypes() == null ? "0" : manufacturer.GetRideTypes().Count)}", null, new Point(400, 18), 15, FontStyle.Regular, null, null));

            // "Models" text
            ViewPanel.Controls.Add(CreateLabel("Models", null, new Point(14, 65), 15, FontStyle.Bold, null, null));

            int location = 92;

            if (manufacturer.GetRideTypes() != null)
            {
                List<RideType> rideTypes = manufacturer.GetRideTypes().OrderBy(rideTypes => rideTypes.GetName()).ToList();

                foreach (RideType rideType in rideTypes)
                {
                    // RideType name
                    ViewPanel.Controls.Add(CreateLabel($"{rideType.GetName()}", null, new Point(14, location), 13, FontStyle.Regular, ViewRideTypeClicked, rideType));

                    if (location != 92)
                    {
                        // Partition between RideTypes
                        ViewPanel.Controls.Add(CreatePartition(location - 5));
                    }

                    location += 30;
                }
            }
        }
        public void NewManufacturer(object sender, EventArgs e)
        {
            // Creates a new Manufacturer with default values
            Manufacturer manufacturer = new Manufacturer(Database.GetNextManufacturerID(), "");
            Button button = (Button)sender;
            button.Tag = manufacturer;
            sender = (object)button;
            EditManufacturer(sender, e);
        }
        public void EditManufacturer(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            Button clickedButton = (Button)sender;
            Manufacturer manufacturer = (Manufacturer)clickedButton.Tag;

            // Save Manufacturer
            ViewPanel.Controls.Add(CreateButton("Save manufacturer", new Point(1070, 14), SaveManufacturer, manufacturer));

            // Delete Manufacturer
            ViewPanel.Controls.Add(CreateButton("Delete manufacturer", new Point(1066, 44), DeleteManufacturer, manufacturer));

            // "Name" text
            ViewPanel.Controls.Add(CreateLabel("Name:", null, new Point(14, 14), 10, FontStyle.Regular, null, null));

            // Manufacturer name field
            ViewPanel.Controls.Add(CreateTextBox($"{(manufacturer == null ? "" : manufacturer.GetName())}", "Name", new Point(65, 12), 200, false));

            if (Database.manufacturers.Contains(manufacturer))
            {
                // "New ride type" text
                ViewPanel.Controls.Add(CreateButton("New ride type", new Point(14, 50), NewRideType, manufacturer));

                int location = 80;

                foreach (RideType rideType in manufacturer.GetRideTypes())
                {
                    // Panel for RideTypes linked to the Manufacturer
                    Panel panel = new Panel();
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Size = new Size(900, 35);
                    panel.Location = new Point(14, location);
                    ViewPanel.Controls.Add(panel);

                    // RideType name
                    panel.Controls.Add(CreateLabel(rideType.GetName(), null, new Point(9, 9), 10, FontStyle.Bold, null, rideType));

                    // Removes RideType from Manufacturer
                    panel.Controls.Add(CreateButton("Delete", new Point(820, 5), RemoveRideTypeFromManufacturer, rideType));

                    location += 40;
                }
            }
        }
        public void SaveManufacturer(object sender, EventArgs e)
        {
            // Gets the Manufacturer being saved
            Button clickedButton = (Button)sender;
            Manufacturer manufacturer = (Manufacturer)clickedButton.Tag;

            // Updates name
            TextBox textBox = (TextBox)ViewPanel.Controls.Find("Name", true)[0];
            bool valid = textBox.Text.Trim() != "";
            if (valid) manufacturer.SetName(textBox.Text);

            // Adds the Manufacturer if it is being created
            bool newManufacturer = false;
            if (!Database.manufacturers.Contains(manufacturer)) newManufacturer = true;

            if (!Database.manufacturers.Contains(manufacturer) && valid)
            {
                Database.manufacturers.Add(manufacturer);
            }

            if (!valid) MessageBox.Show("Invalid name");

            if (!valid && newManufacturer) LoadManufacturers();
            else ViewManufacturerClicked(sender, e);
        }
        public void DeleteManufacturer(object sender, EventArgs e)
        {
            // Gets the Manufacturer being deleted
            Button button = (Button)sender;
            Manufacturer manufacturer = (Manufacturer)button.Tag;

            // Gets all RideTypes linked to this Manufacturer
            List<RideType> rideTypes = Database.GetRideTypesByManufacturerID(manufacturer.GetID());
            if (rideTypes.Count > 0)
            {
                // Gets all Attractions which uses a RideType of the selected Manufacturer
                List<Attraction> attractions = new List<Attraction>();
                foreach (Attraction attraction in Database.attractions)
                {
                    if (rideTypes.Contains(attraction.GetRideType()))
                    {
                        attractions.Add(attraction);
                    }
                }

                // Asks to confirm delete if there are RideTypes and/or Attractions associated with the Manufacturer
                DialogResult confirmResult = MessageBox.Show($"This will{(rideTypes.Count > 0 ? $"\n- Delete {rideTypes.Count} Ride Type{(rideTypes.Count == 0 ? "" : "s")}" : "")}{(attractions.Count > 0 ? $"\n- Reset the ride type of {attractions.Count} ride{(attractions.Count == 0 ? "" : "s")}" : "")}\n\nAre you sure you want to delete the manufacturer {manufacturer.GetName()}?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    // Removes the Manufacturer, RideTypes, and resets the RideType of relevant attractions
                    Database.manufacturers.Remove(manufacturer);
                    foreach (RideType rideType in rideTypes)
                    {
                        Database.rideTypes.Remove(rideType);
                    }
                    foreach (Attraction attraction in attractions)
                    {
                        attraction.SetRideType(null);
                    }
                }
            }
            else
            {
                // If there are no associated RideTypes, the Manufacturer can be deleted without affecting anything else
                Database.manufacturers.Remove(manufacturer);
            }

            LoadManufacturers();
        }
        public void RemoveRideTypeFromManufacturer(object sender, EventArgs e)
        {
            // Gets the RideType and it's Manufacturer
            Button button = (Button)sender;
            RideType rideType = (RideType)button.Tag;
            Manufacturer manufacturer = rideType.GetManufacturer();

            // Removes references to the RideType, leaving it to be removed and not saved
            manufacturer.RemoveRideType(rideType);
            Database.rideTypes.Remove(rideType);

            // Reloads the Manufacturers edit page
            button.Tag = manufacturer;
            sender = (object)button;
            EditManufacturer(sender, e);
        }

        private void ridesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verifies the user has logged in
            if (Database.profile != null)
            {
                LoadRides("all");
            }
        }
        private void flatRidesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verifies the user has logged in
            if (Database.profile != null)
            {
                LoadRides("3");
            }
        }
        private void rollercoastersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verifies the user has logged in
            if (Database.profile != null)
            {
                LoadRides("1");
            }
        }
        private void darkRidesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verifies the user has logged in
            if (Database.profile != null)
            {
                LoadRides("2");
            }
        }
        public void LoadRides(string type)
        {
            ViewPanel.Controls.Clear();

            List<Attraction> attractions = Database.GetAttractionsByType(type);

            if (Database.attractions.Count != 0)
            {
                int location = 14;

                List<Control> controls = new List<Control>();
                object lockObj = new object();

                // Parallel loops through each Attraction
                Parallel.ForEach(attractions, attraction =>
                {
                    Control attractionLabel, partition = null;
                    int thisLocation;

                    // This locks this object, pausing all other threads that reach this point, allowing the shared location variable to be processed properly
                    lock (lockObj)
                    {
                        thisLocation = location;
                        location += 50;
                    }

                    // Attraction name
                    attractionLabel = CreateLabel($"{attraction.GetName(DateOnly.FromDateTime(DateTime.Now))} - {attraction.GetPark().GetName()}", null, new Point(14, thisLocation), 20, FontStyle.Regular, ViewRideClicked, attraction);

                    if (thisLocation != 14)
                    {
                        // Partition between Attractions
                        partition = CreatePartition(thisLocation - 10);
                    }

                    // Add controls to the list of controls to be added
                    lock (lockObj)
                    {
                        controls.Add(attractionLabel);
                        if (partition != null) controls.Add(partition);
                    }
                });

                // Add all controls
                Invoke(() =>
                {
                    // AddRange allows them to be added in bulk for further optimisation
                    ViewPanel.Controls.AddRange(controls.ToArray());
                });
            }
            else
            {
                if (Database.parks.Count != 0)
                {
                    // No activity message when no rides
                    ViewPanel.Controls.Add(CreateLabel("No Rides to display. Got to a park and click 'New Ride' to get started!", null, new Point(50, 317), 10, FontStyle.Regular, null, null));
                }
                else
                {
                    // No activity message when no park
                    ViewPanel.Controls.Add(CreateLabel("No Rides to display. Click 'New Park' and then add a ride to get started!", null, new Point(50, 317), 10, FontStyle.Regular, null, null));
                }
            }
        }
        public void ViewRideClicked(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            Attraction attraction = null;
            bool edit = false;

            // Gets the Attraction, trying for both places that could call this
            try
            {
                Label clickedLabel = (Label)sender;
                attraction = (Attraction)clickedLabel.Tag;
            }
            catch
            {
                try
                {
                    Button clickedButton = (Button)sender;
                    attraction = (Attraction)clickedButton.Tag;
                }
                catch { }
            }

            // Edit Attraction button
            ViewPanel.Controls.Add(CreateButton("Edit attraction", new Point(1110, 14), EditRide, attraction));

            // Attraction name
            ViewPanel.Controls.Add(CreateLabel(attraction.GetName(DateOnly.FromDateTime(DateTime.Now)), null, new Point(14, 14), 20, FontStyle.Bold, null, null));

            // Theme Park name
            ViewPanel.Controls.Add(CreateLabel(attraction.GetPark().GetName(), null, new Point(14, 44), 15, FontStyle.Regular, ViewThemeParkClicked, attraction.GetPark()));

            int location = 70;

            // Displays opening name if different
            if (attraction.GetName(DateOnly.FromDateTime(DateTime.Now)) != attraction.GetOpeningName())
            {
                ViewPanel.Controls.Add(CreateLabel($"Opened as: {attraction.GetOpeningName()}", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                location += 20;
            }

            // Displays opening date
            ViewPanel.Controls.Add(CreateLabel($"Opened on: {attraction.GetOpeningDate()}", null, new Point(14, location), 10, FontStyle.Regular, null, null));
            location += 40;

            if (attraction.GetRideType() != null)
            {
                // Displays Ride Type
                ViewPanel.Controls.Add(CreateLabel($"Ride Type: {attraction.GetRideType().GetName()} - {attraction.GetRideType().GetManufacturer().GetName()}", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                location += 20;
            }

            // Displays different info depending on the type of Attraction
            List<string> elements = attraction.GetElements();
            switch (elements[0])
            {
                // Rollercoaster
                case "1":
                    // Type
                    ViewPanel.Controls.Add(CreateLabel($"Type: Rollercoaster", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                    location += 20;

                    // Displays Track Length
                    ViewPanel.Controls.Add(CreateLabel($"Track Length: {elements[1]}", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                    location += 20;

                    // Displays Top Speed
                    ViewPanel.Controls.Add(CreateLabel($"Top Speed: {elements[2]}", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                    location += 20;

                    // Displays Inversions
                    ViewPanel.Controls.Add(CreateLabel($"Inversions: {elements[3]}", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                    location += 40;

                    break;

                // Dark Ride
                case "2":
                    // Type
                    ViewPanel.Controls.Add(CreateLabel($"Type: Dark Ride", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                    location += 20;

                    // Displays Track Length
                    ViewPanel.Controls.Add(CreateLabel($"Track Length: {elements[1]}", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                    location += 20;

                    // Displays Type
                    ViewPanel.Controls.Add(CreateLabel($"Type: {elements[2]}", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                    location += 40;

                    break;

                // Flat Ride
                case "3":
                    // Type
                    ViewPanel.Controls.Add(CreateLabel($"Type: Flat Ride", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                    location += 20;

                    // Displays Type
                    ViewPanel.Controls.Add(CreateLabel($"Type: {elements[1]}", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                    location += 40;

                    break;
            }

            // Rename History
            if (attraction.GetRenames().Count > 0)
            {
                // "Rename History" header
                ViewPanel.Controls.Add(CreateLabel($"Name History:", null, new Point(14, location), 15, FontStyle.Bold, null, null));
                location += 30;

                // Original name
                ViewPanel.Controls.Add(CreateLabel($"{attraction.GetOpeningDate()}  -  {attraction.GetOpeningName()}", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                location += 20;

                List<AttractionRename> renames = attraction.GetRenames();
                foreach (AttractionRename rename in renames)
                {
                    // New name
                    ViewPanel.Controls.Add(CreateLabel($"{rename.GetDate()}  -  {rename.GetName()}", null, new Point(14, location), 10, FontStyle.Regular, null, null));
                    location += 20;
                }
                location += 20;
            }

            // Visits

            // Gets a list of all Visits which contain this ride
            List<Visit> visits = new List<Visit>();
            foreach (Visit visit in Database.GetVisitsByParkID(attraction.GetID()))
            {
                if (visit.GetAttractions().Any(visitAttraction => visitAttraction.GetAttraction() == attraction))
                {
                    visits.Add(visit);
                }
            }

            // Checks if any Visits were found
            if (visits.Count > 0)
            {
                // "Rename History" header
                ViewPanel.Controls.Add(CreateLabel($"In Visits:", null, new Point(14, location), 15, FontStyle.Bold, null, null));
                location += 30;

                // Outputs each Visit found
                foreach (Visit visit in visits)
                {
                    // Visit name
                    ViewPanel.Controls.Add(CreateLabel($"{visit.GetPark().GetName()} - {visit.GetDate()}", null, new Point(14, location), 10, FontStyle.Regular, ViewVisitClicked, visit));
                    location += 20;
                }
            }

            if (Database.WasInLastVisit(attraction))
            {
                ViewPanel.Controls.Add(CreateLabel($"You got on this ride in your last visit", null, new Point(14, location + 30), 10, FontStyle.Regular, null, null));
            }
            else
            {
                ViewPanel.Controls.Add(CreateLabel($"You did not get on this ride in your last visit", null, new Point(14, location + 30), 10, FontStyle.Regular, null, null));
            }
        }
        public void NewRide(object sender, EventArgs e)
        {
            // Creates a new Attraction with default values
            Button buttonClicked = (Button)sender;
            Rollercoaster rollercoaster = new Rollercoaster(Database.GetNextAttractionID(), "", DateOnly.FromDateTime(DateTime.Now), (Park)buttonClicked.Tag, null, 0, 0, 0);
            buttonClicked.Tag = rollercoaster;
            sender = (object)buttonClicked;
            EditRide(sender, e);
        }
        public void EditRide(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            Button clickedButton = (Button)sender;
            Attraction attraction = (Attraction)clickedButton.Tag;

            // Save Attraction button
            ViewPanel.Controls.Add(CreateButton("Save attraction", new Point(1100, 14), SaveRide, attraction));

            // Save Attraction button
            ViewPanel.Controls.Add(CreateButton("Delete attraction", new Point(1096, 44), DeleteRide, attraction));

            // "Name" text
            ViewPanel.Controls.Add(CreateLabel("Name:", null, new Point(14, 14), 10, FontStyle.Regular, null, null));

            // Name field
            ViewPanel.Controls.Add(CreateTextBox($"{(attraction == null ? "" : attraction.GetName(DateOnly.FromDateTime(DateTime.Now)))}", "Name", new Point(115, 12), 200, false));

            // "Ride Model" text
            ViewPanel.Controls.Add(CreateLabel("Ride Model:", null, new Point(14, 35), 10, FontStyle.Regular, null, null));

            // Ride Model ComboBox
            ComboBox comboBox = new ComboBox();
            Database.rideTypes.OrderBy(rideType => rideType.GetName());
            Database.rideTypes.OrderBy(rideType => rideType.GetManufacturer().GetName());
            comboBox.Items.Add("None");
            int index = 0;
            foreach (RideType rideType in Database.rideTypes)
            {
                comboBox.Items.Add($"{rideType.GetManufacturer().GetName()} - {rideType.GetName()}");
                if (attraction != null)
                {
                    if (rideType == attraction.GetRideType())
                    {
                        index = comboBox.Items.Count - 1;
                    }
                }
            }
            comboBox.SelectedIndex = index;
            comboBox.Location = new Point(115, 33);
            comboBox.Size = new Size(200, 10);
            comboBox.Name = "RideTypes";
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ViewPanel.Controls.Add(comboBox);

            // "Ride Type" label
            ViewPanel.Controls.Add(CreateLabel("Ride Type:", null, new Point(14, 56), 10, FontStyle.Regular, null, null));

            // Ride Type ComboBox
            comboBox = new ComboBox();
            comboBox.Items.Add("Rollercoaster");
            comboBox.Items.Add("Dark Ride");
            comboBox.Items.Add("Flat Ride");
            comboBox.Location = new Point(115, 54);
            comboBox.Size = new Size(200, 10);
            comboBox.Tag = attraction;
            comboBox.Name = "Types";
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ViewPanel.Controls.Add(comboBox);

            if (attraction != null) comboBox.SelectedIndex = int.Parse(attraction.GetElements()[0]) - 1;
            comboBox.SelectedIndexChanged += TypeChanged;

            // "Opening Date" text
            ViewPanel.Controls.Add(CreateLabel("Opening Date:", null, new Point(14, 77), 10, FontStyle.Regular, null, null));

            // Opening Date
            DateTimePicker timePicker = new DateTimePicker();
            timePicker.Location = new Point(115, 75);
            timePicker.Name = "OpeningDate";
            ViewPanel.Controls.Add(timePicker);

            if (attraction != null) timePicker.Value = attraction.GetOpeningDate().ToDateTime(TimeOnly.FromDateTime(DateTime.Now));

            // Add Rename button
            ViewPanel.Controls.Add(CreateButton("Add Rename", new Point(14, 124), RenameClicked, null));

            // Displays all renames if they exist
            foreach (AttractionRename rename in attraction.GetRenames())
            {
                clickedButton.Tag = rename;
                sender = (object)clickedButton;
                RenameClicked(sender, e);
            }
            OutputTypeDetails(attraction, false);
        }
        public void TypeChanged(object sender, EventArgs e)
        {
            // Updates the class if the Attractions type is changed
            ComboBox comboBox = (ComboBox)sender;
            Attraction attraction = (Attraction)comboBox.Tag;
            int index = comboBox.SelectedIndex;
            switch (index)
            {
                case 0:
                    Rollercoaster rollercoaster = new Rollercoaster(attraction.GetID(), attraction.GetOpeningName(), attraction.GetOpeningDate(), attraction.GetPark(), attraction.GetRideType(), 1, 1, 1);
                    attraction = rollercoaster;
                    break;
                case 1:
                    DarkRide darkRide = new DarkRide(attraction.GetID(), attraction.GetOpeningName(), attraction.GetOpeningDate(), attraction.GetPark(), attraction.GetRideType(), 1, 1);
                    attraction = darkRide;
                    break;
                case 2:
                    FlatRide flatRide = new FlatRide(attraction.GetID(), attraction.GetOpeningName(), attraction.GetOpeningDate(), attraction.GetPark(), attraction.GetRideType(), 1);
                    attraction = flatRide;
                    break;
            }
            OutputTypeDetails(attraction, true);
        }
        public void OutputTypeDetails(Attraction attraction, bool updated)
        {
            // If it's being updated, it needs to remove the type data that is already there
            if (updated)
            {
                // Labels, so will exist for all 3 types
                object[] labels = Controls.Find("TypeBased", true);
                foreach (object labelEdit in labels)
                {
                    Label labelToRemove = (Label)labelEdit;
                    ViewPanel.Controls.Remove(labelToRemove);
                }

                // Input fields
                string[] numerics = { "TrackLength", "TopSpeed", "Inversions" };
                foreach (string numeric in numerics)
                {
                    try
                    {
                        object[] objectFind = Controls.Find(numeric, true);
                        foreach (object objectFound in objectFind)
                        {
                            try
                            {
                                NumericUpDown upDown = (NumericUpDown)objectFound;
                                ViewPanel.Controls.Remove(upDown);
                            }
                            catch { }
                        }
                    }
                    catch { }
                }
                try
                {
                    object[] objectFind = Controls.Find("Type", true);
                    try
                    {
                        foreach (object objectFound in objectFind)
                        {
                            ComboBox comboBoxFound = (ComboBox)objectFound;
                            ViewPanel.Controls.Remove(comboBoxFound);
                        }
                    }
                    catch { }
                }
                catch { }
            }

            NumericUpDown numericUpDown;
            string[] items = null;
            string selectedItem = "";
            ComboBox comboBox;

            // Creates different controls for each type
            switch (attraction.GetElements()[0])
            {
                // Tracked Ride -> Rollercoaster
                case "1":
                    // "Track Length" text
                    ViewPanel.Controls.Add(CreateLabel("Track Length (metres):", "TypeBased", new Point(500, 25), 10, FontStyle.Regular, null, null));

                    // Track Length NumericUpDown
                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 23);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 2500;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[1]);
                    numericUpDown.Name = "TrackLength";
                    ViewPanel.Controls.Add(numericUpDown);

                    // "Top Speed" text
                    ViewPanel.Controls.Add(CreateLabel("Top Speed (mph):", "TypeBased", new Point(500, 46), 10, FontStyle.Regular, null, null));

                    // Top Speed NumericUpDown
                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 44);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 150;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[2]);
                    numericUpDown.Name = "TopSpeed";
                    ViewPanel.Controls.Add(numericUpDown);

                    // "Inversions" text
                    ViewPanel.Controls.Add(CreateLabel("Inversions:", "TypeBased", new Point(500, 67), 10, FontStyle.Regular, null, null));

                    // Inversions NumericUpDown
                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 65);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 14;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[3]);
                    numericUpDown.Name = "Inversions";
                    ViewPanel.Controls.Add(numericUpDown);

                    break;

                // Tracked Ride -> Dark Ride
                case "2":
                    // "Track Length" text
                    ViewPanel.Controls.Add(CreateLabel("Track Length (metres):", "TypeBased", new Point(500, 35), 10, FontStyle.Regular, null, null));

                    // Track Length NumericUpDown
                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 33);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 2500;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[1]);
                    numericUpDown.Name = "TrackLength";
                    ViewPanel.Controls.Add(numericUpDown);

                    // "Type" text
                    ViewPanel.Controls.Add(CreateLabel("Type:", "TypeBased", new Point(500, 56), 10, FontStyle.Regular, null, null));

                    // Type ComboBox
                    comboBox = new ComboBox();
                    comboBox.Location = new Point(700, 54);
                    items = new string[] { "Omnimover", "Trackless", "Boat", "Other" };
                    selectedItem = attraction.GetElements()[2];
                    foreach (string item in items)
                    {
                        comboBox.Items.Add(item);
                        if (item == selectedItem)
                        {
                            comboBox.SelectedIndex = comboBox.Items.Count - 1;
                        }
                    }
                    comboBox.Size = new Size(100, 10);
                    comboBox.Name = "Type";
                    ViewPanel.Controls.Add(comboBox);

                    break;

                // Flat Ride
                case "3":
                    // "Type" text
                    ViewPanel.Controls.Add(CreateLabel("Type:", "TypeBased", new Point(500, 46), 10, FontStyle.Regular, null, null));

                    // Type ComboBox
                    comboBox = new ComboBox();
                    comboBox.Location = new Point(700, 44);
                    items = new string[] { "Pendulum", "Rotation", "Tower", "Other" };
                    selectedItem = attraction.GetElements()[1];
                    foreach (string item in items)
                    {
                        comboBox.Items.Add(item);
                        if (item == selectedItem)
                        {
                            comboBox.SelectedIndex = comboBox.Items.Count - 1;
                        }
                    }
                    comboBox.Size = new Size(100, 10);
                    comboBox.Name = "Type";
                    ViewPanel.Controls.Add(comboBox);

                    break;
            }
        }
        public void SaveRide(object sender, EventArgs e)
        {
            // Gets the Attraction being saved
            Button clickedButton = (Button)sender;
            Attraction attraction = (Attraction)clickedButton.Tag;

            // Updates name
            TextBox textBox = (TextBox)ViewPanel.Controls.Find("Name", true)[0];
            bool valid = textBox.Text.Trim() != "";
            if (valid) attraction.SetName(textBox.Text);

            // Gets the RideType
            ComboBox comboBox = (ComboBox)ViewPanel.Controls.Find("RideTypes", true)[0];
            if (comboBox.SelectedItem != "None")
            {
                attraction.SetRideType(Database.rideTypes[comboBox.SelectedIndex - 1]);
            }
            else
            {
                attraction.SetRideType(null);
            }

            // Adds the RideType if it is being created
            bool newAttraction = false;
            if (!Database.attractions.Contains(attraction)) newAttraction = true;

            if (!Database.attractions.Contains(attraction) && valid)
            {
                attraction.GetPark().AddAttraction(attraction);
                Database.attractions.Add(attraction);
            }

            if (!valid) MessageBox.Show("Invalid name");

            if (valid)
            {
                // Gets the type
                comboBox = (ComboBox)ViewPanel.Controls.Find("Types", true)[0];
                attraction.GetPark().RemoveAttraction(attraction);
                Database.attractions.Remove(attraction);

                // Gets the Attraction based on its type
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        attraction = new Rollercoaster(attraction.GetID(), attraction.GetOpeningName(), attraction.GetOpeningDate(), attraction.GetPark(), attraction.GetRideType(), int.Parse(((NumericUpDown)Controls.Find("TrackLength", true)[0]).Value.ToString()), int.Parse(((NumericUpDown)Controls.Find("TopSpeed", true)[0]).Value.ToString()), int.Parse(((NumericUpDown)Controls.Find("Inversions", true)[0]).Value.ToString()));
                        break;
                    case 1:
                        attraction = new DarkRide(attraction.GetID(), attraction.GetOpeningName(), attraction.GetOpeningDate(), attraction.GetPark(), attraction.GetRideType(), int.Parse(((NumericUpDown)Controls.Find("TrackLength", true)[0]).Value.ToString()), int.Parse((((ComboBox)Controls.Find("Type", true)[0]).SelectedIndex + 1).ToString()));
                        break;
                    case 2:
                        attraction = new FlatRide(attraction.GetID(), attraction.GetOpeningName(), attraction.GetOpeningDate(), attraction.GetPark(), attraction.GetRideType(), int.Parse((((ComboBox)Controls.Find("Type", true)[0]).SelectedIndex + 1).ToString()));
                        break;
                }

                // Adds the Attraction if it's being created
                Database.attractions.Add(attraction);
                attraction.GetPark().AddAttraction(attraction);
            }

            // Gets each rename
            DateTimePicker datePicker;
            Control[] panels = ViewPanel.Controls.Find("Panel", true);
            if (panels.Length > 0)
            {
                List<AttractionRename> renames = new List<AttractionRename>();
                foreach (Control panelControl in panels)
                {
                    try
                    {
                        Panel panel = (Panel)panelControl;
                        textBox = (TextBox)panel.Controls.Find("Name", true)[0];
                        datePicker = (DateTimePicker)panel.Controls.Find("ChangeDate", true)[0];
                        renames.Add(new AttractionRename(DateOnly.FromDateTime(datePicker.Value), textBox.Text));
                    }
                    catch { }
                }
                renames.OrderBy(rename => rename.GetDate());
                attraction.SetRenames(renames);
            }

            // Gets the opening date
            datePicker = (DateTimePicker)ViewPanel.Controls.Find("OpeningDate", true)[0];
            attraction.SetOpeningDate(DateOnly.FromDateTime(datePicker.Value));

            // Goes back to the view of the Attraction
            clickedButton.Tag = attraction;
            sender = (object)clickedButton;

            if (!valid && newAttraction) LoadRides("all");
            else ViewRideClicked(sender, e);
        }
        public void DeleteRide(object sender, EventArgs e)
        {
            // Gets the Attraction being deleted
            Button button = (Button)sender;
            Attraction attraction = (Attraction)button.Tag;

            // Get Visits containing this ride
            Park park = attraction.GetPark();
            Dictionary<VisitAttraction, Visit> visitAttractions = new Dictionary<VisitAttraction, Visit>();

            HashSet<Visit> uniqueVisits = new HashSet<Visit>();
            foreach (Visit visit in Database.GetVisitsByParkID(park.GetID()))
            {
                foreach (VisitAttraction visitAttraction in visit.GetAttractions())
                {
                    if (visitAttraction.GetAttraction() == attraction)
                    {
                        visitAttractions.Add(visitAttraction, visit);
                        uniqueVisits.Add(visit);
                    }
                }
            }

            // Asks to confirm delete if there are Visits associated with the Attraction
            DialogResult confirmResult = MessageBox.Show($"This will\n- Remove this ride from {uniqueVisits.Count} visit{(uniqueVisits.Count == 0 ? "" : "s")}\n\nAre you sure you want to delete the Attraction {attraction.GetName(DateOnly.FromDateTime(DateTime.Now))}?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                // Deletes the Attraction and removes it from any Visits
                Database.attractions.Remove(attraction);
                park.RemoveAttraction(attraction);

                foreach (KeyValuePair<VisitAttraction, Visit> entry in visitAttractions)
                {
                    Visit visit = (Visit)entry.Value;
                    visit.RemoveVisitAttraction((VisitAttraction)entry.Key);
                }

            }

            // Loads al Rides once removed
            LoadRides("all");
        }
        public void RenameClicked(object sender, EventArgs e)
        {
            // Gets rename
            AttractionRename rename = null;
            Button clickedButton = (Button)sender;
            try
            {
                rename = (AttractionRename)clickedButton.Tag;
            }
            catch { }

            int before = Controls.Find("TypeBased", true).Count() * 2;

            // Creates Panel for rename
            Panel panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Size = new Size(900, 35);
            panel.Location = new Point(14, (ViewPanel.Controls.Count - 10 - before) * 40 + 124 + ViewPanel.AutoScrollPosition.Y);
            panel.Name = "Panel";
            ViewPanel.Controls.Add(panel);

            // "Name" text
            panel.Controls.Add(CreateLabel("Name:", null, new Point(9, 9), 10, FontStyle.Bold, null, rename));

            // Name field
            panel.Controls.Add(CreateTextBox($"{(rename == null ? "" : rename.GetName())}", "Name", new Point(59, 5), 200, false));

            // "Change date" text
            panel.Controls.Add(CreateLabel("Change date:", null, new Point(344, 9), 10, FontStyle.Regular, null, null));

            // Change date field
            DateTimePicker timePicker = new DateTimePicker();
            timePicker.Location = new Point(439, 5);
            timePicker.Name = "ChangeDate";
            panel.Controls.Add(timePicker);

            if (rename != null)
            {
                timePicker.Value = rename.GetDate().ToDateTime(TimeOnly.Parse("3:00:00"));
            }

            // Delete rename button
            panel.Controls.Add(CreateButton("Delete", new Point(818, 5), DeleteRename, panel));
        }
        public void DeleteRename(object sender, EventArgs e)
        {
            // Gets Rename being removed
            Button clickedButton = (Button)sender;
            Panel panel = (Panel)clickedButton.Tag;
            int location = panel.Location.Y;
            ViewPanel.Controls.Remove(panel);

            object[] panels = ViewPanel.Controls.Find("Panel", true);

            // Moves all panels up if location below the removed one
            if (panels.Length > 1)
            {
                foreach (Control panelControl in panels)
                {
                    Panel panelCheck = (Panel)panelControl;
                    if (panelCheck.Location.Y > location)
                    {
                        panelCheck.Location = new Point(panelCheck.Location.X, panelCheck.Location.Y - 40);
                    }
                }
            }
        }

        private void rideTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Verifies the user has logged in
            if (Database.profile != null)
            {
                LoadRideTypes();
            }
        }
        public void LoadRideTypes()
        {
            ViewPanel.Controls.Clear();

            List<RideType> rideTypes = Database.rideTypes;

            if (Database.rideTypes.Count > 0)
            {
                int location = 14;

                List<Control> controls = new List<Control>();
                object lockObj = new object();

                // Parallel loops through each RideType
                Parallel.ForEach(rideTypes, rideType =>
                {
                    Control rideName, partition = null;
                    int thisLocation;

                    // This locks this object, pausing all other threads that reach this point, allowing the shared location variable to be processed properly
                    lock (lockObj)
                    {
                        thisLocation = location;
                        location += 50;
                    }

                    // Ride name and manufacturer label
                    rideName = CreateLabel($"{rideType.GetName()} - {rideType.GetManufacturer().GetName()}", null, new Point(14, thisLocation), 20, FontStyle.Regular, ViewRideTypeClicked, rideType);

                    if (thisLocation != 14)
                    {
                        // Partition between RideTypes
                        partition = CreatePartition(thisLocation - 10);
                    }

                    // Add controls to the list of controls to be added
                    lock (lockObj)
                    {
                        controls.Add(rideName);
                        if (partition != null) controls.Add(partition);
                    }
                });

                // Add all controls
                Invoke(() =>
                {
                    // AddRange allows them to be added in bulk for further optimisation
                    ViewPanel.Controls.AddRange(controls.ToArray());
                });
            }
            else
            {
                if (Database.parks.Count != 0)
                {
                    // No activity message
                    ViewPanel.Controls.Add(CreateLabel("No Ride Types to display. Got to a park and click 'New Manufacturer' to get started!", null, new Point(50, 317), 10, FontStyle.Regular, null, null));
                }
                else
                {
                    // No activity message
                    ViewPanel.Controls.Add(CreateLabel("No Ride Types to display.Click 'New Manufacturer' and then add a ride type to get started!", null, new Point(50, 317), 10, FontStyle.Regular, null, null));
                }
            }
        }
        public void ViewRideTypeClicked(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            RideType rideType = null;

            // Gets the RideType, trying for both places that could call this
            try
            {
                Label labelClicked = (Label)sender;
                rideType = (RideType)labelClicked.Tag;
            }
            catch
            {
                try
                {
                    Button clickedButton = (Button)sender;
                    rideType = (RideType)clickedButton.Tag;
                }
                catch { }
            }

            // Edit Ride Type button
            ViewPanel.Controls.Add(CreateButton("Edit Ride Type", new Point(1080, 14), EditRideType, rideType));

            // Ride Type name
            ViewPanel.Controls.Add(CreateLabel(rideType.GetName(), null, new Point(14, 14), 20, FontStyle.Bold, null, null));

            int location = 57;

            // Displays all rides
            List<Attraction> attractions = Database.attractions.Where(attraction => attraction.GetRideType() == rideType).ToList();
            if (attractions.Count > 0)
            {
                // "Attractions" text
                ViewPanel.Controls.Add(CreateLabel("Attractions", null, new Point(14, location), 15, FontStyle.Bold, null, null));
                location += 27;

                foreach (Attraction attraction in attractions)
                {
                    // "Attractions" text
                    ViewPanel.Controls.Add(CreateLabel(attraction.GetName(DateOnly.FromDateTime(DateTime.Now)), null, new Point(14, location), 10, FontStyle.Regular, ViewRideClicked, attraction));
                    location += 20;
                }
            }
        }
        public void NewRideType(object sender, EventArgs e)
        {
            // Creates a new RideType with default values
            Button buttonClicked = (Button)sender;
            RideType rideType = new RideType(Database.GetNextRideTypeID(), "", (Manufacturer)buttonClicked.Tag);
            buttonClicked.Tag = rideType;
            sender = (object)buttonClicked;
            EditRideType(sender, e);
        }
        public void EditRideType(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            // Gets RideType being edited
            Button clickedButton = (Button)sender;
            RideType rideType = (RideType)clickedButton.Tag;

            // Save Ride Type button
            ViewPanel.Controls.Add(CreateButton("Save Ride Type", new Point(1100, 14), SaveRideType, rideType));

            // Save Ride Type button
            ViewPanel.Controls.Add(CreateButton("Delete Ride Type", new Point(1096, 44), DeleteRideType, rideType));

            // "Name" text
            ViewPanel.Controls.Add(CreateLabel("Name:", null, new Point(14, 14), 10, FontStyle.Regular, null, null));

            // "Name" field
            ViewPanel.Controls.Add(CreateTextBox($"{(rideType == null ? "" : rideType.GetName())}", "Name", new Point(100, 12), 200, false));
        }
        public void SaveRideType(object sender, EventArgs e)
        {
            // Gets RideType being saved
            Button clickedButton = (Button)sender;
            RideType rideType = (RideType)clickedButton.Tag;

            // Updates name
            TextBox textBox = (TextBox)ViewPanel.Controls.Find("Name", true)[0];
            bool valid = textBox.Text.Trim() != "";
            if (valid) rideType.SetName(textBox.Text);

            // Adds the RideType if it is being created
            bool newRideType = false;
            if (!Database.rideTypes.Contains(rideType)) newRideType = true;

            if (!Database.rideTypes.Contains(rideType) && valid)
            {
                rideType.GetManufacturer().AddRideType(rideType);
                Database.rideTypes.Add(rideType);
            }

            if (!valid) MessageBox.Show("Invalid name");

            if (!valid && newRideType) LoadRideTypes();
            else ViewRideTypeClicked(sender, e);
        }
        public void DeleteRideType(object sender, EventArgs e)
        {
            // Gets the RideTpye being deleted
            Button button = (Button)sender;
            RideType rideType = (RideType)button.Tag;

            // Gets all Attractions set to this RideType
            List<Attraction> attractions = new List<Attraction>();
            foreach (Attraction attraction in Database.attractions)
            {
                if (attraction.GetRideType() == rideType)
                {
                    attractions.Add(attraction);
                }
            }

            // Asks to confirm delete if there are Attactions associated with the RideType
            DialogResult confirmResult = MessageBox.Show($"This will\n- Reset the ride type of {attractions.Count} ride{(attractions.Count == 0 ? "" : "s")}\n\nAre you sure you want to delete the Ride Type {rideType.GetName()}?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                // Deletes the RideType and removes it from any Attractions
                Database.rideTypes.Remove(rideType);
                rideType.GetManufacturer().RemoveRideType(rideType);

                foreach (Attraction attraction in attractions)
                {
                    attraction.SetRideType(null);
                }
            }

            // Loads al RideTypes once removed
            LoadRideTypes();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Database.SaveData();
        }

        public Label CreateLabel(string text, string name, Point location, int fontSize, FontStyle fontStyle, EventHandler clickEvent, object tag)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = location;
            label.Font = new Font("Arial", fontSize, fontStyle);
            label.AutoSize = true;
            label.Click += clickEvent;
            label.Tag = tag;
            label.Name = name;
            if (clickEvent != null)
            {
                label.Cursor = Cursors.Hand;
                if (tag != null)
                {
                    string type = tag.GetType().Name;
                    if (type == "Tuple`2") type = tag.GetType().ToString().Split('.')[2].Split(',')[0];
                    ToolTip toolTip = new ToolTip();
                    toolTip.SetToolTip(label, $"Go to {type}");
                }
            }
            return label;
        }
        public Button CreateButton(string text, Point location, EventHandler clickEvent, object tag)
        {
            Button button = new Button();
            button.Text = text;
            button.Location = location;
            button.AutoSize = true;
            button.Tag = tag;
            button.Click += clickEvent;
            button.Cursor = Cursors.Hand;
            return button;
        }
        public TextBox CreateTextBox(string text, string name, Point location, int width, Boolean password)
        {
            TextBox textBox = new TextBox();
            textBox.Text = text;
            textBox.Location = location;
            textBox.Width = width;
            textBox.Name = name;
            textBox.UseSystemPasswordChar = password;
            return textBox;
        }
        public Label CreatePartition(int height)
        {
            Label label = new Label();
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Size = new Size(1186, 1);
            label.Location = new Point(14, height);
            return label;
        }
    }
}