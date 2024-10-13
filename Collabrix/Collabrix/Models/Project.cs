using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("Projects")]
    public class Project
    {
        [Key]
        public Guid ProjectId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string ProjectName { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(50)]
        public required string Type { get; set; }

        [MaxLength(50)]
        public required string Status { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
