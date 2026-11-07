
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.DataStructures;

namespace MunicipalServicesApp
{
    public partial class ServiceRequestStatusForm : Form
    {
        // --------------------------------------------------
        // THEME COLOURS
        // --------------------------------------------------

        private readonly Color backgroundBlue =
            Color.FromArgb(235, 244, 255);

        private readonly Color darkBlue =
            Color.FromArgb(25, 55, 109);

        private readonly Color primaryBlue =
            Color.FromArgb(37, 99, 235);

        private readonly Color lightBlue =
            Color.FromArgb(219, 234, 254);

        private readonly Color textDark =
            Color.FromArgb(31, 41, 55);

        private readonly Color white = Color.White;

        // --------------------------------------------------
        // FORM CONTROLS
        // --------------------------------------------------

        private TextBox txtReference = new TextBox();

        private DataGridView requestsGrid = new DataGridView();

        private Label lblTotal = new Label();
        private Label lblActive = new Label();
        private Label lblResolved = new Label();
        private Label lblSubmitted = new Label();

        private Label lblReference = new Label();
        private Label lblCategory = new Label();
        private Label lblLocation = new Label();
        private Label lblDepartment = new Label();
        private Label lblStatus = new Label();
        private Label lblPriority = new Label();
        private Label lblDescription = new Label();
        private Label lblSubmittedDate = new Label();
        private Label lblExpectedDate = new Label();

        private ListBox historyList = new ListBox();

        private Label lblResultCount = new Label();
        private Label lblUrgentRequest = new Label();

        private List<ServiceRequest> displayedRequests =
            new List<ServiceRequest>();

        // Integrated data structure manager
        private ServiceRequestDataManager dataManager;

        // --------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------

        public ServiceRequestStatusForm()
        {
            InitializeComponent();

            // Initialise the manager.
            // Its constructor automatically loads the requests
            // into the integrated data structures.
            dataManager = new ServiceRequestDataManager();

            SetupForm();
            CreateInterface();
            LoadRequests();
        }

        // --------------------------------------------------
        // FORM SETTINGS
        // --------------------------------------------------

        private void SetupForm()
        {
            Text = "Service Request Status";

            StartPosition = FormStartPosition.CenterParent;

            Size = new Size(1150, 780);
            MinimumSize = new Size(950, 650);

            BackColor = backgroundBlue;
            Font = new Font("Segoe UI", 9.5F);
        }

        // --------------------------------------------------
        // MAIN INTERFACE
        // --------------------------------------------------

        private void CreateInterface()
        {
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                BackColor = backgroundBlue,
                Padding = new Padding(0)
            };

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 95));

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 105));

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 75));

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            mainLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 55));

            mainLayout.Controls.Add(CreateHeader(), 0, 0);
            mainLayout.Controls.Add(CreateSummaryPanel(), 0, 1);
            mainLayout.Controls.Add(CreateSearchPanel(), 0, 2);
            mainLayout.Controls.Add(CreateContentPanel(), 0, 3);
            mainLayout.Controls.Add(CreateFooter(), 0, 4);

            Controls.Add(mainLayout);
        }

        // --------------------------------------------------
        // HEADER
        // --------------------------------------------------

        private Panel CreateHeader()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = darkBlue
            };

            Label title = new Label
            {
                Text = "Service Request Status",
                ForeColor = white,
                Font = new Font(
                    "Segoe UI", 22, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, 15)
            };

            Label subtitle = new Label
            {
                Text = "Track and monitor your municipal service requests",
                ForeColor = lightBlue,
                Font = new Font("Segoe UI", 10.5F),
                AutoSize = true,
                Location = new Point(28, 57)
            };

            panel.Controls.Add(title);
            panel.Controls.Add(subtitle);

            return panel;
        }

        // --------------------------------------------------
        // SUMMARY CARDS
        // --------------------------------------------------

        private Panel CreateSummaryPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = backgroundBlue,
                Padding = new Padding(12, 8, 12, 5)
            };

            TableLayoutPanel cards = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = backgroundBlue
            };

            for (int i = 0; i < 4; i++)
            {
                cards.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 25));
            }

            lblTotal = CreateSummaryCard(
                cards, 0, "Total Requests");

            lblActive = CreateSummaryCard(
                cards, 1, "Active Requests");

            lblResolved = CreateSummaryCard(
                cards, 2, "Resolved Requests");

            lblSubmitted = CreateSummaryCard(
                cards, 3, "New Submissions");

            panel.Controls.Add(cards);

            return panel;
        }

        private Label CreateSummaryCard(
            TableLayoutPanel layout,
            int column,
            string heading)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = white,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label title = new Label
            {
                Text = heading,
                ForeColor = textDark,
                Font = new Font(
                    "Segoe UI", 9, FontStyle.Bold),
                Location = new Point(12, 10),
                AutoSize = true
            };

            Label number = new Label
            {
                Text = "0",
                ForeColor = primaryBlue,
                Font = new Font(
                    "Segoe UI", 21, FontStyle.Bold),
                Location = new Point(12, 35),
                AutoSize = true
            };

            card.Controls.Add(title);
            card.Controls.Add(number);

            layout.Controls.Add(card, column, 0);

            return number;
        }

        // --------------------------------------------------
        // SEARCH PANEL
        // --------------------------------------------------

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = white,
                Padding = new Padding(15, 8, 15, 8)
            };

            Label searchLabel = new Label
            {
                Text = "Search by reference number:",
                AutoSize = true,
                ForeColor = darkBlue,
                Font = new Font(
                    "Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(15, 8)
            };

            txtReference = new TextBox
            {
                Location = new Point(15, 34),
                Size = new Size(330, 28),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "e.g. SR-2026-0002"
            };

            Button searchButton = CreateButton(
                "Search", primaryBlue);

            searchButton.Location = new Point(360, 30);
            searchButton.Size = new Size(100, 35);
            searchButton.Click += SearchButton_Click;

            Button clearButton = CreateButton(
                "Show All",
                Color.FromArgb(226, 232, 240));

            clearButton.ForeColor = darkBlue;
            clearButton.Location = new Point(470, 30);
            clearButton.Size = new Size(100, 35);
            clearButton.Click += ClearButton_Click;

            lblUrgentRequest = new Label
            {
                AutoSize = false,
                Location = new Point(600, 25),
                Size = new Size(450, 40),
                Font = new Font(
                    "Segoe UI", 9F, FontStyle.Bold),
                ForeColor = darkBlue,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "Most urgent request: Loading..."
            };

            panel.Controls.Add(searchLabel);
            panel.Controls.Add(txtReference);
            panel.Controls.Add(searchButton);
            panel.Controls.Add(clearButton);
            panel.Controls.Add(lblUrgentRequest);

            return panel;
        }

        // --------------------------------------------------
        // MAIN CONTENT
        // --------------------------------------------------

        private Panel CreateContentPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = backgroundBlue,
                Padding = new Padding(12, 8, 12, 8)
            };

            TableLayoutPanel contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = backgroundBlue,
                Padding = new Padding(0)
            };

            contentLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 55));

            contentLayout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 45));

            contentLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            Panel leftPanel = CreateRequestListPanel();
            Panel rightPanel = CreateDetailsPanel();

            leftPanel.Dock = DockStyle.Fill;
            rightPanel.Dock = DockStyle.Fill;

            leftPanel.Margin = new Padding(0, 0, 6, 0);
            rightPanel.Margin = new Padding(6, 0, 0, 0);

            contentLayout.Controls.Add(leftPanel, 0, 0);
            contentLayout.Controls.Add(rightPanel, 1, 0);

            panel.Controls.Add(contentLayout);

            return panel;
        }

        // --------------------------------------------------
        // REQUEST LIST
        // --------------------------------------------------

        private Panel CreateRequestListPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = white,
                Padding = new Padding(10),
                Margin = new Padding(5)
            };

            Label heading = new Label
            {
                Text = "Service Requests",
                Font = new Font(
                    "Segoe UI", 13, FontStyle.Bold),
                ForeColor = darkBlue,
                Dock = DockStyle.Top,
                Height = 35
            };

            requestsGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = white,
                BorderStyle = BorderStyle.None,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 8.5F),
                ColumnHeadersHeight = 38,
                RowTemplate = { Height = 42 },
                EnableHeadersVisualStyles = false
            };

            requestsGrid.ColumnHeadersDefaultCellStyle.BackColor =
                darkBlue;

            requestsGrid.ColumnHeadersDefaultCellStyle.ForeColor =
                white;

            requestsGrid.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 8.5F, FontStyle.Bold);

            requestsGrid.DefaultCellStyle.SelectionBackColor =
                lightBlue;

            requestsGrid.DefaultCellStyle.SelectionForeColor =
                darkBlue;

            requestsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Reference",
                    DataPropertyName = "ReferenceNumber",
                    FillWeight = 95
                });

            requestsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Category",
                    DataPropertyName = "Category",
                    FillWeight = 90
                });

            requestsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Status",
                    DataPropertyName = "Status",
                    FillWeight = 85
                });

            requestsGrid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Priority",
                    DataPropertyName = "Priority",
                    FillWeight = 70
                });

            requestsGrid.SelectionChanged +=
                RequestsGrid_SelectionChanged;

            lblResultCount = new Label
            {
                Text = "0 requests found",
                Dock = DockStyle.Bottom,
                Height = 25,
                ForeColor = Color.FromArgb(100, 116, 139),
                TextAlign = ContentAlignment.MiddleLeft
            };

            panel.Controls.Add(requestsGrid);
            panel.Controls.Add(lblResultCount);
            panel.Controls.Add(heading);

            return panel;
        }

        // --------------------------------------------------
        // REQUEST DETAILS
        // --------------------------------------------------

        private Panel CreateDetailsPanel()
        {
            Panel outerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = white,
                Padding = new Padding(10)
            };

            // Separate layout keeps the heading fixed.
            TableLayoutPanel detailsLayout =
                new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 2,
                    BackColor = white,
                    Padding = new Padding(0)
                };

            detailsLayout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 38));

            detailsLayout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            // Fixed Request Details heading
            Label heading = new Label
            {
                Text = "Request Details",
                Dock = DockStyle.Fill,
                Font = new Font(
                    "Segoe UI", 13, FontStyle.Bold),
                ForeColor = darkBlue,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Scrollable information section
            Panel detailsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = white,
                AutoScroll = true,
                Padding = new Padding(5)
            };

            int y = 8;

            lblReference = CreateDetailLabel(
                detailsPanel, "Reference: -", ref y);

            lblCategory = CreateDetailLabel(
                detailsPanel, "Category: -", ref y);

            lblLocation = CreateDetailLabel(
                detailsPanel, "Location: -", ref y);

            lblDepartment = CreateDetailLabel(
                detailsPanel, "Department: -", ref y);

            lblStatus = CreateDetailLabel(
                detailsPanel, "Status: -", ref y);

            lblPriority = CreateDetailLabel(
                detailsPanel, "Priority: -", ref y);

            lblSubmittedDate = CreateDetailLabel(
                detailsPanel, "Date submitted: -", ref y);

            lblExpectedDate = CreateDetailLabel(
                detailsPanel, "Expected resolution: -", ref y);

            // Description
            lblDescription = new Label
            {
                Text = "Description: -",
                Location = new Point(12, y),
                Size = new Size(320, 65),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Left |
                         AnchorStyles.Right,
                ForeColor = textDark,
                Font = new Font("Segoe UI", 9F),
                AutoEllipsis = true
            };

            detailsPanel.Controls.Add(lblDescription);

            y += 75;

            // Status History heading
            Label historyHeading = new Label
            {
                Text = "Status History",
                Location = new Point(12, y),
                Size = new Size(250, 28),
                ForeColor = darkBlue,
                Font = new Font(
                    "Segoe UI", 11, FontStyle.Bold)
            };

            detailsPanel.Controls.Add(historyHeading);

            y += 32;

            // Status History list
            historyList = new ListBox
            {
                Location = new Point(12, y),
                Size = new Size(320, 180),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Left |
                         AnchorStyles.Right,
                Font = new Font("Segoe UI", 8.5F),
                BackColor = backgroundBlue,
                ForeColor = textDark,
                BorderStyle = BorderStyle.FixedSingle,
                HorizontalScrollbar = true,
                IntegralHeight = false
            };

            detailsPanel.Controls.Add(historyList);

            detailsLayout.Controls.Add(heading, 0, 0);
            detailsLayout.Controls.Add(detailsPanel, 0, 1);

            outerPanel.Controls.Add(detailsLayout);

            return outerPanel;
        }

        // --------------------------------------------------
        // CREATE DETAIL LABELS
        // --------------------------------------------------

        private Label CreateDetailLabel(
            Panel panel,
            string text,
            ref int y)
        {
            Label label = new Label
            {
                Text = text,
                Location = new Point(12, y),
                Size = new Size(320, 23),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Left |
                         AnchorStyles.Right,
                ForeColor = textDark,
                Font = new Font("Segoe UI", 9F)
            };

            panel.Controls.Add(label);

            y += 27;

            return label;
        }

        // --------------------------------------------------
        // FOOTER
        // --------------------------------------------------

        private Panel CreateFooter()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = white
            };

            Button backButton = CreateButton(
                "Back to Main Menu", darkBlue);

            backButton.Size = new Size(190, 38);

            backButton.Anchor = AnchorStyles.Right |
                                AnchorStyles.Top;

            backButton.Location = new Point(0, 7);

            backButton.Click += (sender, e) =>
            {
                Close();
            };

            panel.Resize += (sender, e) =>
            {
                backButton.Location = new Point(
                    panel.Width - backButton.Width - 15,
                    7);
            };

            panel.Controls.Add(backButton);

            return panel;
        }

        // --------------------------------------------------
        // BUTTON DESIGN
        // --------------------------------------------------

        private Button CreateButton(
            string text,
            Color backColor)
        {
            Button button = new Button
            {
                Text = text,
                BackColor = backColor,
                ForeColor = white,
                Font = new Font(
                    "Segoe UI", 9.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;

            return button;
        }

        // --------------------------------------------------
        // LOAD REQUESTS
        // --------------------------------------------------

        private void LoadRequests()
        {
            displayedRequests =
                ServiceRequestRepository.GetAll().ToList();

            UpdateSummaryCards();

            DisplayRequests(displayedRequests);

            UpdateMostUrgentRequest();
        }

        // --------------------------------------------------
        // DISPLAY MOST URGENT REQUEST USING MAX HEAP
        // --------------------------------------------------

        private void UpdateMostUrgentRequest()
        {
            ServiceRequest? urgentRequest =
                dataManager.GetMostUrgentRequest();

            if (urgentRequest == null)
            {
                lblUrgentRequest.Text =
                    "Most urgent request: None available";

                return;
            }

            lblUrgentRequest.Text =
                "Most urgent: "
                + urgentRequest.ReferenceNumber
                + " | "
                + urgentRequest.Priority;
        }

        // --------------------------------------------------
        // UPDATE SUMMARY CARDS
        // --------------------------------------------------

        private void UpdateSummaryCards()
        {
            var allRequests =
                ServiceRequestRepository.GetAll();

            int total = allRequests.Count;

            int resolved = allRequests.Count(request =>
                request.Status == ServiceRequestStatus.Resolved ||
                request.Status == ServiceRequestStatus.Closed);

            int submitted = allRequests.Count(request =>
                request.Status == ServiceRequestStatus.Submitted);

            int active = total - resolved - submitted;

            lblTotal.Text = total.ToString();
            lblActive.Text = active.ToString();
            lblResolved.Text = resolved.ToString();
            lblSubmitted.Text = submitted.ToString();
        }

        // --------------------------------------------------
        // DISPLAY REQUESTS IN TABLE
        // --------------------------------------------------

        private void DisplayRequests(
            List<ServiceRequest> requests)
        {
            displayedRequests = requests;

            requestsGrid.DataSource = null;
            requestsGrid.DataSource = requests;

            lblResultCount.Text =
                requests.Count + " request(s) found.";

            if (requestsGrid.Rows.Count > 0)
            {
                requestsGrid.ClearSelection();

                requestsGrid.Rows[0].Selected = true;

                requestsGrid.CurrentCell =
                    requestsGrid.Rows[0].Cells[0];

                ShowRequestDetails(requests[0]);
            }
            else
            {
                ClearDetails();
            }
        }

        // --------------------------------------------------
        // SEARCH USING BINARY SEARCH TREE
        // --------------------------------------------------

        private void SearchButton_Click(
            object sender,
            EventArgs e)
        {
            string reference = txtReference.Text.Trim();

            if (string.IsNullOrWhiteSpace(reference))
            {
                MessageBox.Show(
                    "Please enter a service request reference number.",
                    "Search Request",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            ServiceRequest? request =
                dataManager.FindUsingBST(reference);

            if (request == null)
            {
                DisplayRequests(new List<ServiceRequest>());

                MessageBox.Show(
                    "No service request was found with reference number: "
                    + reference,
                    "Request Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            DisplayRequests(
                new List<ServiceRequest> { request });
        }

        // --------------------------------------------------
        // SHOW ALL REQUESTS
        // --------------------------------------------------

        private void ClearButton_Click(
            object sender,
            EventArgs e)
        {
            txtReference.Clear();

            LoadRequests();
        }

        // --------------------------------------------------
        // SELECTED REQUEST
        // --------------------------------------------------

        private void RequestsGrid_SelectionChanged(
            object? sender,
            EventArgs e)
        {
            if (requestsGrid.SelectedRows.Count == 0)
            {
                return;
            }

            ServiceRequest? selectedRequest =
                requestsGrid.SelectedRows[0].DataBoundItem
                as ServiceRequest;

            if (selectedRequest != null)
            {
                ShowRequestDetails(selectedRequest);
            }
        }

        // --------------------------------------------------
        // DISPLAY SELECTED REQUEST DETAILS
        // --------------------------------------------------

        private void ShowRequestDetails(
            ServiceRequest request)
        {
            lblReference.Text =
                "Reference: " + request.ReferenceNumber;

            lblCategory.Text =
                "Category: " + request.Category;

            lblLocation.Text =
                "Location: " + request.Location;

            lblDepartment.Text =
                "Department: " +
                FormatEnum(request.Department.ToString());

            lblStatus.Text =
                "Status: " +
                FormatEnum(request.Status.ToString());

            lblPriority.Text =
                "Priority: " +
                FormatEnum(request.Priority.ToString());

            lblSubmittedDate.Text =
                "Date submitted: " +
                request.DateSubmitted.ToString("dd MMM yyyy");

            lblExpectedDate.Text =
                "Expected resolution: " +
                (request.ExpectedResolutionDate.HasValue
                    ? request.ExpectedResolutionDate.Value.ToString("dd MMM yyyy")
                    : "Not specified");

            lblDescription.Text =
                "Description: " + request.Description;

            historyList.Items.Clear();

            if (request.StatusHistory.Count == 0)
            {
                historyList.Items.Add(
                    "No status history available.");
            }
            else
            {
                foreach (ServiceRequestStatusEntry entry
                    in request.StatusHistory
                    .OrderBy(item => item.ChangedAt))
                {
                    // One history record per line.
                    string historyText =
                        entry.ChangedAt.ToString("dd MMM yyyy HH:mm")
                        + " | "
                        + FormatEnum(entry.Status.ToString())
                        + " | "
                        + entry.Notes;

                    historyList.Items.Add(historyText);
                }
            }
        }

        // --------------------------------------------------
        // CLEAR REQUEST DETAILS
        // --------------------------------------------------

        private void ClearDetails()
        {
            lblReference.Text = "Reference: -";
            lblCategory.Text = "Category: -";
            lblLocation.Text = "Location: -";
            lblDepartment.Text = "Department: -";
            lblStatus.Text = "Status: -";
            lblPriority.Text = "Priority: -";
            lblSubmittedDate.Text = "Date submitted: -";
            lblExpectedDate.Text = "Expected resolution: -";
            lblDescription.Text = "Description: -";

            historyList.Items.Clear();

            historyList.Items.Add(
                "Select a request to view its history.");
        }

        // --------------------------------------------------
        // FORMAT ENUM VALUES FOR DISPLAY
        // --------------------------------------------------

        private string FormatEnum(string value)
        {
            string result = string.Empty;

            foreach (char character in value)
            {
                if (char.IsUpper(character) &&
                    result.Length > 0)
                {
                    result += " ";
                }

                result += character;
            }

            return result;
        }
    }
}
