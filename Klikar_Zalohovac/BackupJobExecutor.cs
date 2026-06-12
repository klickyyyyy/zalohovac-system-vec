using Quartz;
using System.Text.Json;

namespace Zalohovac
{
    public class BackupJobExecutor : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            string json = context.JobDetail.JobDataMap.GetString("jobConfig")!;
            BackupJob config = JsonSerializer.Deserialize<BackupJob>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Spouštím zálohu: {config.Name} (metoda: {config.Method})");

            foreach (string source in config.Sources)
            {
                foreach (string target in config.Targets)
                {
                    try
                    {
                        CopyDirectory(source, target);
                        Console.WriteLine($"  {source} -> {target} OK");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  Chyba: {source} -> {target}: {ex.Message}");
                    }
                }
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Záloha dokončena: {config.Name}");
            return Task.CompletedTask;
        }

        private void CopyDirectory(string source, string target)
        {
            if (!Directory.Exists(source))
                return;

            Directory.CreateDirectory(target);

            foreach (string file in Directory.GetFiles(source))
            {
                string fileName = Path.GetFileName(file);
                File.Copy(file, Path.Combine(target, fileName), overwrite: true);
            }

            foreach (string dir in Directory.GetDirectories(source))
            {
                string dirName = new DirectoryInfo(dir).Name;
                CopyDirectory(dir, Path.Combine(target, dirName));
            }
        }
    }
}
