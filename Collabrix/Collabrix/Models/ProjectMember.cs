using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("ProjectMembers")]
    public class ProjectMember
    {
        [Key]
        public Guid ProjectMemberId { get; set; }

        public Guid? ProjectId { get; set; }

        public Guid? UserId { get; set; }

        [MaxLength(50)]
        public required string Role { get; set; }

        public DateTime? JoinedAt { get; set; }
    }
}
