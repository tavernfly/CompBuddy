namespace CompBuddy
{
    public static class Hotkeys
    {
        public static int Plus = 0xBB;
        public static int Space = 0x20;
        public static int H = 0x48;
        public static int P = 0x50;
        public static int C = 0x43;
        public static int ModNone = 0x0000;
        public static int ModAlt = 0x0001;
        public static int ModCtrl = 0x0002;

        public static int CounterKeyDefault = Plus;
        public static int CounterModDefault = ModNone;
        public static int CounterKey = CounterKeyDefault;
        public static int IncrementModifier = CounterModDefault;
        public static int DecrementModifier = ModAlt;

        public static int TimerKey = Space;
        public static int TimerModifier = ModCtrl;
        public static int PrintKey = P;
        public static int PrintModifier = ModCtrl;
        public static int HideKey = H;
        public static int HideModifier = ModCtrl;
        public static int ResetTimerKey = Space;
        public static int ResetTimerModifier = ModAlt;
        public static int ResetAllKey = C;
        public static int ResetAllModifier = ModAlt;

        public static int OneKey = 0x31;
        public static int TwoKey = 0x32;
        public static int ThreeKey = 0x33;
        public static int FourKey = 0x34;
        public static int FiveKey = 0x35;
        public static int SixKey = 0x36;
        public static int SevenKey = 0x37;
        public static int EightKey = 0x38;
        public static int NineKey = 0x39;

        public static int NumOneKey = 0x61;
        public static int NumTwoKey = 0x62;
        public static int NumThreeKey = 0x63;
        public static int NumFourKey = 0x64;
        public static int NumFiveKey = 0x65;
        public static int NumSixKey = 0x66;
        public static int NumSevenKey = 0x67;
        public static int NumEightKey = 0x68;
        public static int NumNineKey = 0x69;

        public static void SetCounterKey(int vkc, int mod)
        {
            CounterKey = vkc;
            IncrementModifier = mod;
            Config.SetHotkey("CounterKey", vkc);
            Config.SetHotkey("IncrementModifier", mod);
        }
        public static string CounterKeyName
        {
            get { return $"{GetName(CounterKey)}"; }
        }
        public static string IncrementKeyName
        {
            get { return $"{((IncrementModifier == ModCtrl) ? "Ctrl+" : "")}{CounterKeyName}"; }
        }
        public static string DecrementKeyName
        {
            get { return $"Alt+{CounterKeyName}"; }
        }
        public static string TimerKeyName = "Ctrl+Space";
        public static string PrintKeyName = "Ctrl+P";
        public static string HideKeyName = "Ctrl+H";
        public static string ResetTimerKeyName = "Alt+Space";
        public static string ResetAllKeyName = "Alt+C";

        public static string GetName(int vkc)
        {
            AllowedKeys.TryGetValue(vkc, out string name);
            return name;
        }

        public static Dictionary<int, string> AllowedKeys = new Dictionary<int, string>
        {
            { 0x41, "A" },
            { 0x42, "B" },
            { 0x44, "D" },
            { 0x45, "E" },
            { 0x46, "F" },
            { 0x47, "G" },
            { 0x48, "H" },
            { 0x49, "I" },
            { 0x4A, "J" },
            { 0x4B, "K" },
            { 0x4C, "L" },
            { 0x4D, "M" },
            { 0x4E, "N" },
            { 0x4F, "O" },
            { 0x51, "Q" },
            { 0x52, "R" },
            { 0x53, "S" },
            { 0x54, "T" },
            { 0x55, "U" },
            { 0x57, "W" },
            { 0x59, "Y" },
            { 0xBA, ";" },
            { 0xBB, "Plus" },
            { 0xBC, "Comma" },
            { 0xBD, "Minus" },
            { 0xBE, "Period" },
            { 0xBF, "?" },
            { 0xC0, "Tilde" },
            { 0xDB, "]" },
            { 0xDC, "\\" },
            { 0xDD, "]" },
            { 0xDE, "Quote" },
            { 0x6A, "Mult" },
            { 0x6B, "Add" },
            { 0x6C, "Sep" },
            { 0x6D, "Subtr" },
            { 0x6E, "Decim" },
            { 0x6F, "Divide" },
        };
    }
}