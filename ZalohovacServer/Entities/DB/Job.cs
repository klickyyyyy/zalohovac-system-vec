using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZalohovacServer.Entities.DB
{
    [Table("job")]
    public class Job
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("timing")]
        public string Timing { get; set; } = string.Empty;

        [Column("method")]
        public string Method { get; set; } = string.Empty;

        [Column("retention_count")]
        public int RetentionCount { get; set; }

        [Column("retention_size")]
        public int RetentionSize { get; set; }

        public virtual List<Source> Sources { get; set; } = new();
        public virtual List<Target> Targets { get; set; } = new();
    }
}
