using System.Text.Json;

namespace DMCockpit_Library.Services
{
    public interface IDMCockpitConfigurationService
    {
        string? GetEnvironmentVariable(string variable);
        string GetAppFilePath(string fileName);
        void WriteAppJSONFile(string fileName, object content);
    }

    public class DMCockpitConfigurationService : IDMCockpitConfigurationService
    {
        public string? GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable) ?? null;
        }

        public string GetAppFilePath(string filePath)
        {
            var currentDir = AppDomain.CurrentDomain.BaseDirectory;

            if(!filePath.Contains(':'))
            {
                filePath = Path.Combine(currentDir, filePath);
            }

            if (!File.Exists(filePath))
            {
                throw new Exception($"File {filePath} not found");
            }

            return filePath;
        }

        public void WriteAppJSONFile(string filePath, object content)
        {
            var currentDir = AppDomain.CurrentDomain.BaseDirectory;

            if (!filePath.Contains(':'))
            {
                filePath = Path.Combine(currentDir, filePath);
            }

            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                IncludeFields = true
            };

            string json = JsonSerializer.Serialize(content, options);
            File.WriteAllText(filePath, json);
        }
    }
}
