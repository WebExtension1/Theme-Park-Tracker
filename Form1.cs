using System;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Theme_Park_Tracker
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            // cd OneDrive - Sheffield Hallam University\Y2\Systems Programming\Theme Park Tracker\bin\Debug\net6.0-windows
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
                    catch (Exception ex) { }
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
                        catch(Exception ex) { }
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
            List<Task> tasks1 = new List<Task>();
            List<Task> tasks2 = new List<Task>();
            List<Task> tasks3 = new List<Task>();
            List<string> fileCreations = new List<string>();

            tasks1.Add(Task.Run(() =>
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

            tasks1.Add(Task.Run(() =>
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

            tasks1.Add(Task.Run(() =>
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

            await Task.WhenAll(tasks1);

            tasks2.Add(Task.Run(() =>
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
                        catch (Exception ex) { }
                        Profile profile = Database.GetProfileByID(br.ReadInt32());
                        Park park = Database.GetParkByID(br.ReadInt32());
                        Database.visits.Add(new Visit(id, date, profile, park));
                    }

                    br.Close();
                    fs.Close();
                }
            }));

            tasks2.Add(Task.Run(() =>
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

            tasks2.Add(Task.Run(() =>
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

            await Task.WhenAll(tasks2);

            tasks3.Add(Task.Run(() =>
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
                        catch (Exception ex) { }
                        string newName = br.ReadString();
                        attraction.AddRename(new AttractionRename(date, newName));
                    }

                    br.Close();
                    fs.Close();
                }
            }));

            tasks3.Add(Task.Run(() =>
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

            await Task.WhenAll(tasks3);
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

            Profile checkProfile = Database.GetProfileByUsername(username);
            if (checkProfile == null)
            {
                MessageBox.Show("User not found");
            }
            else
            {
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

            if (username.Trim() == "" || email.Trim() == "" || password1.Trim() == "" || password2.Trim() == "")
            {
                MessageBox.Show("Missing field");
            }
            else
            {
                Profile checkProfile = Database.GetProfileByUsername(username);
                if (checkProfile != null)
                {
                    MessageBox.Show("Username already exists");
                }
                else
                {
                    checkProfile = Database.GetProfileByEmail(email);
                    if (checkProfile != null)
                    {
                        MessageBox.Show("Email already exists");
                    }
                    else
                    {
                        if (password1 != password2)
                        {
                            MessageBox.Show("Passwords do not match");
                        }
                        else
                        {
                            if (Database.VerifyInfo(username, email, password1))
                            {
                                Profile profile = new Profile(Database.GetNextProfileID(), username, email, password1, true);
                                Database.profiles.Add(profile);
                                Database.profile = profile;
                                LoadFeed();
                            }
                        }
                    }
                }
            }
        }

        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Database.profile != null)
            {
                LoadProfile();
            }
        }
        public void LoadProfile()
        {
            ViewPanel.Controls.Clear();

            List<TextBox> textBoxes = new List<TextBox>();

            // Displays username at top of page
            ViewPanel.Controls.Add(CreateLabel(Database.profile.GetUsername(), null, new Point(12, 10), 30, FontStyle.Regular, null, null));

            // Button to delete Profile
            ViewPanel.Controls.Add(CreateButton("Delete Profile", new Point(1110, 610), DeleteProfile, null));

            // "Username" text
            ViewPanel.Controls.Add(CreateLabel("Username:", null, new Point(14, 80), 15, FontStyle.Regular, null, null));

            // Username field
            textBoxes.Add(CreateTextBox(Database.profile.GetUsername(), "Username", new Point(130, 82), 200, false));
            ViewPanel.Controls.Add(textBoxes[0]);

            // "Email" text
            ViewPanel.Controls.Add(CreateLabel("Email:", null, new Point(14, 110), 15, FontStyle.Regular, null, null));

            // Email field
            textBoxes.Add(CreateTextBox(Database.profile.GetEmail(), "Email", new Point(130, 112), 200, false));
            ViewPanel.Controls.Add(textBoxes[1]);

            // "Password" text
            ViewPanel.Controls.Add(CreateLabel("Password:", null, new Point(14, 140), 15, FontStyle.Regular, null, null));

            // Password field
            textBoxes.Add(CreateTextBox(null, "Password", new Point(130, 142), 200, true));
            ViewPanel.Controls.Add(textBoxes[2]);

            // Update info button
            ViewPanel.Controls.Add(CreateButton("Update", new Point(110, 180), UpdateButtonClicked, null));

            // "Other Users" label for list of other users
            ViewPanel.Controls.Add(CreateLabel("Other Users", null, new Point(14, 230), 15, FontStyle.Regular, null, null));

            int location = 260;
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
        private void UpdateButtonClicked(object sender, EventArgs e)
        {
            string username = ((TextBox)ViewPanel.Controls.Find("Username", true)[0]).Text;
            string email = ((TextBox)ViewPanel.Controls.Find("Email", true)[0]).Text;
            string password = ((TextBox)ViewPanel.Controls.Find("Password", true)[0]).Text;
            if (Database.VerifyInfo(username, email, password))
            {
                Database.profile.UpdateInfo(username, email, password);
            }
        }
        private void ViewProfileClicked(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Profile profile = (Profile)label.Tag;
            LoadVisits(profile, false);
        }
        private void DeleteProfile(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            DialogResult confirmResult = MessageBox.Show($"This will permentantly delete this Profile, delete all Visits attached to this Profile, and Save the data\n\nAre you sure you want to delete this Profile?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Profile profile = Database.profile;
                foreach (Visit visit in profile.GetVisits())
                {
                    Database.visits.Remove(visit);
                }
                Database.profiles.Remove(profile);
                Database.profile = null;
                Database.SaveData();
                DisplayLogin(null, null);
            }
        }

        private void feedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Database.profile != null)
            {
                LoadFeed();
            }
        }
        public void LoadFeed()
        {
            ViewPanel.Controls.Clear();

            // Display hello message
            ViewPanel.Controls.Add(CreateLabel($"Hello, {Database.profile.GetUsername()}", null, new Point(14, 10), 30, FontStyle.Regular, null, null));

            IEnumerable<Visit> otherVisits = Database.visits.Where(visit => Database.profile.CheckIfDidVisit(visit) == false);

            if (otherVisits.Count() != 0)
            {
                int location = 90;
                foreach (Visit visit in otherVisits)
                {
                    // Visitor and park
                    ViewPanel.Controls.Add(CreateLabel($"{visit.GetProfile().GetUsername()} visited {visit.GetPark().GetName()} on {visit.GetDate()}", null, new Point(20, location), 15, FontStyle.Regular, null, null));

                    // Visit activity
                    ViewPanel.Controls.Add(CreateLabel($"They got on {visit.GetAttractionCount()} ride{(visit.GetAttractionCount() == 1 ? "" : "s")}", null, new Point(20, location + 25), 10, FontStyle.Regular, null, null));

                    location += 65;
                }
            }
            else
            {
                // No activity message
                ViewPanel.Controls.Add(CreateLabel("No activity from other users!", null, new Point(50, 317), 10, FontStyle.Regular, null, null));
            }
        }

        private void visitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Database.profile != null)
            {
                LoadVisits(Database.profile, true);
            }
        }
        public void LoadVisits(Profile profile, bool edit)
        {
            ViewPanel.Controls.Clear();

            int location = 14;

            if (edit)
            {
                // New Visit button
                ViewPanel.Controls.Add(CreateButton("New Visit", new Point(14, 14), NewVisit, null));

                location = 74;
            }

            if (Database.visits.Count > 0)
            {
                foreach (Visit visit in profile.GetVisits())
                {
                    // Visit info with view route
                    Tuple<Visit, bool> tuple = Tuple.Create(visit, edit);
                    ViewPanel.Controls.Add(CreateLabel($"{visit.GetPark().GetName()} - {visit.GetDate()}", null, new Point(14, location), 20, FontStyle.Regular, ViewVisitClicked, tuple));

                    // Visit summary
                    ViewPanel.Controls.Add(CreateLabel($"-- {visit.GetAttractionCount()} Ride{(visit.GetAttractionCount() == 1 ? "" : "s")} -- {visit.GetUniqueAttractionCount()} Unique Ride{(visit.GetUniqueAttractionCount() == 1 ? "" : "s")} --", null, new Point(1000, location + 8), 10, FontStyle.Regular, null, null));

                    if (location != 14 && location != 74)
                    {
                        // Partition between Visits
                        ViewPanel.Controls.Add(CreatePartition(location - 10));
                    }

                    location += 50;
                }
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

            try
            {
                Label clickedLabel = (Label)sender;
                Tuple<Visit, bool> tuple = (Tuple<Visit, bool>)clickedLabel.Tag;
                visit = (Visit)tuple.Item1;
                edit = (bool)tuple.Item2;
            }
            catch
            {
                try
                {
                    Button clickedButton = (Button)sender;
                    Tuple<Visit, bool> tuple = (Tuple<Visit, bool>)clickedButton.Tag;
                    visit = (Visit)tuple.Item1;
                    edit = (bool)tuple.Item2;
                }
                catch { }
            }

            Label label;

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

            // "Rides" text
            ViewPanel.Controls.Add(CreateLabel("Rides", null, new Point(14, 103), 15, FontStyle.Bold, null, null));

            // Unique Rides count
            ViewPanel.Controls.Add(CreateLabel($"Unique Rides: {visit.GetUniqueAttractionCount()}", null, new Point(300, 25), 15, FontStyle.Regular, null, null));

            // Total Rides count
            ViewPanel.Controls.Add(CreateLabel($"Total Rides: {visit.GetAttractionCount()}", null, new Point(300, 50), 15, FontStyle.Regular, null, null));

            int location = 130;
            foreach (VisitAttraction attraction in visit.GetAttractions().OrderBy(attraction => attraction.GetOrder()).ToList())
            {
                // Ride order and name
                ViewPanel.Controls.Add(CreateLabel($"{attraction.GetOrder()}. {attraction.GetAttraction().GetName(visit.GetDate())}", null, new Point(14, location), 13, FontStyle.Regular, ViewRideClicked, attraction.GetAttraction()));

                if (location != 130)
                {
                    // Partition between Rides
                    ViewPanel.Controls.Add(CreatePartition(location - 10));
                }
                int waitTime = attraction.GetWaitTime();
                int hours = waitTime / 60, minutes = waitTime % 60;
                if (waitTime != -1)
                {
                    // Displays wait time
                    ViewPanel.Controls.Add(CreateLabel($"Wait Time: {(hours != 0 ? $"{hours} hour{(hours == 1 ? "" : "s")}" : "")}{(hours != 0 && minutes != 0 ? " and " : "")}{(((minutes != -1 && hours == 0) || minutes != 0) ? $"{minutes} minute{(minutes == 1 ? "" : "s")}" : "")}", null, new Point(300, location + 2), 10, FontStyle.Regular, null, null));
                }

                TimeOnly time = attraction.GetTime();
                if (time != TimeOnly.Parse("3:00 am"))
                {
                    // Displays enter time
                    ViewPanel.Controls.Add(CreateLabel($"Enter Time: {time}", null, new Point(600, location + 2), 10, FontStyle.Regular, null, null));
                }

                location += 30;
            }
        }
        private void NewVisit(object sender, EventArgs e)
        {
            if (Database.attractions.Count > 0)
            {
                Visit visit = new Visit(Database.GetNextVisitID(), DateOnly.FromDateTime(DateTime.Now), Database.profile, Database.parks[0]);
                Button buttonClicked = (Button)sender;
                buttonClicked.Tag = visit;
                sender = (object)buttonClicked;
                EditVisit(sender, e);
            }
            else
            {
                MessageBox.Show("Please add attractions to a theme park before logging a visit");
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
            ViewPanel.Controls.Add(CreateButton("Delete Visit", new Point(1120, 610), DeleteVisit, visit));

            // "Theme Park" text
            ViewPanel.Controls.Add(CreateLabel("Theme Park:", null, new Point(14, 14), 10, FontStyle.Regular, null, null));

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
            ViewPanel.Controls.Add(comboBox);

            // "Date" text
            ViewPanel.Controls.Add(CreateLabel("Date:", null, new Point(14, 35), 10, FontStyle.Regular, null, null));

            DateTimePicker datePicker = new DateTimePicker();
            datePicker.Location = new Point(105, 33);
            if (visit != null)
            {
                datePicker.Value = visit.GetDate().ToDateTime(TimeOnly.Parse("10:00 PM"));
            }
            datePicker.Name = "Date";
            ViewPanel.Controls.Add(datePicker);

            int selectedPark = comboBox.SelectedIndex;
            comboBox = new ComboBox();
            List<Attraction> attractions = Database.GetAttractionByPark(Database.GetParkByID(selectedPark + 1).GetID()).OrderBy(attraction => attraction.GetID()).ToList();
            foreach (Attraction attraction in attractions)
            {
                comboBox.Items.Add(attraction.GetName(DateOnly.FromDateTime(DateTime.Now)));
            }
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
                comboBox.Location = new Point(105, 73);
                comboBox.Width = 200;
                comboBox.Name = "combo1";
                comboBox.Tag = Database.GetParkByID(selectedPark + 1);
                ViewPanel.Controls.Add(comboBox);
            }

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
            Button clickedButton = (Button)sender;
            Visit visit = (Visit)clickedButton.Tag;
            Tuple<Visit, bool> tuple = new Tuple<Visit, bool>(visit, true);
            clickedButton.Tag = tuple;
            sender = (object)clickedButton;

            ComboBox comboBox = (ComboBox)ViewPanel.Controls.Find("Park", true)[0];
            visit.SetPark(Database.GetParkByID(comboBox.SelectedIndex + 1));

            DateTimePicker datePicker = (DateTimePicker)ViewPanel.Controls.Find("Date", true)[0];
            visit.SetDate(DateOnly.FromDateTime(datePicker.Value));

            List<VisitAttraction> attractions = new List<VisitAttraction>();

            foreach (Panel attractionPanel in ViewPanel.Controls.Find("Panel", true))
            {
                Attraction attraction = (Attraction)attractionPanel.Controls[0].Tag;
                int order = ((attractionPanel.Location.Y - 112) / 40) + 1;
                int waitTime = -1;
                TimeOnly time = TimeOnly.Parse("3:00:00");

                CheckBox checkBox = (CheckBox)attractionPanel.Controls[3];

                if (checkBox.Checked)
                {
                    NumericUpDown numericUpDown = (NumericUpDown)attractionPanel.Controls[2];
                    waitTime = Convert.ToInt32(Math.Round(numericUpDown.Value, 0));
                }

                checkBox = (CheckBox)attractionPanel.Controls[6];

                if (checkBox.Checked)
                {
                    DateTimePicker dateTimePicker = (DateTimePicker)attractionPanel.Controls[5];
                    time = TimeOnly.Parse($"{dateTimePicker.Value.Hour}:{dateTimePicker.Value.Minute}:{dateTimePicker.Value.Second}");
                }

                attractions.Add(new VisitAttraction(attraction, order, time, waitTime));
            }
            visit.SetAttractions(attractions);

            if (!Database.visits.Contains(visit))
            {
                Database.visits.Add(visit);
                Database.profile.AddVisit(visit);
            }

            ViewVisitClicked(sender, e);
        }
        public void DeleteVisit(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Visit visit = (Visit)button.Tag;

            DialogResult confirmResult = MessageBox.Show($"This will permentantly delete this Visit\n\nAre you sure you want to delete this Visit?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Database.visits.Remove(visit);
                Database.profile.RemoveVisit(visit);
            }

            LoadVisits(Database.profile, true);
        }
        public void AttractionClicked(object sender, EventArgs e)
        {
            Attraction attraction = null;
            VisitAttraction visitAttraction = null;
            Button clickedButton = (Button)sender;
            bool con = true;
            try
            {
                ComboBox comboBox = (ComboBox)clickedButton.Tag;
                Park park = (Park)comboBox.Tag;
                List<Attraction> attractions = Database.GetAttractionByPark(park.GetID()).OrderBy(attraction => attraction.GetID()).ToList();
                attraction = attractions[comboBox.SelectedIndex];
            }
            catch (Exception ex1)
            {
                try
                {
                    visitAttraction = (VisitAttraction)clickedButton.Tag;
                    attraction = visitAttraction.GetAttraction();
                }
                catch (Exception ex2)
                {
                    con = false;
                }
            }

            if (con)
            {
                Panel panel = new Panel();
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.Size = new Size(900, 35);
                panel.Location = new Point(14, (ViewPanel.Controls.Count - 8) * 40 + 112 + ViewPanel.AutoScrollPosition.Y);
                panel.Name = "Panel";
                ViewPanel.Controls.Add(panel);

                // Displays attractions name
                panel.Controls.Add(CreateLabel(attraction.GetName(DateOnly.FromDateTime(DateTime.Now)), null, new Point(9, 9), 10, FontStyle.Bold, null, attraction));

                // "Wait time" text
                panel.Controls.Add(CreateLabel("Wait time:", null, new Point(239, 9), 10, FontStyle.Regular, null, null));

                NumericUpDown numericUpDown = new NumericUpDown();
                numericUpDown.Location = new Point(309, 5);
                numericUpDown.Width = 40;
                numericUpDown.Maximum = 999;
                panel.Controls.Add(numericUpDown);

                CheckBox checkBox = new CheckBox();
                checkBox.Location = new Point(359, 6);
                checkBox.Tag = numericUpDown;
                panel.Controls.Add(checkBox);

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

                DateTimePicker timePicker = new DateTimePicker();
                timePicker.Format = DateTimePickerFormat.Time;
                timePicker.Location = new Point(549, 5);
                timePicker.Width = 67;
                timePicker.ShowUpDown = true;
                panel.Controls.Add(timePicker);

                checkBox = new CheckBox();
                checkBox.Location = new Point(626, 10);
                checkBox.Tag = timePicker;
                checkBox.AutoSize = true;
                panel.Controls.Add(checkBox);

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
            Button clickedButton = (Button)sender;
            Panel panel = (Panel)clickedButton.Tag;
            int location = panel.Location.Y;
            location -= 40;

            if (ViewPanel.Controls.Count > 8)
            {
                for (int i = 8; i < ViewPanel.Controls.Count; i++)
                {
                    Panel panelCheck = (Panel)ViewPanel.Controls[i];
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
            Button clickedButton = (Button)sender;
            Panel panel = (Panel)clickedButton.Tag;
            int location = panel.Location.Y;
            location += 40;

            if (ViewPanel.Controls.Count > 8)
            {
                for (int i = 8; i < ViewPanel.Controls.Count; i++)
                {
                    Panel panelCheck = (Panel)ViewPanel.Controls[i];
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
            Button clickedButton = (Button)sender;
            Panel panel = (Panel)clickedButton.Tag;
            int location = panel.Location.Y;
            ViewPanel.Controls.Remove(panel);

            if (ViewPanel.Controls.Count >= 9)
            {
                for (int i = 8; i < ViewPanel.Controls.Count; i++)
                {
                    Panel panelCheck = (Panel)ViewPanel.Controls[i];
                    if (panelCheck.Location.Y > location)
                    {
                        panelCheck.Location = new Point(panelCheck.Location.X, panelCheck.Location.Y - 40);
                    }
                }
            }
        }
        public void ThemeParkChangedOnVisit(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Park selectedPark = Database.GetParkByID(comboBox.SelectedIndex + 1);
            List<Attraction> attractions = Database.GetAttractionByPark(selectedPark.GetID()).OrderBy(attraction => attraction.GetID()).ToList();

            comboBox = (ComboBox)ViewPanel.Controls[6];
            Park originalPark = (Park)comboBox.Tag;

            if (selectedPark != originalPark)
            {
                comboBox.Items.Clear();
                comboBox.Tag = selectedPark;
                if (attractions.Count > 0)
                {
                    foreach (Attraction attraction in attractions)
                    {
                        comboBox.Items.Add(attraction.GetName(DateOnly.FromDateTime(DateTime.Now)));
                    }
                    comboBox.SelectedIndex = 0;
                }
                else
                {
                    comboBox.Items.Add("");
                    comboBox.SelectedIndex = 0;
                }

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

                foreach (Park park in Database.parks)
                {
                    // Parks name
                    ViewPanel.Controls.Add(CreateLabel(park.GetName(), null, new Point(14, location), 20, FontStyle.Regular, ViewThemeParkClicked, park));

                    // Rides in park count
                    int parkRides = Database.GetAttractionByPark(park.GetID()).Count;
                    ViewPanel.Controls.Add(CreateLabel($"{parkRides} ride{(parkRides == 1 ? "" : "s")} tracked", null, new Point(1100, location + 8), 10, FontStyle.Regular, null, null));

                    if (location != 74)
                    {
                        // Partition between Parks
                        ViewPanel.Controls.Add(CreatePartition(location - 10));
                    }

                    location += 50;
                }
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

            // Total ride count
            ViewPanel.Controls.Add(CreateLabel($"Total Rides: {(park.GetAttractions() == null ? "0" : park.GetAttractions().Count)}", null, new Point(400, 18), 15, FontStyle.Regular, null, null));

            // "Rides" text
            ViewPanel.Controls.Add(CreateLabel("Rides", null, new Point(14, 65), 15, FontStyle.Bold, null, null));

            int location = 92;

            if (park.GetAttractions() != null)
            {
                List<Attraction> attractions = park.GetAttractions().OrderBy(attraction => attraction.GetName(DateOnly.FromDateTime(DateTime.Now))).ToList();

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
        public void NewPark(object sender, EventArgs e)
        {
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
            ViewPanel.Controls.Add(CreateButton("Delete park", new Point(1120, 610), DeletePark, park));

            // "Name" text
            ViewPanel.Controls.Add(CreateLabel("Name:", null, new Point(14, 14), 10, FontStyle.Regular, null, null));

            // Park name field
            ViewPanel.Controls.Add(CreateTextBox($"{(park == null ? "" : park.GetName())}", null, new Point(65, 12), 200, false));

            if (Database.parks.Contains(park))
            {
                // New Attraction Button
                ViewPanel.Controls.Add(CreateButton("New attraction", new Point(14, 50), NewRide, park));

                int location = 80;

                foreach (Attraction attraction in park.GetAttractions())
                {
                    Panel panel = new Panel();
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Size = new Size(900, 35);
                    panel.Location = new Point(14, location);
                    ViewPanel.Controls.Add(panel);

                    // Display Attraction name
                    panel.Controls.Add(CreateLabel(attraction.GetName(DateOnly.FromDateTime(DateTime.Now)), null, new Point(9, 9), 10, FontStyle.Bold, null, attraction));

                    // Delete Attraction button
                    panel.Controls.Add(CreateButton("Delete", new Point(820, 5), RemoveRideFromPark, new Tuple<Park, Attraction>(park, attraction)));

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
            Button clickedButton = (Button)sender;
            Park park = (Park)clickedButton.Tag;
            TextBox textBox = (TextBox)ViewPanel.Controls[3];
            park.SetName(textBox.Text);
            if (!Database.parks.Contains(park))
            {
                Database.parks.Add(park);
            }

            ViewThemeParkClicked(sender, e);
        }
        public void DeletePark(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Park park = (Park)button.Tag;

            List<Attraction> attractions = Database.GetAttractionByPark(park.GetID());
            List<Visit> visits = Database.GetVisitsByParkID(park.GetID());
            if (attractions.Count > 0 || visits.Count > 0)
            {
                DialogResult confirmResult = MessageBox.Show($"This will\n- Delete {attractions.Count} Attraction{(attractions.Count > 0 ? "s" : "")}\n- Delete {visits.Count} Visit{(visits.Count > 0 ? "s" : "")}\n\nAre you sure you want to delete the Park {park.GetName()}?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    Database.parks.Remove(park);
                    foreach (Attraction attraction in attractions)
                    {
                        Database.attractions.Remove(attraction);
                    }
                    foreach (Visit visit in visits)
                    {
                        Database.visits.Remove(visit);
                    }
                }
            }
            else
            {
                Database.parks.Remove(park);
            }

            LoadThemeParks();
        }
        public void RemoveRideFromPark(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Tuple<Park, Attraction> tuple = (Tuple<Park, Attraction>)button.Tag;

            Park park = tuple.Item1;
            Attraction attraction = tuple.Item2;
            park.RemoveAttraction(attraction);
            Database.attractions.Remove(attraction);

            button.Tag = park;
            sender = (object)button;
            EditPark(sender, e);
        }

        private void manufacturersToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

                foreach (Manufacturer manufacturer in Database.manufacturers)
                {
                    // Displays Manufacturer name
                    ViewPanel.Controls.Add(CreateLabel(manufacturer.GetName(), null, new Point(14, location), 20, FontStyle.Regular, ViewManufacturerClicked, manufacturer));

                    // Manufacturers RideType count
                    int manufacturerModels = Database.GetRideTypesByManufacturerID(manufacturer.GetID()).Count;
                    ViewPanel.Controls.Add(CreateLabel($"{manufacturerModels} model{(manufacturerModels == 1 ? "" : "s")} tracked", null, new Point(1085, location + 8), 10, FontStyle.Regular, null, null));

                    if (location != 74)
                    {
                        // Partition between Manufacturers
                        ViewPanel.Controls.Add(CreatePartition(location - 10));
                    }

                    location += 50;
                }
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
            ViewPanel.Controls.Add(CreateButton("Edit Manufacturer", new Point(1100, 14), EditManufacturer, manufacturer));

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
            ViewPanel.Controls.Add(CreateButton("Delete manufacturer", new Point(1070, 610), DeleteManufacturer, manufacturer));

            // "Name" text
            ViewPanel.Controls.Add(CreateLabel("Name:", null, new Point(14, 14), 10, FontStyle.Regular, null, null));

            // Manufacturer name field
            ViewPanel.Controls.Add(CreateTextBox($"{(manufacturer == null ? "" : manufacturer.GetName())}", null, new Point(65, 12), 200, false));

            if (Database.manufacturers.Contains(manufacturer))
            {
                // "New ride type" text
                ViewPanel.Controls.Add(CreateButton("New ride type", new Point(14, 50), NewRideType, manufacturer));

                int location = 80;

                foreach (RideType rideType in manufacturer.GetRideTypes())
                {
                    Panel panel = new Panel();
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Size = new Size(900, 35);
                    panel.Location = new Point(14, location);
                    ViewPanel.Controls.Add(panel);

                    // RideType name
                    panel.Controls.Add(CreateLabel(rideType.GetName(), null, new Point(9, 9), 10, FontStyle.Bold, null, rideType));

                    // Removes RideType from Manufacturer
                    panel.Controls.Add(CreateButton("Delete", new Point(820, 5), RemoveRideTypeFromManufacturer, new Tuple<Manufacturer, RideType>(manufacturer, rideType)));

                    location += 40;
                }
            }
        }
        public void SaveManufacturer(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Manufacturer manufacturer = (Manufacturer)clickedButton.Tag;
            TextBox textBox = (TextBox)ViewPanel.Controls[3];
            manufacturer.SetName(textBox.Text);

            if (!Database.manufacturers.Contains(manufacturer))
            {
                Database.manufacturers.Add(manufacturer);
            }

            ViewManufacturerClicked(sender, e);
        }
        public void DeleteManufacturer(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Manufacturer manufacturer = (Manufacturer)button.Tag;

            List<RideType> rideTypes = Database.GetRideTypesByManufacturerID(manufacturer.GetID());
            if (rideTypes.Count > 0)
            {
                List<Attraction> attractions = new List<Attraction>();
                foreach (Attraction attraction in Database.attractions)
                {
                    if (rideTypes.Contains(attraction.GetRideType()))
                    {
                        attractions.Add(attraction);
                    }
                }

                if (attractions.Count > 0)
                {
                    DialogResult confirmResult = MessageBox.Show($"This will{(rideTypes.Count > 0 ? $"\n- Delete {rideTypes.Count} Ride Type{(rideTypes.Count == 0 ? "" : "s")}" : "")}{(attractions.Count > 0 ? $"\n- Reset the ride type of {attractions.Count} ride{(attractions.Count == 0 ? "" : "s")}" : "")}\n\nAre you sure you want to delete the manufacturer {manufacturer.GetName()}?", "Confirm Delete", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
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
                    Database.manufacturers.Remove(manufacturer);
                }
            }

            LoadManufacturers();
        }
        public void RemoveRideTypeFromManufacturer(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Tuple<Manufacturer, RideType> tuple = (Tuple<Manufacturer, RideType>)button.Tag;

            Manufacturer manufacturer = tuple.Item1;
            RideType rideType = tuple.Item2;
            manufacturer.RemoveRideType(rideType);
            Database.rideTypes.Remove(rideType);

            button.Tag = manufacturer;
            sender = (object)button;
            EditManufacturer(sender, e);
        }

        private void ridesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Database.profile != null)
            {
                LoadRides("all");
            }
        }
        private void flatRidesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Database.profile != null)
            {
                LoadRides("3");
            }
        }
        private void rollercoastersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Database.profile != null)
            {
                LoadRides("1");
            }
        }
        private void darkRidesToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

                foreach (Attraction attraction in attractions)
                {
                    // Attraction name
                    ViewPanel.Controls.Add(CreateLabel($"{attraction.GetName(DateOnly.FromDateTime(DateTime.Now))} - {attraction.GetPark().GetName()}", null, new Point(14, location), 20, FontStyle.Regular, ViewRideClicked, attraction));

                    if (location != 14)
                    {
                        // Partition between Attractions
                        ViewPanel.Controls.Add(CreatePartition(location - 10));
                    }

                    location += 50;
                }
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
        }
        public void NewRide(object sender, EventArgs e)
        {
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
            ViewPanel.Controls.Add(CreateButton("Save ride", new Point(1100, 14), SaveRide, attraction));

            // Save Attraction button
            ViewPanel.Controls.Add(CreateButton("Delete ride", new Point(1100, 610), DeleteRide, attraction));

            // "Name" text
            ViewPanel.Controls.Add(CreateLabel("Name:", null, new Point(14, 14), 10, FontStyle.Regular, null, null));

            // Name field
            ViewPanel.Controls.Add(CreateTextBox($"{(attraction == null ? "" : attraction.GetName(DateOnly.FromDateTime(DateTime.Now)))}", null, new Point(115, 12), 200, false));

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
            ViewPanel.Controls.Add(comboBox);

            // "Ride Type" label
            ViewPanel.Controls.Add(CreateLabel("Ride Type:", null, new Point(14, 56), 10, FontStyle.Regular, null, null));

            // Ride Type ComboBox
            comboBox = new ComboBox();
            comboBox.Items.Add("Rollercoaster");
            comboBox.Items.Add("Flat Ride");
            comboBox.Items.Add("Dark Ride");
            comboBox.Location = new Point(115, 54);
            comboBox.Size = new Size(200, 10);
            comboBox.Tag = attraction;
            ViewPanel.Controls.Add(comboBox);

            if (attraction != null) comboBox.SelectedIndex = int.Parse(attraction.GetElements()[0]) - 1;
            comboBox.SelectedIndexChanged += TypeChanged;

            // "Opening Date" text
            ViewPanel.Controls.Add(CreateLabel("Opening Date:", null, new Point(14, 77), 10, FontStyle.Regular, null, null));

            // Opening Date
            DateTimePicker timePicker = new DateTimePicker();
            timePicker.Location = new Point(115, 75);
            ViewPanel.Controls.Add(timePicker);

            // Add Rename button
            ViewPanel.Controls.Add(CreateButton("Add Rename", new Point(14, 124), RenameClicked, null));

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
                    FlatRide flatRide = new FlatRide(attraction.GetID(), attraction.GetOpeningName(), attraction.GetOpeningDate(), attraction.GetPark(), attraction.GetRideType(), 1);
                    attraction = flatRide;
                    break;
                case 2:
                    DarkRide darkRide = new DarkRide(attraction.GetID(), attraction.GetOpeningName(), attraction.GetOpeningDate(), attraction.GetPark(), attraction.GetRideType(), 1, 1);
                    attraction = darkRide;
                    break;
            }
            OutputTypeDetails(attraction, true);
        }
        public void OutputTypeDetails(Attraction attraction, bool updated)
        {
            if (updated)
            {
                object[] labels = Controls.Find("TypeBased", true);
                foreach (object labelEdit in labels)
                {
                    Label labelToRemove = (Label)labelEdit;
                    ViewPanel.Controls.Remove(labelToRemove);
                }

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
            switch (attraction.GetElements()[0])
            {
                case "1":
                    // "Track Length" text
                    ViewPanel.Controls.Add(CreateLabel("Track Length (metres):", "TypeBased", new Point(500, 25), 10, FontStyle.Regular, null, null));

                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 23);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 2500;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[1]);
                    numericUpDown.Name = "TrackLength";
                    ViewPanel.Controls.Add(numericUpDown);

                    // "Top Speed" text
                    ViewPanel.Controls.Add(CreateLabel("Top Speed (mph):", "TypeBased", new Point(500, 46), 10, FontStyle.Regular, null, null));

                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 44);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 150;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[2]);
                    numericUpDown.Name = "TopSpeed";
                    ViewPanel.Controls.Add(numericUpDown);

                    // "Inversions" text
                    ViewPanel.Controls.Add(CreateLabel("Top Speed (mph):", "TypeBased", new Point(500, 67), 10, FontStyle.Regular, null, null));

                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 65);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 14;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[3]);
                    numericUpDown.Name = "Inversions";
                    ViewPanel.Controls.Add(numericUpDown);

                    break;
                case "2":
                    // "Track Length" text
                    ViewPanel.Controls.Add(CreateLabel("Track Length (metres):", "TypeBased", new Point(500, 25), 10, FontStyle.Regular, null, null));

                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 33);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 2500;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[1]);
                    numericUpDown.Name = "TrackLength";
                    ViewPanel.Controls.Add(numericUpDown);

                    // "Type" text
                    ViewPanel.Controls.Add(CreateLabel("Type:", "TypeBased", new Point(500, 25), 10, FontStyle.Regular, null, null));

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
                case "3":
                    // "Type" text
                    ViewPanel.Controls.Add(CreateLabel("Type:", "TypeBased", new Point(500, 25), 10, FontStyle.Regular, null, null));

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
            Button clickedButton = (Button)sender;
            Attraction attraction = (Attraction)clickedButton.Tag;
            TextBox textBox = (TextBox)ViewPanel.Controls[3];
            attraction.SetName(textBox.Text);

            ComboBox comboBox = (ComboBox)ViewPanel.Controls[5];
            if (comboBox.SelectedItem != "None")
            {
                attraction.SetRideType(Database.rideTypes[comboBox.SelectedIndex - 1]);
            }
            else
            {
                attraction.SetRideType(null);
            }

            comboBox = (ComboBox)ViewPanel.Controls[7];
            attraction.GetPark().RemoveAttraction(attraction);
            Database.attractions.Remove(attraction);

            switch (comboBox.SelectedIndex)
            {
                case 0:
                    attraction = new Rollercoaster(attraction.GetID(), attraction.GetOpeningName(), attraction.GetOpeningDate(), attraction.GetPark(), attraction.GetRideType(), int.Parse(((NumericUpDown)Controls.Find("TrackLength", true)[0]).Value.ToString()), int.Parse(((NumericUpDown)Controls.Find("TopSpeed", true)[0]).Value.ToString()), int.Parse(((NumericUpDown)Controls.Find("Inversions", true)[0]).Value.ToString()));
                    break;
                case 2:
                    attraction = new DarkRide(attraction.GetID(), attraction.GetOpeningName(), attraction.GetOpeningDate(), attraction.GetPark(), attraction.GetRideType(), int.Parse(((NumericUpDown)Controls.Find("TrackLength", true)[0]).Value.ToString()), int.Parse((((ComboBox)Controls.Find("Type", true)[0]).SelectedIndex + 1).ToString()));
                    break;
                case 1:
                    attraction = new FlatRide(attraction.GetID(), attraction.GetOpeningName(), attraction.GetOpeningDate(), attraction.GetPark(), attraction.GetRideType(), int.Parse((((ComboBox)Controls.Find("Type", true)[0]).SelectedIndex + 1).ToString()));
                    break;
            }
            Database.attractions.Add(attraction);
            attraction.GetPark().AddAttraction(attraction);

            DateTimePicker datePicker;

            if (ViewPanel.Controls.Count > 11)
            {
                List<AttractionRename> renames = new List<AttractionRename>();
                for (int loop = 11; loop < ViewPanel.Controls.Count; loop++)
                {
                    try
                    {
                        Panel panel = (Panel)ViewPanel.Controls[loop];
                        textBox = (TextBox)panel.Controls[1];
                        datePicker = (DateTimePicker)panel.Controls[3];
                        renames.Add(new AttractionRename(DateOnly.FromDateTime(datePicker.Value), textBox.Text));
                    }
                    catch (Exception ex) { }
                }
                attraction.SetRenames(renames);
            }

            datePicker = (DateTimePicker)ViewPanel.Controls[9];
            attraction.SetOpeningDate(DateOnly.FromDateTime(datePicker.Value));

            clickedButton.Tag = attraction;
            sender = (object)clickedButton;

            ViewRideClicked(sender, e);
        }
        public void DeleteRide(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Attraction attraction = (Attraction)button.Tag;

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

            DialogResult confirmResult = MessageBox.Show($"This will\n- Remove this ride from {uniqueVisits.Count} visit{(uniqueVisits.Count == 0 ? "" : "s")}\n\nAre you sure you want to delete the Attraction {attraction.GetName(DateOnly.FromDateTime(DateTime.Now))}?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Database.attractions.Remove(attraction);
                park.RemoveAttraction(attraction);
                foreach (KeyValuePair<VisitAttraction, Visit> entry in visitAttractions)
                {
                    Visit visit = (Visit)entry.Value;
                    visit.RemoveVisitAttraction((VisitAttraction)entry.Key);
                }
            }

            LoadRides("all");
        }
        public void RenameClicked(object sender, EventArgs e)
        {
            AttractionRename rename = null;
            Button clickedButton = (Button)sender;
            try
            {
                rename = (AttractionRename)clickedButton.Tag;
            }
            catch (Exception ex) { }

            int before = Controls.Find("TypeBased", true).Count() * 2;

            Panel panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Size = new Size(900, 35);
            panel.Location = new Point(14, (ViewPanel.Controls.Count - 10 - before) * 40 + 124 + ViewPanel.AutoScrollPosition.Y);
            ViewPanel.Controls.Add(panel);

            // "Name" text
            panel.Controls.Add(CreateLabel("Name:", null, new Point(9, 9), 10, FontStyle.Bold, null, rename));

            // Rename field
            panel.Controls.Add(CreateTextBox($"{(rename == null ? "" : rename.GetName())}", null, new Point(59, 5), 200, false));

            // "Change date" text
            panel.Controls.Add(CreateLabel("Change date:", null, new Point(344, 9), 10, FontStyle.Regular, null, null));

            DateTimePicker timePicker = new DateTimePicker();
            timePicker.Location = new Point(439, 5);
            panel.Controls.Add(timePicker);

            if (rename != null)
            {
                timePicker.Value = rename.GetDate().ToDateTime(TimeOnly.Parse("3:00:00"));
            }

            // Delete rename button
            panel.Controls.Add(CreateButton("D", new Point(870, 5), DeleteRename, panel));
        }
        public void DeleteRename(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Panel panel = (Panel)clickedButton.Tag;
            int location = panel.Location.Y;
            ViewPanel.Controls.Remove(panel);

            if (ViewPanel.Controls.Count >= 12)
            {
                for (int i = 11; i < ViewPanel.Controls.Count; i++)
                {
                    try
                    {
                        Panel panelCheck = (Panel)ViewPanel.Controls[i];
                        if (panelCheck.Location.Y > location)
                        {
                            panelCheck.Location = new Point(panelCheck.Location.X, panelCheck.Location.Y - 40);
                        }
                    }
                    catch(Exception ex) { }
                }
            }
        }

        private void rideTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
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

                foreach (RideType rideType in rideTypes)
                {
                    // Ride name and manufacturer label
                    ViewPanel.Controls.Add(CreateLabel($"{rideType.GetName()} - {rideType.GetManufacturer().GetName()}", null, new Point(14, location), 20, FontStyle.Regular, ViewRideTypeClicked, rideType));

                    if (location != 14)
                    {
                        // Partition between RideTypes
                        ViewPanel.Controls.Add(CreatePartition(location - 10));
                    }

                    location += 50;
                }
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

            // Ride type name
            ViewPanel.Controls.Add(CreateLabel(rideType.GetName(), null, new Point(14, 14), 20, FontStyle.Bold, null, null));
        }
        public void NewRideType(object sender, EventArgs e)
        {
            Button buttonClicked = (Button)sender;
            RideType rideType = new RideType(Database.GetNextRideTypeID(), "", (Manufacturer)buttonClicked.Tag);
            buttonClicked.Tag = rideType;
            sender = (object)buttonClicked;
            EditRideType(sender, e);
        }
        public void EditRideType(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            Button clickedButton = (Button)sender;
            RideType rideType = (RideType)clickedButton.Tag;

            // Save Ride Type button
            ViewPanel.Controls.Add(CreateButton("Save ride type", new Point(1100, 14), SaveRideType, rideType));

            // Save Ride Type button
            ViewPanel.Controls.Add(CreateButton("Delete ride type", new Point(1100, 610), DeleteRideType, rideType));

            // "Name" text
            ViewPanel.Controls.Add(CreateLabel("Name:", null, new Point(14, 14), 10, FontStyle.Regular, null, null));

            // "Name" field
            ViewPanel.Controls.Add(CreateTextBox($"{(rideType == null ? "" : rideType.GetName())}", null, new Point(100, 12), 200, false));
        }
        public void SaveRideType(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            RideType rideType = (RideType)clickedButton.Tag;
            TextBox textBox = (TextBox)ViewPanel.Controls[3];
            rideType.SetName(textBox.Text);

            if (!Database.rideTypes.Contains(rideType))
            {
                Database.rideTypes.Add(rideType);
                rideType.GetManufacturer().AddRideType(rideType);
            }

            ViewRideTypeClicked(sender, e);
        }
        public void DeleteRideType(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            RideType rideType = (RideType)button.Tag;

            List<Attraction> attractions = new List<Attraction>();
            foreach (Attraction attraction in Database.attractions)
            {
                if (attraction.GetRideType() == rideType)
                {
                    attractions.Add(attraction);
                }
            }

            DialogResult confirmResult = MessageBox.Show($"This will\n- Reset the ride type of {attractions.Count} ride{(attractions.Count == 0 ? "" : "s")}\n\nAre you sure you want to delete the Ride Type {rideType.GetName()}?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Database.rideTypes.Remove(rideType);
                rideType.GetManufacturer().RemoveRideType(rideType);

                foreach (Attraction attraction in attractions)
                {
                    attraction.SetRideType(null);
                }
            }

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