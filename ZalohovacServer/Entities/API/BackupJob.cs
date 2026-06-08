namespace ZalohovacServer.Entities.API
{
    public class BackupJob
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> Sources { get; set; } = new();
        public List<string> Targets { get; set; } = new();
        public string Timing { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public BackupRetention Retention { get; set; } = new();
    }
}
