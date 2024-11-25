namespace Theme_Park_Tracker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            feedToolStripMenuItem = new ToolStripMenuItem();
            themeParksToolStripMenuItem = new ToolStripMenuItem();
            ridesToolStripMenuItem = new ToolStripMenuItem();
            flatRidesToolStripMenuItem = new ToolStripMenuItem();
            rollercoastersToolStripMenuItem = new ToolStripMenuItem();
            darkRidesToolStripMenuItem = new ToolStripMenuItem();
            manufacturersToolStripMenuItem = new ToolStripMenuItem();
            rideTypesToolStripMenuItem = new ToolStripMenuItem();
            visitsToolStripMenuItem = new ToolStripMenuItem();
            profileToolStripMenuItem = new ToolStripMenuItem();
            ViewPanel = new Panel();
            saveButton = new Button();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.ActiveBorder;
            menuStrip1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            menuStrip1.Items.AddRange(new ToolStripItem[] { feedToolStripMenuItem, themeParksToolStripMenuItem, ridesToolStripMenuItem, manufacturersToolStripMenuItem, rideTypesToolStripMenuItem, visitsToolStripMenuItem, profileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1234, 29);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // feedToolStripMenuItem
            // 
            feedToolStripMenuItem.Name = "feedToolStripMenuItem";
            feedToolStripMenuItem.Size = new Size(55, 25);
            feedToolStripMenuItem.Text = "Feed";
            feedToolStripMenuItem.Click += feedToolStripMenuItem_Click;
            // 
            // themeParksToolStripMenuItem
            // 
            themeParksToolStripMenuItem.Name = "themeParksToolStripMenuItem";
            themeParksToolStripMenuItem.Size = new Size(110, 25);
            themeParksToolStripMenuItem.Text = "Theme Parks";
            themeParksToolStripMenuItem.Click += themeParksToolStripMenuItem_Click;
            // 
            // ridesToolStripMenuItem
            // 
            ridesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { flatRidesToolStripMenuItem, rollercoastersToolStripMenuItem, darkRidesToolStripMenuItem });
            ridesToolStripMenuItem.Name = "ridesToolStripMenuItem";
            ridesToolStripMenuItem.Size = new Size(60, 25);
            ridesToolStripMenuItem.Text = "Rides";
            ridesToolStripMenuItem.Click += ridesToolStripMenuItem_Click;
            // 
            // flatRidesToolStripMenuItem
            // 
            flatRidesToolStripMenuItem.Name = "flatRidesToolStripMenuItem";
            flatRidesToolStripMenuItem.Size = new Size(178, 26);
            flatRidesToolStripMenuItem.Text = "Flat Rides";
            flatRidesToolStripMenuItem.Click += flatRidesToolStripMenuItem_Click;
            // 
            // rollercoastersToolStripMenuItem
            // 
            rollercoastersToolStripMenuItem.Name = "rollercoastersToolStripMenuItem";
            rollercoastersToolStripMenuItem.Size = new Size(178, 26);
            rollercoastersToolStripMenuItem.Text = "Rollercoasters";
            rollercoastersToolStripMenuItem.Click += rollercoastersToolStripMenuItem_Click;
            // 
            // darkRidesToolStripMenuItem
            // 
            darkRidesToolStripMenuItem.Name = "darkRidesToolStripMenuItem";
            darkRidesToolStripMenuItem.Size = new Size(178, 26);
            darkRidesToolStripMenuItem.Text = "Dark Rides";
            darkRidesToolStripMenuItem.Click += darkRidesToolStripMenuItem_Click;
            // 
            // manufacturersToolStripMenuItem
            // 
            manufacturersToolStripMenuItem.Name = "manufacturersToolStripMenuItem";
            manufacturersToolStripMenuItem.Size = new Size(123, 25);
            manufacturersToolStripMenuItem.Text = "Manufacturers";
            manufacturersToolStripMenuItem.Click += manufacturersToolStripMenuItem_Click;
            // 
            // rideTypesToolStripMenuItem
            // 
            rideTypesToolStripMenuItem.Name = "rideTypesToolStripMenuItem";
            rideTypesToolStripMenuItem.Size = new Size(96, 25);
            rideTypesToolStripMenuItem.Text = "Ride Types";
            rideTypesToolStripMenuItem.Click += rideTypesToolStripMenuItem_Click;
            // 
            // visitsToolStripMenuItem
            // 
            visitsToolStripMenuItem.Name = "visitsToolStripMenuItem";
            visitsToolStripMenuItem.Size = new Size(59, 25);
            visitsToolStripMenuItem.Text = "Visits";
            visitsToolStripMenuItem.Click += visitsToolStripMenuItem_Click;
            // 
            // profileToolStripMenuItem
            // 
            profileToolStripMenuItem.Name = "profileToolStripMenuItem";
            profileToolStripMenuItem.Size = new Size(67, 25);
            profileToolStripMenuItem.Text = "Profile";
            profileToolStripMenuItem.Click += profileToolStripMenuItem_Click;
            // 
            // ViewPanel
            // 
            ViewPanel.AutoScroll = true;
            ViewPanel.Location = new Point(12, 43);
            ViewPanel.Name = "ViewPanel";
            ViewPanel.Size = new Size(1210, 656);
            ViewPanel.TabIndex = 1;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(1155, 3);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 0;
            saveButton.Text = "Save Data";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1234, 711);
            Controls.Add(saveButton);
            Controls.Add(ViewPanel);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MaximumSize = new Size(1250, 750);
            MinimumSize = new Size(1250, 750);
            Name = "Form1";
            Text = "Theme Park Visits Tracker";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem feedToolStripMenuItem;
        private ToolStripMenuItem themeParksToolStripMenuItem;
        private ToolStripMenuItem ridesToolStripMenuItem;
        private ToolStripMenuItem flatRidesToolStripMenuItem;
        private ToolStripMenuItem rollercoastersToolStripMenuItem;
        private ToolStripMenuItem darkRidesToolStripMenuItem;
        private ToolStripMenuItem visitsToolStripMenuItem;
        private ToolStripMenuItem profileToolStripMenuItem;
        private Panel ViewPanel;
        private ToolStripMenuItem manufacturersToolStripMenuItem;
        private Button saveButton;
        private ToolStripMenuItem rideTypesToolStripMenuItem;
    }
}