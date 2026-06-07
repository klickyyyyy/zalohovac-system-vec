using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZalohovacServer.Entities.DB
{
    [Table("source")]
    public class Source
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("directory")]
        public string Directory { get; set; } = string.Empty;

        [Column("job_id")]
        public int JobId { get; set; }
    }
}
