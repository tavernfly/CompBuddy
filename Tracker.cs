namespace CompBuddy
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Reflection;

    namespace CompBuddy
    {

        public partial class Tracker : Form
        {
            [DllImport("user32.dll")]
            private static extern int RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

            [DllImport("user32.dll")]
            private static extern int UnregisterHotKey(IntPtr hWnd, int id);

            public static float scaleX;
            public static float scaleY;

            private Guild[] guilds => Data.Guilds;
            private Guild guild => guilds[Data.GuildNumber];
            private Mode[] modes => guild.Modes;
            private Mode mode => modes[Data.ModeNumber];

            private Label countLabel;
            private Label timeLabel;
            private Label postedLabel;

            private Panel mainPanel;

            private TableLayoutPanel menuContainer;
            private Menu guildMenu;
            private Menu modeMenu;

            private TableLayoutPanel reloader;
            private Label reloaderLabel;
            private TableLayoutPanel reloaderButtons;
            private Button reloaderYes;
            private Button reloaderNo;

            private Label gear;
            private TableLayoutPanel settings;
            private Label settingsLabel;
            private TableLayoutPanel settingsButtons;
            private Button settingsDefault;
            private Button settingsBack;

            private Label question;
            private TableLayoutPanel help;
            private Label helpLabel;
            private TableLayoutPanel helpButtons;
            private Button helpNext;
            private Button helpOk;
            private int helpPage;

            private TableLayoutPanel dialog;
            private Label dialogLabel;
            private TableLayoutPanel dialogButtons;
            private Button dialogYes;
            private Button dialogNo;
            private int dialogNumber;

            private Timer timer;
            private Timer autosaveTimer;

            private bool inputLocked = true;

            private bool settingsActive;

            private int hotkey;

            public Tracker()
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "CompBuddy.compbuddy.ico";

                using (var iconStream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (iconStream != null)
                    {
                        this.Icon = new Icon(iconStream);
                    }
                }

                this.AutoScaleMode = AutoScaleMode.Dpi;
                InitializeComponent();
                InitializeMenus();
                InitializeReloader();
                InitializeSettings();
                InitializeHelp();
                InitializeDialog();
                InitializeDisplays();
                
                InitializeTimers();
                TimerStop();
                UpdateCount();
                UpdateTime();
                RegisterHotkeys();
                this.TopLevel = true;
                this.TopMost = true;
            }

            private void InitializeComponent()
            {
                this.SuspendLayout();

                using (Graphics g = this.CreateGraphics())
                {
                    float dpiX = g.DpiX;
                    float dpiY = g.DpiY;

                    scaleX = dpiX / 96f;
                    scaleY = dpiY / 96f;
                }

                this.ClientSize = new System.Drawing.Size((int)(180 * scaleX), (int)(100 * scaleY));
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
                this.Name = "CompBuddy";
                this.Text = "CompBuddy";
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 10, 10);
                this.WindowState = FormWindowState.Normal;
                this.BackColor = Colors.Background;
                this.TopLevel = true;
                this.TopMost = true;
                this.ResumeLayout(false);
                mainPanel = new Panel { Dock = DockStyle.Fill };
                this.Controls.Add(mainPanel);
            }

            private void InitializeReloader()
            {
                reloader = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoSize = false,
                    RowCount = 2,
                    ColumnCount = 1,
                    Visible = false
                };
                reloader.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                reloader.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                reloader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

                reloaderLabel = new Label
                {
                    Text = "Load previous session?",
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                reloaderButtons = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoSize = false,
                    RowCount = 1,
                    ColumnCount = 2
                };
                reloaderButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                reloaderButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                reloaderButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                reloaderYes = new Button { Text = "Yes", Anchor = AnchorStyles.None, AutoSize = true };
                reloaderNo = new Button { Text = "No", Anchor = AnchorStyles.None, AutoSize = true };
                reloaderButtons.Controls.Add(reloaderYes);
                reloaderButtons.Controls.Add(reloaderNo);
                reloader.Controls.Add(reloaderLabel);
                reloader.Controls.Add(reloaderButtons);
                this.Controls.Add(reloader);
                reloader.BringToFront();
                reloaderYes.Click += ReloaderYes;
                reloaderNo.Click += ReloaderNo;
                if (!Data.Empty)
                    reloader.Visible = true;
                else
                    inputLocked = false;
            }

            private void InitializeSettings()
            {
                gear = new Label
                {
                    Name = "openSettings",
                    Text = "⚙",
                    AutoSize = true,
                    Location = new Point(this.Width - (int)(40 * scaleX), this.Height - (int)(60 * scaleY)),
                    Cursor = Cursors.Hand,
                    ForeColor = Colors.Medium
                };
                gear.Font = new Font("Arial", 10);
                mainPanel.Controls.Add(gear);
                settings = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 2,
                    ColumnCount = 1,
                    Visible = false
                };
                settings.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
                settings.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
                settings.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                settingsLabel = new Label
                {
                    Text = "",
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                settingsButtons = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 1,
                    ColumnCount = 2
                };
                settingsButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                settingsButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                settingsButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                settingsDefault = new Button { Text = "Default", Anchor = AnchorStyles.None, AutoSize = true };
                settingsBack = new Button { Text = "Back", Anchor = AnchorStyles.None, AutoSize = true };
                settingsButtons.Controls.Add(settingsDefault);
                settingsButtons.Controls.Add(settingsBack);
                settings.Controls.Add(settingsLabel);
                settings.Controls.Add(settingsButtons);
                this.Controls.Add(settings);
                settings.BringToFront();
                gear.Click += OpenSettings;
                settingsBack.Click += CloseSettings;
                settingsDefault.Click += SettingsDefault;
                gear.MouseEnter += SettingsHoverOn;
                gear.MouseLeave += SettingsHoverOff;
            }

            private void InitializeHelp()
            {
                question = new Label
                {
                    Name = "openHelp",
                    Text = "?",
                    AutoSize = true,
                    Location = new Point(this.Width - (int)(53 * scaleX), this.Height - (int)(63 * scaleY)),
                    Cursor = Cursors.Hand,
                    ForeColor = Colors.Light
                };
                question.Font = new Font(question.Font.FontFamily, 11, FontStyle.Bold);
                mainPanel.Controls.Add(question);
                help = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 2,
                    ColumnCount = 1,
                    Visible = false
                };
                help.RowStyles.Add(new RowStyle(SizeType.Percent, 65F));
                help.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
                help.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                helpLabel = new Label
                {
                    Text = "",
                    //TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                helpButtons = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 1,
                    ColumnCount = 2
                };
                helpButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                helpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                helpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                helpNext = new Button { Text = "Next", Anchor = AnchorStyles.None, AutoSize = true };
                helpOk = new Button { Text = "Ok", Anchor = AnchorStyles.None, AutoSize = true };
                helpButtons.Controls.Add(helpNext);
                helpButtons.Controls.Add(helpOk);
                help.Controls.Add(helpLabel);
                help.Controls.Add(helpButtons);
                this.Controls.Add(help);
                help.BringToFront();
                question.Click += OpenHelp;
                helpOk.Click += CloseHelp;
                helpNext.Click += HelpNext;
                question.MouseEnter += HelpHoverOn;
                question.MouseLeave += HelpHoverOff;
            }

            private void InitializeDialog()
            {
                dialog = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 2,
                    ColumnCount = 1,
                    Visible = false
                };
                dialog.RowStyles.Add(new RowStyle(SizeType.Percent, 65F));
                dialog.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
                dialog.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                dialogLabel = new Label
                {
                    Text = "",
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                dialogButtons = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 1,
                    ColumnCount = 2
                };
                dialogButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                dialogButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                dialogButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                dialogYes = new Button { Text = "Yes", Anchor = AnchorStyles.None, AutoSize = true };
                dialogNo = new Button { Text = "No", Anchor = AnchorStyles.None, AutoSize = true };
                dialogButtons.Controls.Add(dialogYes);
                dialogButtons.Controls.Add(dialogNo);
                dialog.Controls.Add(dialogLabel);
                dialog.Controls.Add(dialogButtons);
                this.Controls.Add(dialog);
                dialog.BringToFront();
                dialogYes.Click += DialogYes;
                dialogNo.Click += DialogNo;
            }

            private void InitializeDisplays()
            {
                countLabel = new Label
                {
                    Name = "countLabel",
                    Text = "",
                    AutoSize = true,
                    Location = new Point((int)(10 * scaleX), (int)(37 * scaleY))
                };
                mainPanel.Controls.Add(countLabel);

                timeLabel = new Label
                {
                    Name = "timeLabel",
                    Text = "",
                    AutoSize = true,
                    Location = new Point((int)(10 * scaleX), (int)(57 * scaleY))
                };
                mainPanel.Controls.Add(timeLabel);

                postedLabel = new Label
                {
                    Name = "postedLabel",
                    Text = "",
                    AutoSize = true,
                    Location = new Point((int)(10 * scaleX), (int)(77 * scaleY))
                };
                mainPanel.Controls.Add(postedLabel);
            }

            private void InitializeMenus()
            {
                menuContainer = new TableLayoutPanel
                {
                    Dock = DockStyle.Top,
                    AutoSize = false,
                    Height = (int)(22 * scaleY),
                    ColumnCount = 2,
                    RowCount = 1
                };
                menuContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                menuContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                mainPanel.Controls.Add(menuContainer);
                menuContainer.BringToFront();

                guildMenu = new Menu
                {
                    Name = "Guild",
                    Size = Data.Guilds.Length,
                    Parent = this,
                    Container = menuContainer,
                    Center = true,
                    Colorful = true,
                    Bold = true
                };
                for (int i = 0; i < Data.Guilds.Length; i++)
                    guildMenu.AddOption(i, Data.Guilds[i].Header, "", Data.Guilds[i].Color);
                guildMenu.Select(Data.GuildNumber);
                menuContainer.Controls.Add(guildMenu.MenuPanel, 0, 0);

                modeMenu = new Menu
                {
                    Name = "Mode",
                    Size = Data.CompTypes.Length,
                    Parent = this,
                    Container = menuContainer,
                    Italic = true
                };
                for (int i = 0; i < Data.CompTypes.Length; i++)
                    modeMenu.AddOption(i, Data.CompTypes[i].Name, Data.CompTypes[i].Gerund);
                modeMenu.Select(Data.ModeNumber);
                menuContainer.Controls.Add(modeMenu.MenuPanel, 1, 0);
            }

            private void OpenSettings(object sender, EventArgs e)
            {
                inputLocked = true;
                TimerStop();
                UnregisterHotkeys();
                settingsActive = true;
                settingsLabel.Text = $"Counter Hotkey: {Hotkeys.IncrementKeyName}\r\nPress any key now to change the hotkey";
                settings.Visible = true;
            }

            private void CloseSettings(object sender, EventArgs e)
            {
                settingsActive = false;
                settings.Visible = false;
                RegisterHotkeys();
                inputLocked = false;
            }

            private void SettingsDefault(object sender, EventArgs e)
            {
                SettingsChangeHotkey(Hotkeys.CounterKeyDefault, Hotkeys.CounterModDefault);
            }

            private void SettingsChangeHotkey(int counterKey, int counterMod)
            {
                Hotkeys.SetCounterKey(counterKey, counterMod);
                settingsLabel.Text = $"Counter Hotkey: {Hotkeys.IncrementKeyName}\r\nPress any key now to change the hotkey";
            }

            private void SettingsHoverOn(object sender, EventArgs e)
            {
                gear.ForeColor = Colors.Light;
            }
            private void SettingsHoverOff(object sender, EventArgs e)
            {
                gear.ForeColor = Colors.Medium;
            }

            private void OpenHelp(object sender, EventArgs e)
            {
                inputLocked = true;
                TimerStop();
                helpPage = 0;
                DisplayHelpText();
                help.Visible = true;
            }
            private void CloseHelp(object sender, EventArgs e)
            {
                help.Visible = false;
                inputLocked = false;
            }
            private void DisplayHelpText()
            {
                string helpText = "";
                if (helpPage == 0)
                {
                    helpText = $"Increment Counter: {Hotkeys.IncrementKeyName}";
                    helpText = $"{helpText}\r\nDecrement Counter: {Hotkeys.DecrementKeyName}";
                    helpText = $"{helpText}\r\nTimer Start/Stop: {Hotkeys.TimerKeyName}";
                    helpText = $"{helpText}\r\nPrint Comp: {Hotkeys.PrintKeyName}";
                }
                else if (helpPage == 1)
                {
                    helpText = "Change Comp Type: Ctrl+1-7";
                    helpText = $"{helpText}\r\nReset Timer: {Hotkeys.ResetTimerKeyName}";
                    helpText = $"{helpText}\r\nReset EVERYTHING: {Hotkeys.ResetAllKeyName}";
                    helpText = $"{helpText}\r\nHide CompBuddy: {Hotkeys.HideKeyName}";
                }
                else
                    helpText = "CompBuddy 2.0\r\nby Vern\r\nNow you have to post your\r\ncomp, Karum!";
                helpLabel.Text = helpText;
            }
            private void HelpNext(object sender, EventArgs e)
            {
                if (helpPage == 2)
                    helpPage = 0;
                else
                    helpPage++;
                DisplayHelpText();
            }

            private void HelpHoverOn(object sender, EventArgs e)
            {
                question.ForeColor = Colors.Disabled;
            }
            private void HelpHoverOff(object sender, EventArgs e)
            {
                question.ForeColor = Colors.Light;
            }
            private void ReloaderYes(object sender, EventArgs e)
            {
                reloader.Visible = false;
                inputLocked = false;
            }
            private void ReloaderNo(object sender, EventArgs e)
            {
                Data.Reset();
                Update();
                reloader.Visible = false;
                inputLocked = false;
            }

            private void DialogOpen()
            {
                inputLocked = true;
                TimerStop();
                dialog.Visible = true;
                switch (dialogNumber)
                {
                    case 1:
                        dialogLabel.Text = "Are you sure you want to reset the timer?";
                        break;
                    case 2:
                        dialogLabel.Text = "Are you sure you want to reset EVERYTHING?";
                        break;
                }
            }

            private void DialogClose()
            {
                dialog.Visible = false;
                inputLocked = false;
            }
            private void DialogNo(object sender, EventArgs e)
            {
                DialogClose();
            }

            private void DialogYes(object sender, EventArgs e)
            {
                switch (dialogNumber)
                {
                    case 1:
                        TimerReset();
                        DialogClose();
                        break;
                    case 2:
                        Data.Reset();
                        Update();
                        DialogClose();
                        break;
                }

            }

            public void Update()
            {
                Data.GuildNumber = guildMenu.Selected;
                Data.ModeNumber = modeMenu.Selected;
                TimerStop();
                UpdateCount();
                UpdateTime();
                UpdatePosted();
            }

            public void UpdateCount()
            {
                if (mode.HasCounter)
                    countLabel.Text = $"{mode.Plural}: {mode.Count}";
                else
                    countLabel.Text = "";
            }

            public void UpdateTime()
            {
                if (mode.HasTimer)
                    timeLabel.Text = $"Duration: {mode.DisplayTime()}";
                else
                    timeLabel.Text = "";
            }

            public void UpdatePosted()
            {
                if (mode.Posted > 0)
                    postedLabel.Text = $"Posted {mode.PostedText}";
                else
                    postedLabel.Text = "";
            }

            public void ClearPosted()
            {
                mode.Posted = 0;
                mode.PostedText = "";
                postedLabel.Text = "";
            }

            public void TimerStart()
            {
                if (mode.HasTimer)
                {
                    timer.Start();
                    this.Text = "(TIMER RUNNING)";
                    ClearPosted();
                }
            }
            public void TimerStop()
            {
                timer.Stop();
                if (mode.HasTimer)
                    this.Text = "CompBuddy (PAUSED)";
                else
                    this.Text = "CompBuddy";
            }

            public void TimerToggle()
            {
                if (timer.Enabled)
                    TimerStop();
                else
                    TimerStart();
            }

            public void TimerReset()
            {
                if (timer.Enabled)
                    TimerStop();
                mode.ResetTime();
                UpdateTime();
            }

            private void InitializeTimers()
            {
                timer = new Timer();
                timer.Interval = 1000;
                timer.Tick += Timer_Tick;
                autosaveTimer = new Timer();
                autosaveTimer.Interval = 60000;
                autosaveTimer.Tick += Autosave;
                autosaveTimer.Start();
            }
            private void Timer_Tick(object sender, EventArgs e)
            {
                mode.IncrementTime();
                UpdateTime();
            }
            private void Autosave(object sender, EventArgs e)
            {
                if (Data.Unsaved)
                    Data.Save();
            }
            private void Print()
            {
                TimerStop();
                string date = DateTime.Now.ToString("yyyy/MM/dd");
                string notes = $"{guild.Title} {(mode.Count > 1 ? mode.Plural : mode.Title)} {date}";
                if (mode.Posted == mode.Count)
                {
                    mode.Posted = 0;
                    mode.PostedText = "";
                }
                int count = Math.Min((mode.Count - mode.Posted), 99);
                int time;
                if (mode.Count > 99)
                {
                    mode.Posted += count;
                    double post = mode.Posted / 99.0;
                    double posts = mode.Count / 99.0;
                    time = (post > 1) ? 0 : mode.Minutes;
                    mode.PostedText = $"{Math.Ceiling(post)} of {Math.Ceiling(posts)}";
                    notes = $"{notes} ({mode.PostedText})";
                }
                else
                {
                    time = mode.Minutes;
                    mode.Posted = count;
                }
                string comp = $"/comp guild:{guild.Name} comp_type: {mode.Name} count: {count} duration: {time} notes: {notes}";
                string cb = Clipboard.GetText();
                Clipboard.SetText(comp);
                SendKeys.SendWait("^v");
                if (!string.IsNullOrEmpty(cb)) Clipboard.SetText(cb);
                UpdatePosted();
            }

            private void HideToggle()
            {
                if (this.WindowState == FormWindowState.Minimized)
                    this.WindowState = FormWindowState.Normal;
                else
                    this.WindowState = FormWindowState.Minimized;
            }

            private const int IncrementCount = 1;
            private const int DecrementCount = 2;
            private const int TimerOnOff = 3;
            private const int PrintComp = 4;
            private const int ResetTimer = 5;
            private const int ResetEverything = 6;
            private const int HideWindow = 7;

            private void RegisterHotkeys()
            {
                RegisterHotKey(this.Handle, IncrementCount, Hotkeys.IncrementModifier, Hotkeys.CounterKey);
                RegisterHotKey(this.Handle, DecrementCount, Hotkeys.DecrementModifier, Hotkeys.CounterKey);
                RegisterHotKey(this.Handle, TimerOnOff, Hotkeys.TimerModifier, Hotkeys.TimerKey);
                RegisterHotKey(this.Handle, PrintComp, Hotkeys.PrintModifier, Hotkeys.PrintKey);
                RegisterHotKey(this.Handle, ResetTimer, Hotkeys.ResetTimerModifier, Hotkeys.ResetTimerKey);
                RegisterHotKey(this.Handle, ResetEverything, Hotkeys.ResetAllModifier, Hotkeys.ResetAllKey);
                RegisterHotKey(this.Handle, HideWindow, Hotkeys.HideModifier, Hotkeys.HideKey);
                // Comp Type Hotkeys 1-7
                RegisterHotKey(this.Handle, 10, Hotkeys.ModCtrl, Hotkeys.OneKey);
                RegisterHotKey(this.Handle, 11, Hotkeys.ModCtrl, Hotkeys.TwoKey);
                RegisterHotKey(this.Handle, 12, Hotkeys.ModCtrl, Hotkeys.ThreeKey);
                RegisterHotKey(this.Handle, 13, Hotkeys.ModCtrl, Hotkeys.FourKey);
                RegisterHotKey(this.Handle, 14, Hotkeys.ModCtrl, Hotkeys.FiveKey);
                RegisterHotKey(this.Handle, 15, Hotkeys.ModCtrl, Hotkeys.SixKey);
                RegisterHotKey(this.Handle, 16, Hotkeys.ModCtrl, Hotkeys.SevenKey);
                // Comp Type Hotkeys Numpad 1-7
                RegisterHotKey(this.Handle, 20, Hotkeys.ModCtrl, Hotkeys.NumOneKey);
                RegisterHotKey(this.Handle, 21, Hotkeys.ModCtrl, Hotkeys.NumTwoKey);
                RegisterHotKey(this.Handle, 22, Hotkeys.ModCtrl, Hotkeys.NumThreeKey);
                RegisterHotKey(this.Handle, 23, Hotkeys.ModCtrl, Hotkeys.NumFourKey);
                RegisterHotKey(this.Handle, 24, Hotkeys.ModCtrl, Hotkeys.NumFiveKey);
                RegisterHotKey(this.Handle, 25, Hotkeys.ModCtrl, Hotkeys.NumSixKey);
                RegisterHotKey(this.Handle, 26, Hotkeys.ModCtrl, Hotkeys.NumSevenKey);
            }

            private void UnregisterHotkeys()
            {
                UnregisterHotKey(this.Handle, IncrementCount);
                UnregisterHotKey(this.Handle, DecrementCount);
                UnregisterHotKey(this.Handle, TimerOnOff);
                UnregisterHotKey(this.Handle, PrintComp);
                UnregisterHotKey(this.Handle, ResetTimer);
                UnregisterHotKey(this.Handle, ResetEverything);
                UnregisterHotKey(this.Handle, HideWindow);
                UnregisterHotKey(this.Handle, 10);
                UnregisterHotKey(this.Handle, 11);
                UnregisterHotKey(this.Handle, 12);
                UnregisterHotKey(this.Handle, 13);
                UnregisterHotKey(this.Handle, 14);
                UnregisterHotKey(this.Handle, 15);
                UnregisterHotKey(this.Handle, 16);
                UnregisterHotKey(this.Handle, 20);
                UnregisterHotKey(this.Handle, 21);
                UnregisterHotKey(this.Handle, 22);
                UnregisterHotKey(this.Handle, 23);
                UnregisterHotKey(this.Handle, 24);
                UnregisterHotKey(this.Handle, 25);
                UnregisterHotKey(this.Handle, 26);
            }

            protected override void WndProc(ref Message m)
            {
                const int WM_HOTKEY = 0x0312;
                if (m.Msg == WM_HOTKEY)
                {
                    if (!inputLocked) {
                        hotkey = (int)m.WParam;
                        switch (hotkey)
                        {
                            case IncrementCount:
                                mode.IncrementCount();
                                ClearPosted();
                                UpdateCount();
                                if (!timer.Enabled)
                                    TimerStart();
                                break;
                            case DecrementCount:
                                mode.DecrementCount();
                                ClearPosted();
                                UpdateCount();
                                break;
                            case TimerOnOff:
                                TimerToggle();
                                break;
                            case PrintComp:
                                Print();
                                break;
                            case ResetTimer:
                                dialogNumber = 1;
                                DialogOpen();
                                break;
                            case ResetEverything:
                                dialogNumber = 2;
                                DialogOpen();
                                break;
                            case HideWindow:
                                HideToggle();
                                break;
                        }
                        if (hotkey > 9)
                        {
                            if (hotkey < 17)
                            {
                                modeMenu.Select(hotkey - 10);
                                Update();
                            }
                            else if ((hotkey > 19) && (hotkey < 27))
                            {
                                modeMenu.Select(hotkey - 20);
                                Update();
                            }
                        }
                    }
                }
                base.WndProc(ref m);
            }

            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
            {
                if (settingsActive)
                {
                    bool ctrlPressed = (ModifierKeys & Keys.Control) == Keys.Control;
                    keyData = keyData & Keys.KeyCode;
                    int virtualKeyCode = (int)keyData;
                    if (Hotkeys.AllowedKeys.TryGetValue(virtualKeyCode, out string keyName))
                    {
                        SettingsChangeHotkey(virtualKeyCode, ctrlPressed ? Hotkeys.ModCtrl : Hotkeys.ModNone);
                        return true;
                    }
                    else if (keyData != Keys.ControlKey)
                    {
                        keyName = keyData.ToString();
                        settingsLabel.Text = $"{keyName} is not allowed.\r\nPress any key now to change the hotkey";
                    }
                }
                // Call base class method
                return base.ProcessCmdKey(ref msg, keyData);
            }

            protected override void OnFormClosing(FormClosingEventArgs e)
            {
                UnregisterHotkeys();
                if (Data.Unsaved) 
                    Data.Save();
                base.OnFormClosing(e);
            }

            //private void Tracker_KeyDown(object sender, KeyEventArgs e)
            //{
            //    if (e.KeyCode == Keys.Oemplus) // `=` key on US keyboard layout
            //    {
            //        mode.Count++;
            //        countLabel.Text = $"{mode.Plural}: {mode.Count}";
            //    }
            //}
        }
    }
}