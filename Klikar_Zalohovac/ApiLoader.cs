using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zalohovac
{
    public class ApiLoader
    {
        // UUID tohoto pocitace - podle toho server vi, ktere ulohy poslat
        private const string COMPUTER_UUID = "d0ce5934-ac37-4222-9eda-1b7c8d918df9";
        private const string SERVER_URL = "http://localhost:5000";

        public static List<BackupJob> LoadFromServer()
        {
            using HttpClient client = new HttpClient();

            string url = $"https://localhost:7000/api/client/{COMPUTER_UUID}/jobs";

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Chyba pri nacitani z API: {response.StatusCode}");
                return new List<BackupJob>();
            }

            string json = response.Content.ReadAsStringAsync().Result;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            return JsonSerializer.Deserialize<List<BackupJob>>(json, options) ?? new List<BackupJob>();
        }
    }
}
