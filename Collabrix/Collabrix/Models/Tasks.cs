using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("Tasks")]
    public class Tasks
    {
        [Key]
        public Guid TaskId { get; set; }

        public Guid? ProjectId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }

        public string? Description { get; set; }

        public Guid? AssignedTo { get; set; }

        public Guid? StageId { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
