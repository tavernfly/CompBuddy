namespace CompBuddy
{
    public class MenuOption
    {
        private Menu menu;
        private Label label;
        public string Text { get; set; }
        public string HeaderText { get; set; }
        public int Number { get; set; }
        public Color HoverColor { get; set; }

        public MenuOption(int i, Menu parent)
        {
            Number = i;
            menu = parent;
            HoverColor = Color.Gray;
            label = new Label
            {
                Name = $"{menu.Name}MenuOption{i}",
                Text = Text,
                AutoSize = false,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = menu.labelHeight,
                ForeColor = Colors.Text,
                Cursor = Cursors.Hand
            };
            menu.MenuPanel.Controls.Add(label);
            label.MouseEnter += HoverOn;
            label.MouseLeave += HoverOff;
            label.Click += Clicked;
            Deselect();
        }

        public void Initialize(string text, string headerText, Color color)
        {
            Text = text;
            label.Text = text;
            HeaderText = headerText;
            if (color == Color.Empty)
                HoverColor = Colors.Light;
            else
                HoverColor = color;
            if (menu.Center) 
                label.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void HoverOn(object sender = null, EventArgs e = null)
        {
            if (menu.Selected != Number)
                label.ForeColor = HoverColor;
        }

        private void HoverOff(object sender = null, EventArgs e = null)
        {
            if (menu.Selected != Number)
                label.ForeColor = Colors.Text;
        }

        private void Clicked(object sender = null, EventArgs e = null)
        {
            if (menu.Selected != Number)
            {
                menu.Select(Number);
                menu.Update();
            }
        }

        public void Select()
        {
            label.ForeColor = Colors.Disabled;
            label.Cursor = Cursors.Default;
        }

        public void Deselect()
        {
            label.ForeColor = Colors.Text;
            label.Cursor = Cursors.Hand;
        }
    }
}