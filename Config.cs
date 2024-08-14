namespace CompBuddy
{
    public static class Config
    {
        public static bool NightMode { get; set; }

        private static readonly string IniFile = "config.ini";

        static Config()
        {
            NightMode = (GetPreference("NightMode") == 0) ? false : true;
            int hotkey = GetHotkey("CounterKey");
            if (hotkey != null && hotkey != 0)
                Hotkeys.CounterKey = hotkey;
            hotkey = GetHotkey("IncrementModifier");
            if (hotkey != null && hotkey != 0)
                Hotkeys.IncrementModifier = hotkey;
        }

        // Read a value from the INI file
        public static string ReadValue(string section, string key)
        {
            if (!File.Exists(IniFile)) return null;
            var lines = File.ReadAllLines(IniFile);
            bool inSection = false;
            foreach (var line in lines)
            {
                if (line.StartsWith($"[{section}]"))
                {
                    inSection = true;
                    continue;
                }

                if (line.StartsWith("[") && inSection)
                {
                    break;
                }

                if (inSection && line.Contains("="))
                {
                    var parts = line.Split(new[] { '=' }, 2);
                    if (parts[0].Trim() == key)
                    {
                        return parts[1].Trim();
                    }
                }
            }

            return null; // Key not found
        }

        public static void WriteValue(string section, string key, string value)
        {
            // Read all lines from the INI file or create an empty list if the file does not exist
            var lines = File.Exists(IniFile) ? new List<string>(File.ReadAllLines(IniFile)) : new List<string>();

            bool inSection = false;
            bool sectionFound = false;
            bool keyFound = false;
            int sectionIndex = -1;

            // Iterate through the lines to find the correct place to add or update the key-value pair
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];

                if (line.StartsWith($"[{section}]"))
                {
                    inSection = true;
                    sectionFound = true;
                    sectionIndex = i;
                }
                else if (line.StartsWith("["))
                {
                    if (inSection)
                    {
                        inSection = false;
                    }
                }

                if (inSection && line.Contains("="))
                {
                    var parts = line.Split(new[] { '=' }, 2);
                    if (parts[0].Trim() == key)
                    {
                        lines[i] = $"{key}={value}";
                        keyFound = true;
                        break;
                    }
                }
            }

            // If the section was not found, add it
            if (!sectionFound)
            {
                lines.Add(""); // Add an empty line before the new section
                lines.Add($"[{section}]");
                sectionIndex = lines.Count - 2;
            }

            // If the section is found but the key was not found, add the key-value pair
            if (inSection && !keyFound)
            {
                lines.Insert(sectionIndex + 1, $"{key}={value}");
            }

            // Write all lines back to the file
            File.WriteAllLines(IniFile, lines);
        }

        // Read and convert values from the INI file
        public static int GetHotkey(string key)
        {
            string value = ReadValue("hotkeys", key);
            if (value != null)
            {
                return Convert.ToInt16(value);
            }
            return 0; // Default value if conversion fails
        }

        public static void SetHotkey(string key, int value)
        {
            WriteValue("hotkeys", key, value.ToString());
        }

        public static int GetPreference(string key)
        {
            string value = ReadValue("preferences", key);
            return int.TryParse(value, out int result) ? result : 0;
        }

    }
}