using CompBuddy.CompBuddy;
using Timer = System.Windows.Forms.Timer;

namespace CompBuddy
{
    public class Menu
    {

        public Panel MenuPanel;
        private MenuOption[] options;
        private Label header;
        private bool open;
        public int labelHeight = (int)(20 * Tracker.scaleY);
        public int panelHeight;
        public int parentHeight; // = (int)(139 * Tracker.scaleY);
        public int padding = (int)(2 * Tracker.scaleY);
        public int hoffset = (int)(46 * Tracker.scaleY);

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                MenuPanel.Name = $"{_name}Menu";
                header.Name = $"{_name}MenuHeader";
            }
        }

        private int _size;
        public int Size { 
            get => _size;
            set
            {
                _size = value;
                options = new MenuOption[_size];
                for (int i = _size - 1; i >= 0; i--)
                {
                    options[i] = new MenuOption(i, this);
                }
                MenuPanel.Controls.Add(header);
                panelHeight = (_size + 1) * labelHeight;
            }
        }

        private Tracker _parent;
        public Tracker Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                parentHeight = _parent.Height;
                //_parent.Controls.Add(MenuPanel);
                //MenuPanel.BringToFront();
            }
        }
        public TableLayoutPanel Container { get; set; }

        private bool _center;
        public bool Center
        {
            get => _center;
            set
            {
                _center = value;
                if (_center)
                    header.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        private bool _bold;
        public bool Bold
        {
            get => _bold;
            set
            {
                _bold = value;
                if (_bold)
                    header.Font = new Font(header.Font, FontStyle.Bold);
            }
        }

        private bool _italic;
        public bool Italic
        {
            get => _italic;
            set
            {
                _italic = value;
                if (_italic)
                    header.Font = new Font(header.Font, FontStyle.Italic);
            }
        }

        public bool Colorful { get; set; }

        public int Selected { get; set; }

        private Timer timer;
        private DateTime startTime;
        private int startHeight;
        private int targetHeight;
        private int duration = 200;

        public Menu()
        {
            MenuPanel = new Panel
            {
                AutoSize = false,
                Height = labelHeight,
                Dock = DockStyle.Top
            };
            header = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = labelHeight,
                Cursor = Cursors.Hand
            };
            MenuPanel.Controls.Add(header);
            header.MouseEnter += HoverOn;
            header.MouseLeave += HoverOff;
            header.Click += ToggleOpen;
            timer = new Timer
            {
                Interval = 15
            };
            timer.Tick += Timer_Tick;
        }

        public void AddOption(int i, string text, string headerText = "", Color color = default)
        {
            options[i].Initialize(text, headerText, color);
        }

        public void Select(int i)
        {
            options[this.Selected].Deselect();
            this.Selected = i;
            options[i].Select();
            header.Text = (options[i].HeaderText == "") ? options[i].Text : options[i].HeaderText;
            HoverOff();
            Collapse();
        }

        public void Update()
        {
            Parent.Update();
        }

        private void HoverOn(object sender = null, EventArgs e = null)
        {
            header.ForeColor = Colorful ? Colors.Light : Colors.Light;
        }

        private void HoverOff(object sender = null, EventArgs e = null)
        {
            header.ForeColor = Colorful ? options[Selected].HoverColor : Colors.Text;
        }

        private void Expand()
        {
            if (!open)
                ToggleOpen();
        }

        private void Collapse()
        {
            if (open)
                ToggleOpen();
        }

        private void ToggleOpen(object sender = null, EventArgs e = null)
        {
            open = !open;
            startHeight = MenuPanel.Height;
            targetHeight = open ? panelHeight : labelHeight;
            startTime = DateTime.Now;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            float elapsed = (float)(DateTime.Now - startTime).TotalMilliseconds / duration;
            elapsed = Math.Clamp(elapsed, 0f, 1f);

            if (elapsed >= 1f)
            {
                MenuPanel.Height = targetHeight;
                timer.Stop();
            }
            else
            {
                float easedValue = EaseInOut(elapsed);
                int currentHeight = (int)(startHeight + (targetHeight - startHeight) * easedValue);
                MenuPanel.Height = currentHeight;
            }
            Container.Height = Math.Max(Container.Controls[0].Height, Container.Controls[1].Height)+padding;
            Parent.Height = Math.Max((Container.Height + hoffset), parentHeight);
        }

        private float EaseInOut(float t)
        {
            return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }

    }
}