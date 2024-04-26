using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MudBlazor;

namespace DMCockpit_Library.Services
{
    public interface ISettingsManager
    {
        Dictionary<string, string> PlaylistDictionary { get; }
        Dictionary<string, Tuple<string, string>> IFrames { get; }

        void SaveSettings();
    }

    public class SettingsManager : ISettingsManager
    {
        private readonly DMCockpitSettings settings;

        public SettingsManager()
        {
            var settingsFilePath = Environment.GetEnvironmentVariable("DM_COCKPIT_SETTINGS_OVERRIDE") ?? "settings.json";

            settings = JsonSerializer.Deserialize<DMCockpitSettings>(File.ReadAllText(settingsFilePath));
        }
        
        public Dictionary<string, string> PlaylistDictionary => settings.PlayListDictionary;
        public Dictionary<string, Tuple<string, string>> IFrames => settings.IFrameURLs;

        public void SaveSettings()
        {
            WriteToJSON("settings.json");
        }

        internal void WriteToJSON(string filePath)
        {
            try
            {
                JsonSerializerOptions options = new()
                {
                    WriteIndented = true,
                    IncludeFields = true
                };

                var currentDir = AppDomain.CurrentDomain.BaseDirectory;
                string json = JsonSerializer.Serialize(settings, options);
                filePath = Path.Combine(currentDir, filePath);
                File.WriteAllText(filePath, json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public class DMCockpitSettings
    {
        public Dictionary<string, string> PlayListDictionary { get; set; }
        public Dictionary<string, Tuple<string, string>> IFrameURLs { get; set; }
    }
}
