
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public partial class MainForm : Form
    {
        // theme colours
        private readonly Color backgroundBlue = Color.FromArgb(235, 244, 255);
        private readonly Color darkBlue = Color.FromArgb(25, 55, 109);
        private readonly Color primaryBlue = Color.FromArgb(37, 99, 235);
        private readonly Color lightBlue = Color.FromArgb(219, 234, 254);
        private readonly Color white = Color.White;
        private readonly Color textDark = Color.FromArgb(31, 41, 55);

        public MainForm()
        {
            InitializeComponent();
            SetupMainForm();
        }

        private void SetupMainForm()
        {
            Text = "Municipal Services Application";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(950, 650);
            MinimumSize = new Size(780, 550);
            BackColor = backgroundBlue;
            Font = new Font("Segoe UI", 10F);

            CreateInterface();
        }

        private void CreateInterface()
        {
            // Header
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = darkBlue
            };

            Label titleLabel = new Label
            {
                Text = "Municipal Services",
                ForeColor = white,
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            Label subtitleLabel = new Label
            {
                Text = "Connecting citizens with their municipality",
                ForeColor = lightBlue,
                Font = new Font("Segoe UI", 11F),
                AutoSize = true,
                Location = new Point(33, 75)
            };

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(subtitleLabel);

            // Responsive content layout
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(22, 15, 22, 12),
                BackColor = backgroundBlue
            };

            mainLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50));
            mainLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50));

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 65));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 42));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 42));
            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 16));

            // Welcome heading
            Label welcomeLabel = new Label
            {
                Text = "How can we help you today?",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = textDark,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            mainLayout.Controls.Add(welcomeLabel, 0, 0);
            mainLayout.SetColumnSpan(welcomeLabel, 2);

            // Report Issues
            Button reportButton = CreateMenuButton(
                "Report Issues",
                "Report a municipal problem",
                primaryBlue);

            reportButton.Click += ReportButton_Click;
            mainLayout.Controls.Add(reportButton, 0, 1);

            // Service Request Status - reserved for Task 3
            Button statusButton = CreateMenuButton(
                "Service Request Status",
                "Coming Soon - Task 3",
                Color.FromArgb(148, 163, 184));

            statusButton.Enabled = false;
            mainLayout.Controls.Add(statusButton, 1, 1);

            // Local Events - enabled for Part 2
            Button eventsButton = CreateMenuButton(
                "Local Events & Announcements",
                "Explore events and community announcements",
                primaryBlue);

            eventsButton.Click += EventsButton_Click;
            mainLayout.Controls.Add(eventsButton, 0, 2);

            // Information card
            Panel infoPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = white,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(8)
            };

            Label infoTitle = new Label
            {
                Text = "About this application",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = darkBlue,
                Location = new Point(15, 12),
                AutoSize = true
            };

            Label infoText = new Label
            {
                Text = "Report municipal issues, explore local events\n" +
                       "and stay informed about your community.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = textDark,
                Location = new Point(15, 43),
                AutoSize = true
            };

            infoPanel.Controls.Add(infoTitle);
            infoPanel.Controls.Add(infoText);
            mainLayout.Controls.Add(infoPanel, 1, 2);

            // Footer
            Label footer = new Label
            {
                Text = "Municipal Services Application • South Africa",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            mainLayout.Controls.Add(footer, 0, 3);
            mainLayout.SetColumnSpan(footer, 2);

            // Add header first, then fill content
            Controls.Add(mainLayout);
            Controls.Add(headerPanel);
        }

        private Button CreateMenuButton(
            string title,
            string description,
            Color buttonColor)
        {
            Button button = new Button
            {
                Text = title + Environment.NewLine + description,
                Dock = DockStyle.Fill,
                Margin = new Padding(8),
                BackColor = buttonColor,
                ForeColor = white,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void ReportButton_Click(object sender, EventArgs e)
        {
            using (ReportIssueForm reportForm = new ReportIssueForm())
            {
                reportForm.ShowDialog(this);
            }
        }

        private void EventsButton_Click(object sender, EventArgs e)
        {
            using (EventsForm eventsForm = new EventsForm())
            {
                eventsForm.ShowDialog(this);
            }
        }
    }
}
