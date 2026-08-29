using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public partial class MainForm : Form
    {
        // Cool blue theme colours
        private Color backgroundBlue = Color.FromArgb(235, 244, 255);
        private Color darkBlue = Color.FromArgb(25, 55, 109);
        private Color primaryBlue = Color.FromArgb(37, 99, 235);
        private Color lightBlue = Color.FromArgb(219, 234, 254);
        private Color white = Color.White;
        private Color textDark = Color.FromArgb(31, 41, 55);

        public MainForm()
        {
            InitializeComponent();
            SetupMainForm();
        }

        private void SetupMainForm()
        {
            // Form settings
            this.Text = "Municipal Services Application";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 600);
            this.MinimumSize = new Size(750, 500);
            this.BackColor = backgroundBlue;

            CreateInterface();
        }

        private void CreateInterface()
        {
            // Header panel
            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 120;
            headerPanel.BackColor = darkBlue;

            this.Controls.Add(headerPanel);

            // Application title
            Label titleLabel = new Label();
            titleLabel.Text = "Municipal Services";
            titleLabel.ForeColor = white;
            titleLabel.Font = new Font("Segoe UI", 26, FontStyle.Bold);
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(40, 20);

            headerPanel.Controls.Add(titleLabel);

            // Subtitle
            Label subtitleLabel = new Label();
            subtitleLabel.Text = "Connecting citizens with their municipality";
            subtitleLabel.ForeColor = Color.FromArgb(219, 234, 254);
            subtitleLabel.Font = new Font("Segoe UI", 11);
            subtitleLabel.AutoSize = true;
            subtitleLabel.Location = new Point(43, 70);

            headerPanel.Controls.Add(subtitleLabel);

            // Welcome message
            Label welcomeLabel = new Label();
            welcomeLabel.Text = "How can we help you today?";
            welcomeLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            welcomeLabel.ForeColor = textDark;
            welcomeLabel.AutoSize = true;
            welcomeLabel.Location = new Point(40, 150);

            this.Controls.Add(welcomeLabel);

            // Report Issues button
            Button reportButton = CreateMenuButton(
                "Report Issues",
                "Report a municipal problem or request a service",
                40,
                205,
                primaryBlue
            );

            reportButton.Click += ReportButton_Click;

            this.Controls.Add(reportButton);

            // Events button
            Button eventsButton = CreateMenuButton(
                "Local Events & Announcements",
                "View upcoming municipal events and announcements",
                40,
                325,
                Color.FromArgb(148, 163, 184)
            );

            eventsButton.Enabled = false;
            eventsButton.Text += "  (Coming Soon)";

            this.Controls.Add(eventsButton);

            // Status button
            Button statusButton = CreateMenuButton(
                "Service Request Status",
                "Track the progress of your service requests",
                470,
                205,
                Color.FromArgb(148, 163, 184)
            );

            statusButton.Enabled = false;
            statusButton.Text += "  (Coming Soon)";

            this.Controls.Add(statusButton);

            // Information card
            Panel infoPanel = new Panel();
            infoPanel.Location = new Point(470, 325);
            infoPanel.Size = new Size(370, 120);
            infoPanel.BackColor = white;
            infoPanel.BorderStyle = BorderStyle.FixedSingle;

            Label infoTitle = new Label();
            infoTitle.Text = "About this application";
            infoTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            infoTitle.ForeColor = darkBlue;
            infoTitle.AutoSize = true;
            infoTitle.Location = new Point(20, 15);

            Label infoText = new Label();
            infoText.Text =
                "Use this application to report municipal issues\n" +
                "and help improve services in your community.";
            infoText.Font = new Font("Segoe UI", 10);
            infoText.ForeColor = textDark;
            infoText.AutoSize = true;
            infoText.Location = new Point(20, 45);

            infoPanel.Controls.Add(infoTitle);
            infoPanel.Controls.Add(infoText);

            this.Controls.Add(infoPanel);

            // Footer
            Label footer = new Label();
            footer.Text = "Municipal Services Application • South Africa";
            footer.Font = new Font("Segoe UI", 9);
            footer.ForeColor = Color.FromArgb(100, 116, 139);
            footer.AutoSize = true;
            footer.Location = new Point(40, 510);

            this.Controls.Add(footer);
        }

        private Button CreateMenuButton(
            string title,
            string description,
            int x,
            int y,
            Color buttonColor)
        {
            Button button = new Button();

            button.Text = title;
            button.Location = new Point(x, y);
            button.Size = new Size(370, 100);

            button.BackColor = buttonColor;
            button.ForeColor = white;

            button.Font = new Font(
                "Segoe UI",
                13,
                FontStyle.Bold
            );

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;

            button.TextAlign = ContentAlignment.MiddleCenter;

            button.Cursor = Cursors.Hand;

            button.Tag = description;

            return button;
        }

        private void ReportButton_Click(object sender, EventArgs e)
        {
            ReportIssueForm reportForm = new ReportIssueForm();

            reportForm.ShowDialog();
        }
    }
}