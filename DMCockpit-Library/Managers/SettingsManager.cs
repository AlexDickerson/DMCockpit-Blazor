using DMCockpit_Library.Models;
using DMCockpit_Library.Services;
using System.Text.Json;

namespace DMCockpit_Library.Managers
{
    public interface ISettingsManager
    {
        Dictionary<string, string> PlaylistDictionary { get; }
        Dictionary<string, Tuple<string, string>> IFrames { get; }

        Tuple<int, int> GetLastWindowPosition(string windowName);

        void SaveSettings();
        void SaveWindowLocation(string windowName, int x, int y);
    }

    public class SettingsManager : ISettingsManager
    {
        public Dictionary<string, string> PlaylistDictionary => settings.PlayListDictionary;
        public Dictionary<string, Tuple<string, string>> IFrames => settings.IFrameURLs;

        private readonly IDMCockpitConfigurationService config;
        private readonly string settingsFilePath;
        private readonly Settings settings;

        public SettingsManager(IDMCockpitConfigurationService config)
        {
            this.config = config;
            this.settingsFilePath = config.GetAppFilePath(config.GetEnvironmentVariable("DMCOCKPIT_SETTINGS_FILE_PATH") ?? "settings.json");
            this.settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(settingsFilePath)) ?? new Settings();
        }

        public Tuple<int, int> GetLastWindowPosition(string windowName)
        {
            if (settings.WindowLocations.ContainsKey(windowName))
            {
                return settings.WindowLocations[windowName];
            }
            return new Tuple<int, int>(0, 0);
        }

        public void SaveWindowLocation(string windowName, int x, int y)
        {
            if (settings.WindowLocations.ContainsKey(windowName))
            {
                settings.WindowLocations[windowName] = new Tuple<int, int>(x, y);
            }
            else
            {
                settings.WindowLocations.Add(windowName, new Tuple<int, int>(x, y));
            }

            SaveSettings();
        }

        public void SaveSettings() => config.WriteAppJSONFile(settingsFilePath, settings);
    }
}