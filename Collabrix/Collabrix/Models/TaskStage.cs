using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("TaskStages")]
    public class TaskStage
    {
        [Key]
        public Guid StageId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string StageName { get; set; }

        public string? StageDescription { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
