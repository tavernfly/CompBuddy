namespace CompBuddy
{
    public class Mode
    {
        private CompType compType;
        private int guildNumber;

        public int Number
        {
            get { return this.compType.Number; }
        }
        public int Count
        {
            get { return Data.GetCount(guildNumber, this.Number); }
            set { Data.SetCount(guildNumber, this.Number, value); }
        }
        public int Posted { get; set; }
        public string PostedText = "";
        public int Time
        {
            get { return Data.GetTime(guildNumber, this.Number); }
            set { Data.SetTime(guildNumber, this.Number, value); }
        }
        public int Minutes
        {
            get { return Math.Max((int)Math.Ceiling(Time / 60.0), 1); }
        }

        public bool HasTimer => this.compType.HasTimer;
        public bool HasCounter => this.compType.HasCounter;
        public string Name => this.compType.Name;
        public string Title => this.compType.Title;
        public string Plural => this.compType.Plural;

        public Mode(CompType comp_type, int guild_number)
        {
            compType = comp_type;
            guildNumber = guild_number;
        }

        public void IncrementCount()
        {
            if (HasCounter) Count = Count + 1;
        }
        public void DecrementCount()
        {
            if (HasCounter && (Count > 0)) Count = Count - 1;
        }
        public void IncrementTime()
        {
            if (Time < 30000) Time = Time + 1;
        }
        public void ResetTime()
        {
            Time = 0;
        }

        public string DisplayTime()
        {
            if (Time == 0) return "0:00";
            int min = Time / 60;
            int sec = Time % 60;
            return $"{min}:{sec:D2}";
        }
    }
}