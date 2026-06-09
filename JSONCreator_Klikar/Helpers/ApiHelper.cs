using JSONCreator.Entities;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JSONCreator.Helpers
{
    public class ApiHelper
    {
        private const string SERVER_URL = "http://localhost:5000";

        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        // ulozime JWT token po prihlaseni
        public static string? AuthToken { get; set; }

        private static HttpClient MakeClient()
        {
            HttpClient client = new HttpClient();
            if (AuthToken != null)
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthToken}");
            return client;
        }

        // prihlaseni - vrati token nebo null pri chybe
        public static string? Login(string username, string password)
        {
            using HttpClient client = new HttpClient();

            var body = new { Username = username, Password = password };
            string json = JsonSerializer.Serialize(body);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync($"{SERVER_URL}/api/auth/login", content).Result;

            if (!response.IsSuccessStatusCode)
                return null;

            string responseJson = response.Content.ReadAsStringAsync().Result;
            var token = JsonSerializer.Deserialize<JsonElement>(responseJson);
            return token.GetProperty("tokenString").GetString();
        }

        // nacte vsechny joby ze serveru
        public static List<BackupJob> GetAllJobs()
        {
            using HttpClient client = MakeClient();

            HttpResponseMessage response = client.GetAsync($"{SERVER_URL}/api/job").Result;

            if (!response.IsSuccessStatusCode)
                return new List<BackupJob>();

            string json = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<List<BackupJob>>(json, _options) ?? new List<BackupJob>();
        }

        // prida novy job
        public static bool CreateJob(BackupJob job)
        {
            using HttpClient client = MakeClient();

            string json = JsonSerializer.Serialize(job, _options);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync($"{SERVER_URL}/api/job", content).Result;
            return response.IsSuccessStatusCode;
        }

        // upravi existujici job (id je index v listu + 1, server si drzi vlastni ID)
        public static bool UpdateJob(int serverId, BackupJob job)
        {
            using HttpClient client = MakeClient();

            string json = JsonSerializer.Serialize(job, _options);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PutAsync($"{SERVER_URL}/api/job/{serverId}", content).Result;
            return response.IsSuccessStatusCode;
        }

        // smaze job podle server ID
        public static bool DeleteJob(int serverId)
        {
            using HttpClient client = MakeClient();

            HttpResponseMessage response = client.DeleteAsync($"{SERVER_URL}/api/job/{serverId}").Result;
            return response.IsSuccessStatusCode;
        }
    }
}
