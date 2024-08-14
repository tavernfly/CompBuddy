using System.Text.Json;

namespace CompBuddy
{
    public static class Data
    {
        public static CompType[] CompTypes = new CompType[] {
            // Name, Number, HasCounter, HasTimer
            new CompType("mark", 0, true, false),
            new CompType("reward", 1, true, true),
            new CompType("transaction", 2, true, true),
            new CompType("event", 3, true, true),
            new CompType("ward", 4, true, true),
            new CompType("staff_work", 5, true, true),
            new CompType("planning", 6, false, true)
        };
        public static Guild[] Guilds = new Guild[] {
            // Name, Hotkey
            new Guild("slayer", "s", 0, Colors.Slayers),
            new Guild("merchant", "m", 1, Colors.Merchants),
            new Guild("builder", "b", 2, Colors.Builders),
            new Guild("ranger", "r", 3, Colors.Rangers),
            new Guild("explorer", "e", 4, Colors.Explorers),
            new Guild("hobbit", "h", 5, Colors.Hobbits),
            new Guild("mage", "w", 6, Colors.Mages)
        };

        public static bool Unsaved;
        public static bool Empty;
        private static string log = "log.txt";
        private static int[][][] data; // [Guild, Mode, Count/Time]
        public static int GuildNumber { get; set; }
        public static int ModeNumber { get; set; }

        static Data()
        {
            // Initialize the data structure
            Load();
        }

        public static int GetCount(int guild, int mode) => data[guild][mode][0];
        public static int GetTime(int guild, int mode) => data[guild][mode][1];
        public static void SetCount(int guild, int mode, int value)
        {
            data[guild][mode][0] = value;
            Unsaved = true;
            Empty = false;
        }
        public static void SetTime(int guild, int mode, int value)
        {
            data[guild][mode][1] = value;
            Unsaved = true;
            Empty = false;
        }

        // Serialize the data, selectedGroup, and selectedSubgroup to JSON
        public static string Serialize()
        {
            var state = new
            {
                Data = data,
                Guild = GuildNumber,
                Mode = ModeNumber
            };

            return JsonSerializer.Serialize(state);
        }

        // Deserialize the JSON string into data, selectedGroup, and selectedSubgroup
        public static void Deserialize(string json)
        {
            var state = JsonSerializer.Deserialize<SerializedState>(json);
            if (state != null)
            {
                data = state.Data;
                GuildNumber = state.Guild;
                ModeNumber = state.Mode;
            }
        }

        // Save the serialized data to a file
        public static void Save()
        {
            if (Empty) return;
            string json = Serialize();
            File.WriteAllText(log, json);
            Unsaved = false;
        }

        // Load data from a file and deserialize it
        public static void Load()
        {
            if (File.Exists(log))
            {
                string json = File.ReadAllText(log);
                Deserialize(json);
            }
            else
                Reset();
        }

        public static void Reset()
        {
            data = new int[7][][];
            for (int i = 0; i < 7; i++)
            {
                data[i] = new int[7][];
                for (int j = 0; j < 7; j++)
                {
                    data[i][j] = new int[2];
                }
            }
            Empty = true;
        }

        // Helper class for deserialization
        private class SerializedState
        {
            public int[][][] Data { get; set; }
            public int Guild { get; set; }
            public int Mode { get; set; }
        }
    }
}