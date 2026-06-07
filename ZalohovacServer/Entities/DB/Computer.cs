using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ZalohovacServer.Entities.DB
{
    [Table("computer")]
    public class Computer
    {
        [Key]
        [Column("uuid")]
        public Guid? Uuid { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("enabled")]
        public bool Enabled { get; set; } = true;

        [JsonIgnore]
        public virtual List<Assignment> Assignments { get; set; } = new();
    }
}