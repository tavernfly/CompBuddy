namespace CompBuddy
{
    public class CompType
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public bool HasCounter { get; set; }
        public bool HasTimer { get; set; }
        public string Title { get; set; }
        public string Plural { get; set; }
        public string Gerund { get; set; }

        public CompType(string name, int number, bool hasCounter, bool hasTimer)
        {
            Name = name;
            Number = number;
            HasCounter = hasCounter;
            HasTimer = hasTimer;
            Title = char.ToUpper(name[0]) + name.Substring(1);
            Plural = Title + "s";
            Gerund = name + ((name.EndsWith("ing")) ? "" : "ing");
        }
    }
}