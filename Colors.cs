namespace CompBuddy
{
    public static class Colors
    {
        private static Color _dayText => Color.Black;
        private static Color _dayBackground => Color.White;
        private static Color _dayLight => Color.DarkGray;
        private static Color _dayMedium => Color.Gray;
        private static Color _dayDisabled => Color.LightGray;

        private static Color _nightText => Color.White;
        private static Color _nightBackground => Color.DarkGray;
        private static Color _nightLight => Color.LightGray;
        private static Color _nightMedium => Color.Gray;
        private static Color _nightDisabled => Color.Gray;

        public static Color Slayers => Color.Red;
        public static Color Merchants => Color.DarkOrchid;
        public static Color Builders => Color.Turquoise;
        public static Color Rangers => Color.LimeGreen;
        public static Color Explorers => Color.OrangeRed;
        public static Color Hobbits => Color.Goldenrod;
        public static Color Mages => Color.Blue;

        public static Color Text => Config.NightMode ? _nightText : _dayText;
        public static Color Background => Config.NightMode ? _nightBackground : _dayBackground;
        public static Color Light => Config.NightMode ? _nightLight : _dayLight;
        public static Color Medium => Config.NightMode ? _nightMedium : _dayMedium;
        public static Color Disabled => Config.NightMode ? _nightDisabled : _dayDisabled;
    }
}