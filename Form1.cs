using System;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Theme_Park_Tracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PopulateData();
            DisplayLogin(null, null);
        }

        static void PopulateData()
        {
            StreamReader sr;
            string line;

            // Loads all profile information

            Database.profiles.Clear();
            if (!File.Exists("Profiles.txt"))
            {
                File.WriteAllText("Profiles.txt", "end");
            }
            sr = new StreamReader("Profiles.txt");
            line = sr.ReadLine();
            while (line != "end")
            {
                int id = int.Parse(line);
                string username = sr.ReadLine();
                string email = sr.ReadLine();
                string password = sr.ReadLine();
                Database.profiles.Add(new Profile(id, username, email, password, false));
                line = sr.ReadLine();
            }
            sr.Close();

            // Loads all park information

            Database.parks.Clear();
            if (!File.Exists("Parks.txt"))
            {
                File.WriteAllText("Parks.txt", "end");
            }
            sr = new StreamReader("Parks.txt");
            line = sr.ReadLine();
            while (line != "end")
            {
                int id = int.Parse(line);
                string name = sr.ReadLine();
                Database.parks.Add(new Park(id, name));
                line = sr.ReadLine();
            }
            sr.Close();

            // Loads all the visits information

            Database.visits.Clear();
            if (!File.Exists("Visits.txt"))
            {
                File.WriteAllText("Visits.txt", "end");
            }
            sr = new StreamReader("Visits.txt");
            line = sr.ReadLine();
            while (line != "end")
            {
                int id = int.Parse(line);
                DateOnly date = DateOnly.Parse("January 01, 1900");
                try
                {
                    date = DateOnly.Parse(sr.ReadLine());
                }
                catch (Exception ex) { }
                Profile profile = Database.GetProfileByID(int.Parse(sr.ReadLine()));
                Park park = Database.GetParkByID(int.Parse(sr.ReadLine()));
                Database.visits.Add(new Visit(id, date, profile, park));
                line = sr.ReadLine();
            }
            sr.Close();

            // Loads all the manufacturers information

            Database.manufacturers.Clear();
            if (!File.Exists("Manufacturers.txt"))
            {
                File.WriteAllText("Manufacturers.txt", "end");
            }
            sr = new StreamReader("Manufacturers.txt");
            line = sr.ReadLine();
            while (line != "end")
            {
                int id = int.Parse(line);
                string name = sr.ReadLine();
                Database.manufacturers.Add(new Manufacturer(id, name));
                line = sr.ReadLine();
            }
            sr.Close();

            // Loads all ride type information

            Database.rideTypes.Clear();
            if (!File.Exists("RideTypes.txt"))
            {
                File.WriteAllText("RideTypes.txt", "end");
            }
            sr = new StreamReader("RideTypes.txt");
            line = sr.ReadLine();
            while (line != "end")
            {
                int id = int.Parse(line);
                string name = sr.ReadLine();
                Manufacturer manufacturer = Database.GetManufacturerByID(int.Parse(sr.ReadLine()));
                Database.rideTypes.Add(new RideType(id, name, manufacturer));
                line = sr.ReadLine();
            }
            sr.Close();

            // Loads all attraction information

            Database.attractions.Clear();
            if (!File.Exists("Attractions.txt"))
            {
                File.WriteAllText("Attractions.txt", "end");
            }
            sr = new StreamReader("Attractions.txt");
            line = sr.ReadLine();
            while (line != "end")
            {
                int id = int.Parse(line);
                string openingName = sr.ReadLine();
                DateOnly openingDate = DateOnly.Parse(sr.ReadLine());
                Park park = Database.GetParkByID(int.Parse(sr.ReadLine()));
                string rideTypeString = sr.ReadLine();
                RideType rideType = null;
                if (rideTypeString != "None")
                {
                    rideType = Database.GetRideTypeByID(int.Parse(rideTypeString));
                }

                Attraction attraction = new FlatRide(id, openingName, openingDate, park, rideType, 1);
                string[] attractionLine = sr.ReadLine().Split('-');
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
                line = sr.ReadLine();
            }
            sr.Close();

            // Loads all attraction rename information

            if (!File.Exists("AttractionRenames.txt"))
            {
                File.WriteAllText("AttractionRenames.txt", "end");
            }
            sr = new StreamReader("AttractionRenames.txt");
            line = sr.ReadLine();
            while (line != "end")
            {
                Attraction attraction = Database.GetAttractionByID(int.Parse(line));
                DateOnly date = DateOnly.Parse("January 01, 1900");
                try
                {
                    date = DateOnly.Parse(sr.ReadLine());
                }
                catch (Exception ex) { }
                string newName = sr.ReadLine();
                attraction.AddRename(new AttractionRename(date, newName));
                line = sr.ReadLine();
            }
            sr.Close();

            // Loads all visit attraction information

            if (!File.Exists("VisitAttractions.txt"))
            {
                File.WriteAllText("VisitAttractions.txt", "end");
            }
            sr = new StreamReader("VisitAttractions.txt");
            line = sr.ReadLine();
            while (line != "end")
            {
                Visit visit = Database.GetVisitByID(int.Parse(line));
                int order = int.Parse(sr.ReadLine());
                Attraction attraction = Database.GetAttractionByID(int.Parse(sr.ReadLine()));
                TimeOnly time = TimeOnly.Parse(sr.ReadLine());
                int waitTime = int.Parse(sr.ReadLine());
                visit.AddAttraction(new VisitAttraction(attraction, order, time, waitTime));
                line = sr.ReadLine();
            }
            sr.Close();

            foreach (Profile profile in Database.profiles)
            {
                profile.SetVisits(Database.GetVisitsByProfileID(profile.GetID()));
            }
            foreach (Park park in Database.parks)
            {
                park.SetAttractions(Database.GetAttractionByPark(park.GetID()));
            }
            foreach (Manufacturer manufacturer in Database.manufacturers)
            {
                manufacturer.SetRideTypes(Database.GetRideTypesByManufacturerID(manufacturer.GetID()));
            }
        }

        public void DisplayLogin(object sender, EventArgs e)
        {
            ViewPanel.Controls.Clear();

            Label label = new Label();
            label.Text = "Login";
            label.Location = new Point(90, 130);
            label.Font = new Font("Arial", 40, FontStyle.Bold);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = "Username:";
            label.Location = new Point(100, 230);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = "Password:";
            label.Location = new Point(100, 275);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            TextBox textBox1 = new TextBox();
            textBox1.Location = new Point(210, 230);
            textBox1.Width = 200;
            ViewPanel.Controls.Add(textBox1);

            TextBox textBox2 = new TextBox();
            textBox2.Location = new Point(210, 275);
            textBox2.Width = 200;
            textBox2.UseSystemPasswordChar = true;
            ViewPanel.Controls.Add(textBox2);

            Button button = new Button();
            button.Text = "Login";
            button.Location = new Point(180, 320);
            button.Size = new Size(125, 25);
            button.Click += LogInClicked;
            ViewPanel.Controls.Add(button);

            label = new Label();
            label.Text = "Create an account";
            label.Location = new Point(190, 360);
            label.AutoSize = true;
            label.Click += DisplayCreateAccount;
            ViewPanel.Controls.Add(label);
        }
        public void LogInClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            TextBox textBox = (TextBox)ViewPanel.Controls[3];
            string username = textBox.Text;
            textBox = (TextBox)ViewPanel.Controls[4];
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

            Label label = new Label();
            label.Text = "Sign Up";
            label.Location = new Point(90, 130);
            label.Font = new Font("Arial", 40, FontStyle.Bold);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = "Username:";
            label.Location = new Point(100, 230);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = "Email:";
            label.Location = new Point(100, 275);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = "Password:";
            label.Location = new Point(100, 320);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = "Password:";
            label.Location = new Point(100, 365);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Location = new Point(210, 230);
            textBox.Width = 200;
            ViewPanel.Controls.Add(textBox);

            textBox = new TextBox();
            textBox.Location = new Point(210, 275);
            textBox.Width = 200;
            ViewPanel.Controls.Add(textBox);

            textBox = new TextBox();
            textBox.Location = new Point(210, 320);
            textBox.Width = 200;
            textBox.UseSystemPasswordChar = true;
            ViewPanel.Controls.Add(textBox);

            textBox = new TextBox();
            textBox.Location = new Point(210, 365);
            textBox.Width = 200;
            textBox.UseSystemPasswordChar = true;
            ViewPanel.Controls.Add(textBox);

            Button button = new Button();
            button.Text = "Sign up";
            button.Location = new Point(180, 410);
            button.Size = new Size(125, 25);
            button.Click += SignUpClicked;
            ViewPanel.Controls.Add(button);

            label = new Label();
            label.Text = "Login";
            label.Location = new Point(225, 450);
            label.AutoSize = true;
            label.Click += DisplayLogin;
            ViewPanel.Controls.Add(label);
        }
        public void SignUpClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            TextBox textBox = (TextBox)ViewPanel.Controls[5];
            string username = textBox.Text;
            textBox = (TextBox)ViewPanel.Controls[6];
            string email = textBox.Text;
            textBox = (TextBox)ViewPanel.Controls[7];
            string password1 = textBox.Text;
            textBox = (TextBox)ViewPanel.Controls[8];
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
                            Profile profile = new Profile(Database.GetNextProfileID(), username, email, password1, true);
                            Database.profiles.Add(profile);
                            Database.profile = profile;
                            LoadFeed();
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

            Label label = new Label();
            label.Text = Database.profile.GetUsername();
            label.Location = new Point(12, 10);
            label.Font = new Font("Arial", 30, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            Button button = new Button();
            button.Text = "Delete Profile";
            button.Location = new Point(1110, 610);
            button.Size = new Size(80, 30);
            button.Click += DeleteProfile;
            ViewPanel.Controls.Add(button);

            label = new Label();
            label.Text = "Username:";
            label.Location = new Point(14, 80);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            TextBox textBox1 = new TextBox();
            textBox1.Text = Database.profile.GetUsername();
            textBox1.Location = new Point(130, 82);
            textBox1.Width = 200;
            ViewPanel.Controls.Add(textBox1);

            label = new Label();
            label.Text = "Email:";
            label.Location = new Point(14, 110);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            TextBox textBox2 = new TextBox();
            textBox2.Text = Database.profile.GetEmail();
            textBox2.Location = new Point(130, 112);
            textBox2.Width = 200;
            ViewPanel.Controls.Add(textBox2);

            label = new Label();
            label.Text = "Password:";
            label.Location = new Point(14, 140);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            TextBox textBox3 = new TextBox();
            textBox3.Location = new Point(130, 142);
            textBox3.Width = 200;
            textBox3.UseSystemPasswordChar = true;
            ViewPanel.Controls.Add(textBox3);

            button = new Button();
            button.Text = "Update";
            button.Location = new Point(110, 180);
            button.Size = new Size(80, 30);
            button.Tag = new List<TextBox>() { textBox1, textBox2, textBox3 };
            button.Click += UpdateButtonClicked;
            ViewPanel.Controls.Add(button);

            label = new Label();
            label.Text = "Other Users";
            label.Location = new Point(14, 230);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            int location = 260;
            foreach (Profile profile in Database.profiles)
            {
                if (profile != Database.profile)
                {
                    label = new Label();
                    label.Text = profile.GetUsername();
                    label.Location = new Point(14, location);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    label.Tag = profile;
                    label.Click += ViewProfileClicked;
                    label.Cursor = Cursors.Hand;
                    ViewPanel.Controls.Add(label);

                    ToolTip toolTip = new ToolTip();
                    toolTip.SetToolTip(label, "Open profiles visits");

                    location += 20;
                }
            }
        }
        private void UpdateButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            List<TextBox> textBoxes = (List<TextBox>)button.Tag;

            if (Database.VerifyInfo(textBoxes[0].Text, textBoxes[1].Text, textBoxes[2].Text))
            {
                Database.profile.UpdateInfo(textBoxes[0].Text, textBoxes[1].Text, textBoxes[2].Text);
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

            Label label = new Label();
            label.Text = "Hello, " + Database.profile.GetUsername();
            label.Location = new Point(14, 10);
            label.Font = new Font("Arial", 30, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            IEnumerable<Visit> otherVisits = Database.visits.Where(visit => Database.profile.CheckIfDidVisit(visit) == false);

            if (otherVisits.Count() != 0)
            {
                int location = 90;
                foreach (Visit visit in otherVisits)
                {
                    label = new Label();
                    label.Text = $"{visit.GetProfile().GetUsername()} visited {visit.GetPark().GetName()} on {visit.GetDate().ToString()}";
                    label.Location = new Point(20, location);
                    label.Font = new Font("Arial", 15, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);

                    location += 25;

                    label = new Label();
                    int visitRides = visit.GetAttractionCount();
                    label.Text = $"They got on {visitRides} ride{(visitRides == 1 ? "" : "s")}";
                    label.Location = new Point(20, location);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);

                    location += 40;
                }
            }
            else
            {
                label = new Label();
                label.Text = "No activity from other users!";
                label.Location = new Point(50, 317);
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.AutoSize = true;
                ViewPanel.Controls.Add(label);
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
                Button button = new Button();
                button.Text = "New Visit";
                button.Location = new Point(14, 14);
                button.Size = new Size(100, 30);
                button.Click += NewVisit;
                ViewPanel.Controls.Add(button);

                location = 74;
            }

            if (Database.visits.Count > 0)
            {
                foreach (Visit visit in profile.GetVisits())
                {
                    Label label = new Label();
                    label.Text = $"{visit.GetPark().GetName()} - {visit.GetDate()}";
                    label.Location = new Point(14, location);
                    label.Font = new Font("Arial", 20, FontStyle.Regular);
                    label.AutoSize = true;
                    Tuple<Visit, bool> tuple = Tuple.Create(visit, edit);
                    label.Tag = tuple;
                    label.Click += ViewVisitClicked;
                    label.Cursor = Cursors.Hand;
                    ViewPanel.Controls.Add(label);

                    label = new Label();
                    label.Text = $"-- {visit.GetAttractionCount()} Ride{(visit.GetAttractionCount() == 1 ? "" : "s")} -- {visit.GetUniqueAttractionCount()} Unique Ride{(visit.GetUniqueAttractionCount() == 1 ? "" : "s")} --";
                    label.Location = new Point(1000, location + 8);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);

                    if (location != 14 && location != 74)
                    {
                        label = new Label();
                        label.BorderStyle = BorderStyle.FixedSingle;
                        label.Size = new Size(1186, 1);
                        label.Location = new Point(14, location - 10);
                        ViewPanel.Controls.Add(label);
                    }

                    location += 50;
                }
            }
            else
            {
                if (profile == Database.profile)
                {
                    Label label = new Label();
                    label.Text = "No Visits to display. Click 'New Visit' to get started!";
                    label.Location = new Point(50, 317);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);
                }
                else
                {
                    Label label = new Label();
                    label.Text = "This user doesn't have any visits yet!";
                    label.Location = new Point(50, 317);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);
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

            if (edit)
            {
                Button button = new Button();
                button.Text = "Edit visit";
                button.Location = new Point(1120, 14);
                button.Size = new Size(70, 30);
                button.Tag = visit;
                button.Click += EditVisit;
                ViewPanel.Controls.Add(button);
            }

            Label label = new Label();
            label.Text = visit.GetPark().GetName();
            label.Location = new Point(14, 14);
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = visit.GetDate().ToString();
            label.Location = new Point(14, 43);
            label.Font = new Font("Arial", 15, FontStyle.Bold);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = $"{visit.GetProfile().GetUsername()}'s visit";
            label.Location = new Point(14, 65);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = "Rides";
            label.Location = new Point(14, 103);
            label.Font = new Font("Arial", 15, FontStyle.Bold);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = $"Unique Rides: {visit.GetUniqueAttractionCount()}";
            label.Location = new Point(300, 25);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = $"Total Rides: {visit.GetAttractionCount()}";
            label.Location = new Point(300, 50);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            List<VisitAttraction> attractions = visit.GetAttractions().OrderBy(attraction => attraction.GetOrder()).ToList();

            int location = 130;
            foreach (VisitAttraction attraction in attractions)
            {
                label = new Label();
                label.Text = $"{attraction.GetOrder()}. {attraction.GetAttraction().GetName(visit.GetDate())}";
                label.Location = new Point(14, location);
                label.Font = new Font("Arial", 13, FontStyle.Regular);
                label.Tag = attraction;
                label.Click += ViewRideClicked;
                label.Cursor = Cursors.Hand;
                label.AutoSize = true;
                ViewPanel.Controls.Add(label);

                if (location != 130)
                {
                    label = new Label();
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Size = new Size(1186, 1);
                    label.Location = new Point(14, location - 5);
                    ViewPanel.Controls.Add(label);
                }
                int waitTime = attraction.GetWaitTime();
                int hours = waitTime / 60, minutes = waitTime % 60;
                if (waitTime != -1)
                {
                    label = new Label();
                    label.Text = $"Wait Time: {(hours != 0 ? $"{hours} hour{(hours == 1 ? "" : "s")}" : "")}{(hours != 0 && minutes != 0 ? " and " : "")}{(minutes != 0 ? $"{minutes} minute{(minutes == 1 ? "" : "s")}" : "")}";
                    label.Location = new Point(300, location + 2);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);
                }

                TimeOnly time = attraction.GetTime();
                if (time != TimeOnly.Parse("3:00 am"))
                {
                    label = new Label();
                    label.Text = $"Enter Time: {time}";
                    label.Location = new Point(600, location + 2);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);
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

            Button button = new Button();
            button.Text = "Save visit";
            button.Location = new Point(1120, 14);
            button.Size = new Size(70, 30);
            button.Tag = visit;
            button.Click += SaveVisit;
            ViewPanel.Controls.Add(button);

            button = new Button();
            button.Text = "Delete visit";
            button.Location = new Point(1110, 610);
            button.Size = new Size(80, 30);
            button.Tag = visit;
            button.Click += DeleteVisit;
            ViewPanel.Controls.Add(button);

            Label label = new Label();
            label.Text = "Theme Park:";
            label.Location = new Point(14, 14);
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

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
            ViewPanel.Controls.Add(comboBox);

            label = new Label();
            label.Text = "Date:";
            label.Location = new Point(14, 35);
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            DateTimePicker datePicker = new DateTimePicker();
            datePicker.Location = new Point(105, 33);
            if (visit != null)
            {
                datePicker.Value = visit.GetDate().ToDateTime(TimeOnly.Parse("10:00 PM"));
            }
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

            button = new Button();
            button.Text = "Add Ride";
            button.Location = new Point(14, 70);
            button.Size = new Size(70, 30);
            button.Tag = comboBox;
            button.Click += AttractionClicked;
            ViewPanel.Controls.Add(button);

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

            ComboBox comboBox = (ComboBox)ViewPanel.Controls[3];
            visit.SetPark(Database.GetParkByID(comboBox.SelectedIndex + 1));

            DateTimePicker datePicker = (DateTimePicker)ViewPanel.Controls[5];
            visit.SetDate(DateOnly.FromDateTime(datePicker.Value));

            List<VisitAttraction> attractions = new List<VisitAttraction>();

            for (int index = 8; index < ViewPanel.Controls.Count; index++)
            {
                Panel attractionPanel = (Panel)ViewPanel.Controls[index];
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

                Label label = new Label();
                label.Text = attraction.GetName(DateOnly.FromDateTime(DateTime.Now));
                label.Location = new Point(9, 9);
                label.Font = new Font("Arial", 10, FontStyle.Bold);
                label.AutoSize = true;
                label.Tag = attraction;
                panel.Controls.Add(label);

                label = new Label();
                label.Text = "Wait time:";
                label.Location = new Point(239, 9);
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.AutoSize = true;
                panel.Controls.Add(label);

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

                label = new Label();
                label.Text = "Entry time:";
                label.Location = new Point(469, 9);
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.AutoSize = true;
                panel.Controls.Add(label);

                DateTimePicker timePicker = new DateTimePicker();
                timePicker.Format = DateTimePickerFormat.Time;
                timePicker.Location = new Point(549, 5);
                timePicker.Width = 67;
                timePicker.ShowUpDown = true;
                panel.Controls.Add(timePicker);

                checkBox = new CheckBox();
                checkBox.Location = new Point(626, 6);
                checkBox.Tag = timePicker;
                panel.Controls.Add(checkBox);

                if (visitAttraction != null)
                {
                    if (visitAttraction.GetTime() != TimeOnly.Parse("3:00:00"))
                    {
                        timePicker.Value = DateOnly.FromDateTime(DateTime.Now).ToDateTime(visitAttraction.GetTime());

                        checkBox.Checked = true;
                    }
                }

                Button button = new Button();
                button.Text = "/\\";
                button.Location = new Point(810, 5);
                button.Size = new Size(25, 25);
                button.Tag = panel;
                button.Click += MoveAttractionUp;
                panel.Controls.Add(button);

                button = new Button();
                button.Text = "\\/";
                button.Location = new Point(840, 5);
                button.Size = new Size(25, 25);
                button.Tag = panel;
                button.Click += MoveAttractionDown;
                panel.Controls.Add(button);

                button = new Button();
                button.Text = "D";
                button.Location = new Point(870, 5);
                button.Size = new Size(25, 25);
                button.Tag = panel;
                button.Click += DeleteAttraction;
                panel.Controls.Add(button);
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

            Button button = new Button();
            button.Text = "New Park";
            button.Location = new Point(14, 14);
            button.Size = new Size(100, 30);
            button.Click += NewPark;
            ViewPanel.Controls.Add(button);

            if (Database.parks.Count != 0)
            {
                int location = 74;

                foreach (Park park in Database.parks)
                {
                    Label label = new Label();
                    label.Text = park.GetName();
                    label.Location = new Point(14, location);
                    label.Font = new Font("Arial", 20, FontStyle.Regular);
                    label.AutoSize = true;
                    label.Tag = park;
                    label.Click += ViewThemeParkClicked;
                    label.Cursor = Cursors.Hand;
                    ViewPanel.Controls.Add(label);

                    label = new Label();
                    int parkRides = Database.GetAttractionByPark(park.GetID()).Count;
                    label.Text = $"{parkRides} ride{(parkRides == 1 ? "" : "s")} tracked";
                    label.Location = new Point(1100, location + 8);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);

                    if (location != 74)
                    {
                        label = new Label();
                        label.BorderStyle = BorderStyle.FixedSingle;
                        label.Size = new Size(1186, 1);
                        label.Location = new Point(14, location - 10);
                        ViewPanel.Controls.Add(label);
                    }

                    location += 50;
                }
            }
            else
            {
                Label label = new Label();
                label.Text = "No Theme Parks to display. Click 'New Park' to get started!";
                label.Location = new Point(50, 317);
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.AutoSize = true;
                ViewPanel.Controls.Add(label);
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

            Button button = new Button();
            button.Text = "Edit Park";
            button.Location = new Point(1120, 14);
            button.Size = new Size(70, 30);
            button.Tag = park;
            button.Click += EditPark;
            ViewPanel.Controls.Add(button);

            Label label = new Label();
            label.Text = park.GetName(); ;
            label.Location = new Point(14, 14);
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = $"Total Rides: {(park.GetAttractions() == null ? "0" : park.GetAttractions().Count)}";
            label.Location = new Point(400, 18);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = "Rides";
            label.Location = new Point(14, 65);
            label.Font = new Font("Arial", 15, FontStyle.Bold);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            int location = 92;

            if (park.GetAttractions() != null)
            {
                List<Attraction> attractions = park.GetAttractions().OrderBy(attraction => attraction.GetName(DateOnly.FromDateTime(DateTime.Now))).ToList();

                foreach (Attraction attraction in attractions)
                {
                    label = new Label();
                    label.Text = $"{attraction.GetName(DateOnly.FromDateTime(DateTime.Now))}";
                    label.Location = new Point(14, location);
                    label.Font = new Font("Arial", 13, FontStyle.Regular);
                    label.Tag = attraction;
                    label.Click += ViewRideClicked;
                    label.Cursor = Cursors.Hand;
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);

                    if (location != 92)
                    {
                        label = new Label();
                        label.BorderStyle = BorderStyle.FixedSingle;
                        label.Size = new Size(1186, 1);
                        label.Location = new Point(14, location - 5);
                        ViewPanel.Controls.Add(label);
                    }

                    label = new Label();
                    label.Text = $"{(attraction.GetElements()[0] == "1" ? "Rollercoaster" : $"{(attraction.GetElements()[0] == "3" ? "Flat Ride" : "Dark Ride")}")}";
                    label.Location = new Point(300, location + 2);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);

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

            Button button = new Button();
            button.Text = "Save park";
            button.Location = new Point(1120, 14);
            button.Size = new Size(70, 30);
            button.Tag = park;
            button.Click += SavePark;
            ViewPanel.Controls.Add(button);

            button = new Button();
            button.Text = "Delete park";
            button.Location = new Point(1110, 610);
            button.Size = new Size(80, 30);
            button.Tag = park;
            button.Click += DeletePark;
            ViewPanel.Controls.Add(button);

            Label label = new Label();
            label.Text = "Name:";
            label.Location = new Point(14, 14);
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Location = new Point(65, 12);
            textBox.Width = 200;
            ViewPanel.Controls.Add(textBox);

            if (park != null)
            {
                textBox.Text = park.GetName();
            }

            if (Database.parks.Contains(park))
            {
                button = new Button();
                button.Text = "New attraction";
                button.Location = new Point(14, 50);
                button.AutoSize = true;
                button.Tag = park;
                button.Click += NewRide;
                ViewPanel.Controls.Add(button);

                int location = 80;

                foreach (Attraction attraction in park.GetAttractions())
                {
                    Panel panel = new Panel();
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Size = new Size(900, 35);
                    panel.Location = new Point(14, location);
                    ViewPanel.Controls.Add(panel);

                    label = new Label();
                    label.Text = attraction.GetName(DateOnly.FromDateTime(DateTime.Now));
                    label.Location = new Point(9, 9);
                    label.Font = new Font("Arial", 10, FontStyle.Bold);
                    label.AutoSize = true;
                    label.Tag = attraction;
                    panel.Controls.Add(label);

                    button = new Button();
                    button.Text = "Delete";
                    button.Location = new Point(820, 5);
                    button.Tag = new Tuple<Park, Attraction>(park, attraction);
                    button.Click += RemoveRideFromPark;
                    button.Size = new Size(75, 25);
                    panel.Controls.Add(button);

                    location += 40;
                }
            }
            else
            {
                label = new Label();
                label.Text = "Please save park before adding attractions";
                label.Location = new Point(14, 50);
                label.AutoSize = true;
                ViewPanel.Controls.Add(label);
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

            Button button = new Button();
            button.Text = "New Manufacturer";
            button.Location = new Point(14, 14);
            button.Size = new Size(120, 30);
            button.Click += NewManufacturer;
            ViewPanel.Controls.Add(button);

            if (Database.manufacturers.Count != 0)
            {
                int location = 74;

                foreach (Manufacturer manufacturer in Database.manufacturers)
                {
                    Label label = new Label();
                    label.Text = manufacturer.GetName();
                    label.Location = new Point(14, location);
                    label.Font = new Font("Arial", 20, FontStyle.Regular);
                    label.AutoSize = true;
                    label.Tag = manufacturer;
                    label.Click += ViewManufacturerClicked;
                    label.Cursor = Cursors.Hand;
                    ViewPanel.Controls.Add(label);

                    label = new Label();
                    int manufacturerModels = Database.GetRideTypesByManufacturerID(manufacturer.GetID()).Count;
                    label.Text = $"{manufacturerModels} model{(manufacturerModels == 1 ? "" : "s")} tracked";
                    label.Location = new Point(1085, location + 8);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);

                    if (location != 74)
                    {
                        label = new Label();
                        label.BorderStyle = BorderStyle.FixedSingle;
                        label.Size = new Size(1186, 1);
                        label.Location = new Point(14, location - 10);
                        ViewPanel.Controls.Add(label);
                    }

                    location += 50;
                }
            }
            else
            {
                Label label = new Label();
                label.Text = "No Manufacturers to display. Click 'New Manufacturer' to get started!";
                label.Location = new Point(50, 317);
                label.Font = new Font("Arial", 10, FontStyle.Regular);
                label.AutoSize = true;
                ViewPanel.Controls.Add(label);
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

            Button button = new Button();
            button.Text = "Edit Manufacturer";
            button.Location = new Point(1100, 14);
            button.Size = new Size(110, 30);
            button.Tag = manufacturer;
            button.Click += EditManufacturer;
            ViewPanel.Controls.Add(button);

            Label label = new Label();
            label.Text = manufacturer.GetName(); ;
            label.Location = new Point(14, 14);
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = $"Total Models: {(manufacturer.GetRideTypes() == null ? "0" : manufacturer.GetRideTypes().Count)}";
            label.Location = new Point(400, 18);
            label.Font = new Font("Arial", 15, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            label = new Label();
            label.Text = "Models";
            label.Location = new Point(14, 65);
            label.Font = new Font("Arial", 15, FontStyle.Bold);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            int location = 92;

            if (manufacturer.GetRideTypes() != null)
            {
                List<RideType> rideTypes = manufacturer.GetRideTypes().OrderBy(rideTypes => rideTypes.GetName()).ToList();

                foreach (RideType rideType in rideTypes)
                {
                    label = new Label();
                    label.Text = $"{rideType.GetName()}";
                    label.Location = new Point(14, location);
                    label.Font = new Font("Arial", 13, FontStyle.Regular);
                    label.Tag = rideType;
                    label.Click += ViewRideTypeClicked;
                    label.Cursor = Cursors.Hand;
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);

                    if (location != 92)
                    {
                        label = new Label();
                        label.BorderStyle = BorderStyle.FixedSingle;
                        label.Size = new Size(1186, 1);
                        label.Location = new Point(14, location - 5);
                        ViewPanel.Controls.Add(label);
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

            Button button = new Button();
            button.Text = "Save manufacturer";
            button.Location = new Point(1070, 14);
            button.Size = new Size(120, 30);
            button.Tag = manufacturer;
            button.Click += SaveManufacturer;
            ViewPanel.Controls.Add(button);

            button = new Button();
            button.Text = "Delete manufacturer";
            button.Location = new Point(1060, 610);
            button.Size = new Size(130, 30);
            button.Tag = manufacturer;
            button.Click += DeleteManufacturer;
            ViewPanel.Controls.Add(button);

            Label label = new Label();
            label.Text = "Name:";
            label.Location = new Point(14, 14);
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Location = new Point(65, 12);
            textBox.Width = 200;
            ViewPanel.Controls.Add(textBox);

            if (manufacturer != null)
            {
                textBox.Text = manufacturer.GetName();
            }

            if (Database.manufacturers.Contains(manufacturer))
            {
                button = new Button();
                button.Text = "New ride type";
                button.Location = new Point(14, 50);
                button.AutoSize = true;
                button.Tag = manufacturer;
                button.Click += NewRideType;
                ViewPanel.Controls.Add(button);

                int location = 80;

                foreach (RideType rideType in manufacturer.GetRideTypes())
                {
                    Panel panel = new Panel();
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Size = new Size(900, 35);
                    panel.Location = new Point(14, location);
                    ViewPanel.Controls.Add(panel);

                    label = new Label();
                    label.Text = rideType.GetName();
                    label.Location = new Point(9, 9);
                    label.Font = new Font("Arial", 10, FontStyle.Bold);
                    label.AutoSize = true;
                    label.Tag = rideType;
                    panel.Controls.Add(label);

                    button = new Button();
                    button.Text = "Delete";
                    button.Location = new Point(820, 5);
                    button.Tag = new Tuple<Manufacturer, RideType>(manufacturer, rideType);
                    button.Click += RemoveRideTypeFromManufacturer;
                    button.Size = new Size(75, 25);
                    panel.Controls.Add(button);

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
                    Label label = new Label();
                    label.Text = $"{attraction.GetName(DateOnly.FromDateTime(DateTime.Now))} - {attraction.GetPark().GetName()}";
                    label.Location = new Point(14, location);
                    label.Font = new Font("Arial", 20, FontStyle.Regular);
                    label.AutoSize = true;
                    label.Tag = attraction;
                    label.Click += ViewRideClicked;
                    label.Cursor = Cursors.Hand;
                    ViewPanel.Controls.Add(label);

                    if (location != 14)
                    {
                        label = new Label();
                        label.BorderStyle = BorderStyle.FixedSingle;
                        label.Size = new Size(1186, 1);
                        label.Location = new Point(14, location - 10);
                        ViewPanel.Controls.Add(label);
                    }

                    location += 50;
                }
            }
            else
            {
                if (Database.parks.Count != 0)
                {
                    Label label = new Label();
                    label.Text = "No Rides to display. Got to a park and click 'New Ride' to get started!";
                    label.Location = new Point(50, 317);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);
                }
                else
                {
                    Label label = new Label();
                    label.Text = "No Rides to display. Click 'New Park' and then add a ride to get started!";
                    label.Location = new Point(50, 317);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);
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

            Button button = new Button();
            button.Text = "Edit attraction";
            button.Location = new Point(1110, 14);
            button.Size = new Size(100, 30);
            button.Tag = attraction;
            button.Click += EditRide;
            ViewPanel.Controls.Add(button);

            Label label = new Label();
            label.Text = attraction.GetName(DateOnly.FromDateTime(DateTime.Now));
            label.Location = new Point(14, 14);
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);
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

            Button button = new Button();
            button.Text = "Save ride";
            button.Location = new Point(1100, 14);
            button.Size = new Size(90, 30);
            button.Tag = attraction;
            button.Click += SaveRide;
            ViewPanel.Controls.Add(button);

            button = new Button();
            button.Text = "Delete ride";
            button.Location = new Point(1110, 610);
            button.Size = new Size(80, 30);
            button.Tag = attraction;
            button.Click += DeleteRide;
            ViewPanel.Controls.Add(button);

            Label label = new Label();
            label.Text = "Name:";
            label.Location = new Point(14, 14);
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Location = new Point(115, 12);
            textBox.Width = 200;
            ViewPanel.Controls.Add(textBox);

            if (attraction != null)
            {
                textBox.Text = attraction.GetName(DateOnly.FromDateTime(DateTime.Now));
            }

            label = new Label();
            label.Text = "Ride Model:";
            label.Location = new Point(14, 35);
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

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

            label = new Label();
            label.Text = "Ride Type:";
            label.Location = new Point(14, 56);
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            comboBox = new ComboBox();
            comboBox.Items.Add("Rollercoaster");
            comboBox.Items.Add("Flat Ride");
            comboBox.Items.Add("Dark Ride");
            comboBox.Location = new Point(115, 54);
            comboBox.Size = new Size(200, 10);
            comboBox.Tag = attraction;
            ViewPanel.Controls.Add(comboBox);

            if (attraction != null)
            {
                string type = attraction.GetElements()[0];
                if (type == "1")
                {
                    comboBox.SelectedIndex = 0;
                }
                else if (type == "3")
                {
                    comboBox.SelectedIndex = 1;
                }
                else if (type == "2")
                {
                    comboBox.SelectedIndex = 2;
                }
            }
            comboBox.SelectedIndexChanged += TypeChanged;

            label = new Label();
            label.Text = "Opening Date:";
            label.Location = new Point(14, 77);
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            DateTimePicker timePicker = new DateTimePicker();
            timePicker.Location = new Point(115, 75);
            ViewPanel.Controls.Add(timePicker);

            button = new Button();
            button.Text = "Add Rename";
            button.Location = new Point(14, 124);
            button.Size = new Size(90, 30); ;
            button.Click += RenameClicked;
            ViewPanel.Controls.Add(button);

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
            Label label;
            ComboBox comboBox;
            switch (attraction.GetElements()[0])
            {
                case "1":
                    label = new Label();
                    label.Text = "Track Length (metres):";
                    label.Location = new Point(500, 25);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    label.Name = "TypeBased";
                    ViewPanel.Controls.Add(label);

                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 23);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 2500;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[1]);
                    numericUpDown.Name = "TrackLength";
                    ViewPanel.Controls.Add(numericUpDown);

                    label = new Label();
                    label.Text = "Top Speed (mph):";
                    label.Location = new Point(500, 46);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    label.Name = "TypeBased";
                    ViewPanel.Controls.Add(label);

                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 44);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 150;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[2]);
                    numericUpDown.Name = "TopSpeed";
                    ViewPanel.Controls.Add(numericUpDown);

                    label = new Label();
                    label.Text = "Inversions:";
                    label.Location = new Point(500, 67);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    label.Name = "TypeBased";
                    ViewPanel.Controls.Add(label);

                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 65);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 14;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[3]);
                    numericUpDown.Name = "Inversions";
                    ViewPanel.Controls.Add(numericUpDown);

                    break;
                case "2":
                    label = new Label();
                    label.Text = "Track Length (metres):";
                    label.Location = new Point(500, 35);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    label.Name = "TypeBased";
                    ViewPanel.Controls.Add(label);

                    numericUpDown = new NumericUpDown();
                    numericUpDown.Location = new Point(700, 33);
                    numericUpDown.Width = 100;
                    numericUpDown.Maximum = 2500;
                    numericUpDown.Value = int.Parse(attraction.GetElements()[1]);
                    numericUpDown.Name = "TrackLength";
                    ViewPanel.Controls.Add(numericUpDown);

                    label = new Label();
                    label.Text = "Type:";
                    label.Location = new Point(500, 56);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    label.Name = "TypeBased";
                    ViewPanel.Controls.Add(label);

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
                    label = new Label();
                    label.Text = "Type:";
                    label.Location = new Point(500, 46);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    label.Name = "TypeBased";
                    ViewPanel.Controls.Add(label);

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

            Label label = new Label();
            label.Text = "Name:";
            label.Location = new Point(9, 9);
            label.Font = new Font("Arial", 10, FontStyle.Bold);
            label.Tag = rename;
            label.AutoSize = true;
            panel.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Location = new Point(59, 5);
            textBox.Width = 200;
            panel.Controls.Add(textBox);

            if (rename != null)
            {
                textBox.Text = rename.GetName();
            }

            label = new Label();
            label.Text = "Change date:";
            label.Location = new Point(344, 9);
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.AutoSize = true;
            panel.Controls.Add(label);

            DateTimePicker timePicker = new DateTimePicker();
            timePicker.Location = new Point(439, 5);
            panel.Controls.Add(timePicker);

            if (rename != null)
            {
                timePicker.Value = rename.GetDate().ToDateTime(TimeOnly.Parse("3:00:00"));
            }

            Button button = new Button();
            button.Text = "D";
            button.Location = new Point(870, 5);
            button.Size = new Size(25, 25);
            button.Tag = panel;
            button.Click += DeleteRename;
            panel.Controls.Add(button);
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
            LoadRideTypes();
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
                    Label label = new Label();
                    label.Text = $"{rideType.GetName()} - {rideType.GetManufacturer().GetName()}";
                    label.Location = new Point(14, location);
                    label.Font = new Font("Arial", 20, FontStyle.Regular);
                    label.AutoSize = true;
                    label.Tag = rideType;
                    label.Click += ViewRideTypeClicked;
                    label.Cursor = Cursors.Hand;
                    ViewPanel.Controls.Add(label);

                    if (location != 14)
                    {
                        label = new Label();
                        label.BorderStyle = BorderStyle.FixedSingle;
                        label.Size = new Size(1186, 1);
                        label.Location = new Point(14, location - 10);
                        ViewPanel.Controls.Add(label);
                    }

                    location += 50;
                }
            }
            else
            {
                if (Database.parks.Count != 0)
                {
                    Label label = new Label();
                    label.Text = "No Ride Types to display. Got to a park and click 'New Manufacturer' to get started!";
                    label.Location = new Point(50, 317);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);
                }
                else
                {
                    Label label = new Label();
                    label.Text = "No Ride Types to display. Click 'New Manufacturer' and then add a ride type to get started!";
                    label.Location = new Point(50, 317);
                    label.Font = new Font("Arial", 10, FontStyle.Regular);
                    label.AutoSize = true;
                    ViewPanel.Controls.Add(label);
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

            Button button = new Button();
            button.Text = "Edit Ride Type";
            button.Location = new Point(1080, 14);
            button.Size = new Size(110, 30);
            button.Tag = rideType;
            button.Click += EditRideType;
            ViewPanel.Controls.Add(button);

            Label label = new Label();
            label.Text = rideType.GetName(); ;
            label.Location = new Point(14, 14);
            label.Font = new Font("Arial", 20, FontStyle.Bold);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);
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

            Button button = new Button();
            button.Text = "Save ride type";
            button.Location = new Point(1100, 14);
            button.Size = new Size(90, 30);
            button.Tag = rideType;
            button.Click += SaveRideType;
            ViewPanel.Controls.Add(button);

            button = new Button();
            button.Text = "Delete ride type";
            button.Location = new Point(1110, 610);
            button.Size = new Size(80, 30);
            button.Tag = rideType;
            button.Click += DeleteRideType;
            ViewPanel.Controls.Add(button);

            Label label = new Label();
            label.Text = "Name:";
            label.Location = new Point(14, 14);
            label.Font = new Font("Arial", 10, FontStyle.Regular);
            label.AutoSize = true;
            ViewPanel.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Location = new Point(100, 12);
            textBox.Width = 200;
            ViewPanel.Controls.Add(textBox);

            if (rideType != null)
            {
                textBox.Text = rideType.GetName();
            }
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
    }
}