namespace CompBuddy
{
    public class Guild
    {
        public string Name { get; set; }
        public string Hotkey { get; set; }
        public int Number { get; set; }
        public Color Color { get; set; }
        public Mode[] Modes { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }

        public Guild(string name, string hotkey, int number, Color color)
        {
            Name = name;
            Title = char.ToUpper(name[0]) + name.Substring(1);
            Header = name.ToUpper() + ((name.EndsWith("s")) ? "" : "S");
            Hotkey = hotkey;
            Number = number;
            Color = color;
            Modes = new Mode[Data.CompTypes.Length];
            for (int i = 0; i < Data.CompTypes.Length; i++) {
                Modes[i] = new Mode(Data.CompTypes[i], number);
            }
        }
    }
}