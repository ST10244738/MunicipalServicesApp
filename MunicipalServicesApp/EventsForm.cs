
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public class EventsForm : Form
    {
        // theme
        private readonly Color backgroundBlue = Color.FromArgb(235, 244, 255);
        private readonly Color darkBlue = Color.FromArgb(25, 55, 109);
        private readonly Color primaryBlue = Color.FromArgb(37, 99, 235);
        private readonly Color lightBlue = Color.FromArgb(219, 234, 254);
        private readonly Color textDark = Color.FromArgb(31, 41, 55);

        // data structures
        private readonly List<MunicipalEvent> allEvents =
            new List<MunicipalEvent>();

        private readonly Dictionary<string, MunicipalEvent> eventsById =
            new Dictionary<string, MunicipalEvent>();

        private readonly SortedDictionary<DateTime, List<MunicipalEvent>> eventsByDate =
            new SortedDictionary<DateTime, List<MunicipalEvent>>();

        private readonly HashSet<string> categories =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private readonly Stack<string> searchHistory =
            new Stack<string>();

        private readonly Dictionary<string, int> categorySearchCounts =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private readonly Queue<MunicipalEvent> upcomingQueue =
            new Queue<MunicipalEvent>();

        // Lower number means higher priority
        private readonly PriorityQueue<MunicipalEvent, int> priorityQueue =
            new PriorityQueue<MunicipalEvent, int>();

        // Form controls
        private TextBox txtSearch;
        private ComboBox cmbCategory;
        private DateTimePicker dtpDate;
        private CheckBox chkUseDate;
        private FlowLayoutPanel eventsPanel;
        private FlowLayoutPanel recommendationsPanel;
        private Label lblResults;
        private Label lblHistory;

        public EventsForm()
        {
            LoadSampleEvents();
            BuildInterface();
            LoadCategories();
            ShowEvents(GetChronologicalEvents());
            ShowRecommendations();
        }

        private void BuildInterface()
        {
            Text = "Local Events and Announcements";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1050, 760);
            MinimumSize = new Size(850, 650);
            BackColor = backgroundBlue;
            Font = new Font("Segoe UI", 9.5F);
            AutoScaleMode = AutoScaleMode.Font;

            // Main layout
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                BackColor = backgroundBlue
            };

            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 85));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 125));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 180));

            Controls.Add(layout);

            // HEADER
            Panel header = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = darkBlue,
                Margin = Padding.Empty
            };

            Label title = new Label
            {
                Text = "Local Events and Announcements",
                Font = new Font("Segoe UI", 21F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(22, 8)
            };

            Label subtitle = new Label
            {
                Text = "Discover what's happening in your community",
                Font = new Font("Segoe UI", 10F),
                ForeColor = lightBlue,
                AutoSize = true,
                Location = new Point(25, 51)
            };

            header.Controls.Add(title);
            header.Controls.Add(subtitle);
            layout.Controls.Add(header, 0, 0);

            // SEARCH AREA
            Panel searchPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = Padding.Empty,
                Padding = new Padding(15, 8, 15, 8)
            };

            searchPanel.Controls.Add(
                MakeLabel("Search keyword", 15, 5));

            txtSearch = new TextBox
            {
                Location = new Point(15, 29),
                Size = new Size(240, 27),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };

            txtSearch.PlaceholderText = "Enter an event or keyword";
            searchPanel.Controls.Add(txtSearch);

            searchPanel.Controls.Add(
                MakeLabel("Category", 270, 5));

            cmbCategory = new ComboBox
            {
                Location = new Point(270, 29),
                Size = new Size(155, 27),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            searchPanel.Controls.Add(cmbCategory);

            searchPanel.Controls.Add(
                MakeLabel("Event date", 440, 5));

            dtpDate = new DateTimePicker
            {
                Location = new Point(440, 29),
                Size = new Size(140, 27),
                Format = DateTimePickerFormat.Short
            };

            searchPanel.Controls.Add(dtpDate);

            chkUseDate = new CheckBox
            {
                Text = "Filter by date",
                Location = new Point(440, 63),
                AutoSize = true,
                ForeColor = textDark
            };

            searchPanel.Controls.Add(chkUseDate);

            Button btnSearch = MakeButton(
                "Search", primaryBlue, Color.White);

            btnSearch.Size = new Size(90, 34);
            btnSearch.Location = new Point(610, 28);
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.Click += (s, e) => SearchEvents();

            searchPanel.Controls.Add(btnSearch);

            Button btnClear = MakeButton(
                "Clear", Color.FromArgb(226, 232, 240), darkBlue);

            btnClear.Size = new Size(80, 34);
            btnClear.Location = new Point(710, 28);
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.Click += (s, e) => ClearSearch();

            searchPanel.Controls.Add(btnClear);

            // buttons 
            searchPanel.Resize += (s, e) =>
            {
                btnClear.Left =
                    searchPanel.ClientSize.Width - btnClear.Width - 15;

                btnSearch.Left =
                    btnClear.Left - btnSearch.Width - 10;
            };

            txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SearchEvents();
                    e.SuppressKeyPress = true;
                }
            };

            layout.Controls.Add(searchPanel, 0, 1);

            // UPCOMING EVENTS
            Panel eventsContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = backgroundBlue,
                Padding = new Padding(12, 5, 12, 5),
                Margin = Padding.Empty
            };

            Label eventsHeading = new Label
            {
                Text = "Upcoming Events",
                Dock = DockStyle.Top,
                Height = 35,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = darkBlue
            };

            eventsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                Padding = new Padding(4),
                BackColor = backgroundBlue
            };

            eventsContainer.Controls.Add(eventsPanel);
            eventsContainer.Controls.Add(eventsHeading);
            layout.Controls.Add(eventsContainer, 0, 2);

            // RECOMMENDATIONS
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(12, 5, 12, 5),
                Margin = Padding.Empty
            };

            Label recommendationHeading = new Label
            {
                Text = "Recommended for You",
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = darkBlue
            };

            recommendationsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                BackColor = Color.White
            };

            Panel footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 32,
                BackColor = Color.White
            };

            Button btnBack = MakeButton(
                "Back to Main Menu", darkBlue, Color.White);

            btnBack.Dock = DockStyle.Right;
            btnBack.Width = 175;
            btnBack.Click += (s, e) => Close();

            lblResults = new Label
            {
                Dock = DockStyle.Left,
                Width = 150,
                ForeColor = textDark,
                TextAlign = ContentAlignment.MiddleLeft
            };

            lblHistory = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(100, 116, 139),
                TextAlign = ContentAlignment.MiddleLeft
            };

            footerPanel.Controls.Add(lblHistory);
            footerPanel.Controls.Add(lblResults);
            footerPanel.Controls.Add(btnBack);

            bottomPanel.Controls.Add(recommendationsPanel);
            bottomPanel.Controls.Add(footerPanel);
            bottomPanel.Controls.Add(recommendationHeading);

            layout.Controls.Add(bottomPanel, 0, 3);

            AcceptButton = btnSearch;
        }

        private Label MakeLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = darkBlue
            };
        }

        private Button MakeButton(
            string text, Color back, Color fore)
        {
            Button button = new Button
            {
                Text = text,
                BackColor = back,
                ForeColor = fore,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        // SAMPLE MUNICIPAL EVENTS
        private void LoadSampleEvents()
        {
            DateTime today = DateTime.Today;

            AddEvent(new MunicipalEvent(
                "EV001", "Community Clean-up Day", "Community",
                today.AddDays(3), "Community Hall",
                "Help clean public spaces in your neighbourhood.", 2));

            AddEvent(new MunicipalEvent(
                "EV002", "Public Health Awareness", "Health",
                today.AddDays(5), "Municipal Clinic",
                "Learn about local health services.", 1));

            AddEvent(new MunicipalEvent(
                "EV003", "Youth Skills Workshop", "Education",
                today.AddDays(7), "Civic Centre",
                "Explore career planning and practical skills.", 2));

            AddEvent(new MunicipalEvent(
                "EV004", "Road Safety Meeting", "Public Safety",
                today.AddDays(2), "Ward Office",
                "Discuss road safety and community improvements.", 1));

            AddEvent(new MunicipalEvent(
                "EV005", "Local Sports Day", "Sports",
                today.AddDays(10), "Sports Ground",
                "A family-friendly sports and recreation day.", 3));

            AddEvent(new MunicipalEvent(
                "EV006", "Small Business Information Session", "Business",
                today.AddDays(12), "Municipal Library",
                "Learn about local business support.", 2));

            AddEvent(new MunicipalEvent(
                "EV007", "Water Supply Maintenance", "Utilities",
                today.AddDays(1), "Ward 4",
                "Planned maintenance may temporarily affect water supply.",
                0, true));

            AddEvent(new MunicipalEvent(
                "EV008", "Library Reading Club", "Education",
                today.AddDays(14), "Public Library",
                "Reading and storytelling for families.", 3));

            AddEvent(new MunicipalEvent(
                "EV009", "Waste Collection Update", "Announcements",
                today.AddDays(4), "All wards",
                "Check the updated waste collection schedule.", 0, true));
        }

        private void AddEvent(MunicipalEvent item)
        {
            allEvents.Add(item);
            eventsById[item.Id] = item;
            categories.Add(item.Category);

            if (!eventsByDate.TryGetValue(
                item.Date.Date, out List<MunicipalEvent> dateList))
            {
                dateList = new List<MunicipalEvent>();
                eventsByDate[item.Date.Date] = dateList;
            }

            dateList.Add(item);
            priorityQueue.Enqueue(item, item.Priority);
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("All categories");

            foreach (string category in categories.OrderBy(c => c))
                cmbCategory.Items.Add(category);

            cmbCategory.SelectedIndex = 0;
        }

        // Uses SortedDictionary for chronological grouping
        // and Queue for first-in, first-out event processing.
        private List<MunicipalEvent> GetChronologicalEvents()
        {
            upcomingQueue.Clear();

            foreach (MunicipalEvent item in eventsByDate
                .Where(group => group.Key >= DateTime.Today)
                .SelectMany(group => group.Value)
                .OrderBy(item => item.Date))
            {
                upcomingQueue.Enqueue(item);
            }

            List<MunicipalEvent> result = new List<MunicipalEvent>();

            while (upcomingQueue.Count > 0)
            {
                result.Add(upcomingQueue.Dequeue());
            }

            return result;
        }

        // SEARCH AND FILTER
        private void SearchEvents()
        {
            string keyword = txtSearch.Text.Trim();

            string category = cmbCategory.SelectedItem?.ToString()
                              ?? "All categories";

            if (!string.IsNullOrWhiteSpace(keyword)
                || category != "All categories"
                || chkUseDate.Checked)
            {
                string search = category + " | " + keyword;

                if (chkUseDate.Checked)
                    search += " | " + dtpDate.Value.ToShortDateString();

                searchHistory.Push(search);

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    string[] words = keyword.Split(
                        new[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);

                    foreach (string word in words)
                    {
                        if (!categorySearchCounts.ContainsKey(word))
                            categorySearchCounts[word] = 0;

                        categorySearchCounts[word]++;
                    }
                }

                if (category != "All categories")
                {
                    if (!categorySearchCounts.ContainsKey(category))
                        categorySearchCounts[category] = 0;

                    categorySearchCounts[category]++;
                }
            }

            IEnumerable<MunicipalEvent> results = GetChronologicalEvents();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                results = results.Where(item =>
                    item.Title.IndexOf(keyword,
                        StringComparison.OrdinalIgnoreCase) >= 0
                    || item.Description.IndexOf(keyword,
                        StringComparison.OrdinalIgnoreCase) >= 0
                    || item.Category.IndexOf(keyword,
                        StringComparison.OrdinalIgnoreCase) >= 0
                    || item.Location.IndexOf(keyword,
                        StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (category != "All categories")
            {
                results = results.Where(item =>
                    item.Category.Equals(category,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (chkUseDate.Checked)
            {
                results = results.Where(item =>
                    item.Date.Date == dtpDate.Value.Date);
            }

            ShowEvents(results.ToList());
            ShowRecommendations();
            UpdateSearchHistoryLabel();
        }

        private void ClearSearch()
        {
            txtSearch.Clear();
            cmbCategory.SelectedIndex = 0;
            chkUseDate.Checked = false;
            dtpDate.Value = DateTime.Today;

            ShowEvents(GetChronologicalEvents());
            ShowRecommendations();

            lblHistory.Text = "Search cleared. Showing upcoming events.";
        }

        // DISPLAY EVENT CARDS
        private void ShowEvents(List<MunicipalEvent> items)
        {
            eventsPanel.SuspendLayout();
            eventsPanel.Controls.Clear();

            foreach (MunicipalEvent item in items)
            {
                Panel card = new Panel
                {
                    Size = new Size(300, 135),
                    BackColor = Color.White,
                    Margin = new Padding(6),
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Hand
                };

                Label title = new Label
                {
                    Text = item.Title,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = darkBlue,
                    Location = new Point(9, 7),
                    Size = new Size(278, 25),
                    Cursor = Cursors.Hand
                };

                Label details = new Label
                {
                    Text = item.Category + " | " +
                           item.Date.ToString("dd MMM yyyy") +
                           Environment.NewLine + item.Location +
                           Environment.NewLine + item.Description,
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = textDark,
                    Location = new Point(9, 35),
                    Size = new Size(278, 67),
                    Cursor = Cursors.Hand
                };

                Label priority = new Label
                {
                    Text = item.IsAnnouncement
                        ? "IMPORTANT ANNOUNCEMENT"
                        : "COMMUNITY EVENT",
                    ForeColor = item.IsAnnouncement
                        ? Color.DarkRed
                        : primaryBlue,
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    Location = new Point(9, 108),
                    Size = new Size(278, 18),
                    Cursor = Cursors.Hand
                };

                // Use Dictionary to retrieve full event details by ID.
                EventHandler showDetails = (sender, e) =>
                {
                    if (eventsById.TryGetValue(
                        item.Id, out MunicipalEvent selectedEvent))
                    {
                        MessageBox.Show(
                            "Event: " + selectedEvent.Title +
                            Environment.NewLine +
                            "Category: " + selectedEvent.Category +
                            Environment.NewLine +
                            "Date: " + selectedEvent.Date.ToString("dd MMMM yyyy") +
                            Environment.NewLine +
                            "Location: " + selectedEvent.Location +
                            Environment.NewLine +
                            "Description: " + selectedEvent.Description,
                            "Event Details",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                };

                card.Click += showDetails;
                title.Click += showDetails;
                details.Click += showDetails;
                priority.Click += showDetails;

                card.Controls.Add(title);
                card.Controls.Add(details);
                card.Controls.Add(priority);

                eventsPanel.Controls.Add(card);
            }

            if (items.Count == 0)
            {
                eventsPanel.Controls.Add(new Label
                {
                    Text = "No upcoming events match your search.",
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = textDark,
                    AutoSize = true,
                    Margin = new Padding(15)
                });
            }

            lblResults.Text = items.Count + " event(s) found.";
            eventsPanel.ResumeLayout();
        }

        // RECOMMENDATIONS BASED ON SEARCH PREFERENCES
        private void ShowRecommendations()
        {
            recommendationsPanel.Controls.Clear();

            string preferredTerm = categorySearchCounts
                .OrderByDescending(pair => pair.Value)
                .Select(pair => pair.Key)
                .FirstOrDefault();

            List<MunicipalEvent> recommended;

            if (!string.IsNullOrWhiteSpace(preferredTerm))
            {
                // Match the user's most frequent category or search keyword.
                recommended = GetChronologicalEvents()
                    .Where(item =>
                        item.Category.IndexOf(preferredTerm,
                            StringComparison.OrdinalIgnoreCase) >= 0
                        || item.Title.IndexOf(preferredTerm,
                            StringComparison.OrdinalIgnoreCase) >= 0
                        || item.Description.IndexOf(preferredTerm,
                            StringComparison.OrdinalIgnoreCase) >= 0)
                    .Take(3)
                    .ToList();
            }
            else
            {
                // Retrieve highest-priority upcoming events.
                PriorityQueue<MunicipalEvent, int> recommendationQueue =
                    new PriorityQueue<MunicipalEvent, int>();

                foreach (MunicipalEvent item in GetChronologicalEvents())
                {
                    recommendationQueue.Enqueue(item, item.Priority);
                }

                recommended = new List<MunicipalEvent>();

                while (recommendationQueue.Count > 0 &&
                       recommended.Count < 3)
                {
                    recommended.Add(recommendationQueue.Dequeue());
                }
            }

            if (recommended.Count == 0)
            {
                // Fallback: show upcoming events if no preference matches.
                recommended = GetChronologicalEvents().Take(3).ToList();
            }

            if (recommended.Count == 0)
            {
                recommendationsPanel.Controls.Add(new Label
                {
                    Text = "No recommendations are available.",
                    AutoSize = true,
                    ForeColor = textDark,
                    Margin = new Padding(5, 10, 5, 5)
                });
            }

            foreach (MunicipalEvent item in recommended)
            {
                Button card = MakeButton(
                    item.Title + Environment.NewLine +
                    item.Category + " | " +
                    item.Date.ToString("dd MMM"),
                    lightBlue,
                    darkBlue);

                card.Size = new Size(230, 60);
                card.Margin = new Padding(4);

                card.Click += (s, e) =>
                {
                    txtSearch.Text = item.Title;
                    SearchEvents();
                };

                recommendationsPanel.Controls.Add(card);
            }
        }

        private void UpdateSearchHistoryLabel()
        {
            lblHistory.Text = searchHistory.Count > 0
                ? "Recent search: " + searchHistory.Peek()
                : "Search by keyword, category or date.";
        }
    }

    public class MunicipalEvent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsAnnouncement { get; set; }

        public MunicipalEvent(
            string id,
            string title,
            string category,
            DateTime date,
            string location,
            string description,
            int priority,
            bool isAnnouncement = false)
        {
            Id = id;
            Title = title;
            Category = category;
            Date = date;
            Location = location;
            Description = description;
            Priority = priority;
            IsAnnouncement = isAnnouncement;
        }
    }
}
