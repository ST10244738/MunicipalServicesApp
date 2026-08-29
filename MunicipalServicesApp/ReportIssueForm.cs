using MunicipalServicesApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public partial class ReportIssueForm : Form
    {
        // Stores reported issues during application runtime
        private static List<Issue> reportedIssues = new List<Issue>();

        // Controls
        private TextBox txtLocation;
        private ComboBox cmbCategory;
        private RichTextBox txtDescription;
        private Button btnAttach;
        private Button btnSubmit;
        private Button btnBack;
        private Label lblAttachment;
        private Label lblProgress;
        private ProgressBar progressBar;

        // Selected attachment
        private string selectedAttachmentPath = "";

        // Theme
        private Color backgroundBlue = Color.FromArgb(235, 244, 255);
        private Color darkBlue = Color.FromArgb(25, 55, 109);
        private Color primaryBlue = Color.FromArgb(37, 99, 235);
        private Color lightBlue = Color.FromArgb(219, 234, 254);
        private Color white = Color.White;
        private Color textDark = Color.FromArgb(31, 41, 55);

        public ReportIssueForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Report a Municipal Issue";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(900, 700);
            this.MinimumSize = new Size(800, 600);
            this.BackColor = backgroundBlue;

            CreateInterface();
        }

        private void CreateInterface()
        {
            // ==========================
            // HEADER
            // ==========================

            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 100;
            headerPanel.BackColor = darkBlue;

            this.Controls.Add(headerPanel);

            Label titleLabel = new Label();
            titleLabel.Text = "Report an Issue";
            titleLabel.Font = new Font(
                "Segoe UI",
                24,
                FontStyle.Bold
            );

            titleLabel.ForeColor = white;
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(35, 18);

            headerPanel.Controls.Add(titleLabel);

            Label subtitleLabel = new Label();
            subtitleLabel.Text =
                "Tell us about a problem in your community";

            subtitleLabel.Font = new Font(
                "Segoe UI",
                10
            );

            subtitleLabel.ForeColor =
                Color.FromArgb(219, 234, 254);

            subtitleLabel.AutoSize = true;
            subtitleLabel.Location = new Point(38, 62);

            headerPanel.Controls.Add(subtitleLabel);

            // ==========================
            // LOCATION
            // ==========================

            Label lblLocation = CreateLabel(
                "Location",
                40,
                125
            );

            this.Controls.Add(lblLocation);

            txtLocation = new TextBox();

            txtLocation.Location =
                new Point(40, 155);

            txtLocation.Size =
                new Size(390, 30);

            txtLocation.Font =
                new Font("Segoe UI", 11);

            txtLocation.PlaceholderText =
                "Enter the location of the issue";

            this.Controls.Add(txtLocation);

            // ==========================
            // CATEGORY
            // ==========================

            Label lblCategory = CreateLabel(
                "Issue Category",
                470,
                125
            );

            this.Controls.Add(lblCategory);

            cmbCategory = new ComboBox();

            cmbCategory.Location =
                new Point(470, 155);

            cmbCategory.Size =
                new Size(390, 30);

            cmbCategory.Font =
                new Font("Segoe UI", 11);

            cmbCategory.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbCategory.Items.AddRange(
                new string[]
                {
                    "Roads",
                    "Water Supply",
                    "Electricity",
                    "Sanitation",
                    "Waste Management",
                    "Street Lighting",
                    "Public Safety",
                    "Other"
                }
            );

            this.Controls.Add(cmbCategory);

            // ==========================
            // DESCRIPTION
            // ==========================

            Label lblDescription = CreateLabel(
                "Description",
                40,
                205
            );

            this.Controls.Add(lblDescription);

            txtDescription = new RichTextBox();

            txtDescription.Location =
                new Point(40, 235);

            txtDescription.Size =
                new Size(820, 130);

            txtDescription.Font =
                new Font("Segoe UI", 11);

            txtDescription.BorderStyle =
                BorderStyle.FixedSingle;

            txtDescription.TextChanged +=
                Description_TextChanged;

            this.Controls.Add(txtDescription);

            // ==========================
            // ATTACHMENT
            // ==========================

            Label lblMedia = CreateLabel(
                "Supporting Image or Document",
                40,
                390
            );

            this.Controls.Add(lblMedia);

            btnAttach = new Button();

            btnAttach.Text =
                "📎  Attach File";

            btnAttach.Location =
                new Point(40, 425);

            btnAttach.Size =
                new Size(160, 45);

            StylePrimaryButton(btnAttach);

            btnAttach.Click +=
                BtnAttach_Click;

            this.Controls.Add(btnAttach);

            lblAttachment = new Label();

            lblAttachment.Text =
                "No file selected";

            lblAttachment.Font =
                new Font("Segoe UI", 9);

            lblAttachment.ForeColor =
                Color.FromArgb(100, 116, 139);

            lblAttachment.Location =
                new Point(215, 438);

            lblAttachment.AutoSize = true;

            this.Controls.Add(lblAttachment);

            // ==========================
            // ENGAGEMENT FEATURE
            // ==========================

            Panel engagementPanel = new Panel();

            engagementPanel.Location =
                new Point(40, 490);

            engagementPanel.Size =
                new Size(820, 80);

            engagementPanel.BackColor =
                lightBlue;

            this.Controls.Add(engagementPanel);

            lblProgress = new Label();

            lblProgress.Text =
                "Let's get started! Complete the fields above to report your issue.";

            lblProgress.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                );

            lblProgress.ForeColor =
                darkBlue;

            lblProgress.Location =
                new Point(15, 10);

            lblProgress.AutoSize = true;

            engagementPanel.Controls.Add(
                lblProgress
            );

            progressBar = new ProgressBar();

            progressBar.Location =
                new Point(15, 42);

            progressBar.Size =
                new Size(790, 15);

            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;

            engagementPanel.Controls.Add(
                progressBar
            );

            // ==========================
            // BUTTONS
            // ==========================

            btnBack = new Button();

            btnBack.Text =
                "← Back to Main Menu";

            btnBack.Location =
                new Point(40, 600);

            btnBack.Size =
                new Size(200, 45);

            btnBack.BackColor =
                Color.FromArgb(100, 116, 139);

            btnBack.ForeColor =
                white;

            btnBack.FlatStyle =
                FlatStyle.Flat;

            btnBack.FlatAppearance.BorderSize =
                0;

            btnBack.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                );

            btnBack.Cursor =
                Cursors.Hand;

            btnBack.Click +=
                BtnBack_Click;

            this.Controls.Add(btnBack);

            btnSubmit = new Button();

            btnSubmit.Text =
                "Submit Issue";

            btnSubmit.Location =
                new Point(660, 600);

            btnSubmit.Size =
                new Size(200, 45);

            StylePrimaryButton(btnSubmit);

            btnSubmit.Click +=
                BtnSubmit_Click;

            this.Controls.Add(btnSubmit);
        }

        private Label CreateLabel(
            string text,
            int x,
            int y)
        {
            Label label = new Label();

            label.Text = text;

            label.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                );

            label.ForeColor =
                textDark;

            label.Location =
                new Point(x, y);

            label.AutoSize = true;

            return label;
        }

        private void StylePrimaryButton(Button button)
        {
            button.BackColor =
                primaryBlue;

            button.ForeColor =
                white;

            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize =
                0;

            button.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                );

            button.Cursor =
                Cursors.Hand;
        }

        // ==========================
        // DESCRIPTION PROGRESS
        // ==========================

        private void Description_TextChanged(
            object sender,
            EventArgs e)
        {
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            int progress = 0;

            if (!string.IsNullOrWhiteSpace(
                txtLocation.Text))
            {
                progress += 25;
            }

            if (cmbCategory.SelectedIndex >= 0)
            {
                progress += 25;
            }

            if (!string.IsNullOrWhiteSpace(
                txtDescription.Text))
            {
                progress += 25;
            }

            if (!string.IsNullOrWhiteSpace(
                selectedAttachmentPath))
            {
                progress += 25;
            }

            progressBar.Value = progress;

            if (progress == 0)
            {
                lblProgress.Text =
                    "Let's get started! Complete the fields above to report your issue.";
            }
            else if (progress < 50)
            {
                lblProgress.Text =
                    "Great start! Keep going — your report is taking shape.";
            }
            else if (progress < 100)
            {
                lblProgress.Text =
                    "You're more than halfway there! Almost done.";
            }
            else
            {
                lblProgress.Text =
                    "Excellent! Your report is ready to be submitted.";
            }
        }

        // ==========================
        // ATTACH FILE
        // ==========================

        private void BtnAttach_Click(
            object sender,
            EventArgs e)
        {
            using (OpenFileDialog dialog =
                new OpenFileDialog())
            {
                dialog.Title =
                    "Select supporting file";

                dialog.Filter =
                    "Images and Documents|*.jpg;*.jpeg;*.png;*.gif;*.pdf;*.doc;*.docx|All Files|*.*";

                dialog.Multiselect = false;

                if (dialog.ShowDialog() ==
                    DialogResult.OK)
                {
                    selectedAttachmentPath =
                        dialog.FileName;

                    lblAttachment.Text =
                        Path.GetFileName(
                            selectedAttachmentPath);

                    lblAttachment.ForeColor =
                        darkBlue;

                    UpdateProgress();
                }
            }
        }

        // ==========================
        // SUBMIT
        // ==========================

        private void BtnSubmit_Click(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                txtLocation.Text))
            {
                MessageBox.Show(
                    "Please enter the location of the issue.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtLocation.Focus();

                return;
            }

            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select an issue category.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                cmbCategory.Focus();

                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtDescription.Text))
            {
                MessageBox.Show(
                    "Please provide a description of the issue.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtDescription.Focus();

                return;
            }

            Issue newIssue = new Issue();

            newIssue.IssueID =
                reportedIssues.Count + 1;

            newIssue.Location =
                txtLocation.Text.Trim();

            newIssue.Category =
                cmbCategory.SelectedItem.ToString();

            newIssue.Description =
                txtDescription.Text.Trim();

            newIssue.AttachmentPath =
                selectedAttachmentPath;

            newIssue.DateReported =
                DateTime.Now;

            reportedIssues.Add(newIssue);

            MessageBox.Show(
                "Your issue has been reported successfully!\n\n" +
                "Issue Reference: MS-" +
                newIssue.IssueID.ToString("D4") +
                "\n\n" +
                "Thank you for helping improve your community.",
                "Report Submitted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            ClearForm();
        }

        // ==========================
        // CLEAR FORM
        // ==========================

        private void ClearForm()
        {
            txtLocation.Clear();

            cmbCategory.SelectedIndex = -1;

            txtDescription.Clear();

            selectedAttachmentPath = "";

            lblAttachment.Text =
                "No file selected";

            lblAttachment.ForeColor =
                Color.FromArgb(100, 116, 139);

            progressBar.Value = 0;

            lblProgress.Text =
                "Let's get started! Complete the fields above to report your issue.";

            txtLocation.Focus();
        }

        // ==========================
        // BACK
        // ==========================

        private void BtnBack_Click(
            object sender,
            EventArgs e)
        {
            this.Close();
        }
    }
}