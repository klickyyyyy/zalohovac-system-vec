using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZalohovacServer.Entities.DB
{
    [Table("assignment")]
    public class Assignment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("computer_uuid")]
        public Guid? ComputerUuid { get; set; }

        [Column("job_id")]
        public int JobId { get; set; }

        [Column("assign_at")]
        public DateTime AssignAt { get; set; }
    }
}