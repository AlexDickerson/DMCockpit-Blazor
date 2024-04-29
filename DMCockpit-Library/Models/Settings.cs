namespace DMCockpit_Library.Models
{
    public class Settings
    {
        public Dictionary<string, string> PlayListDictionary { get; set; }
        public Dictionary<string, Tuple<string, string>> IFrameURLs { get; set; }
        public Dictionary<string, Tuple<int, int>> WindowLocations { get; set; }

        public Settings()
        {
            PlayListDictionary = new Dictionary<string, string>();
            IFrameURLs = new Dictionary<string, Tuple<string, string>>();
            WindowLocations = new Dictionary<string, Tuple<int, int>>();
        }
    }
}
