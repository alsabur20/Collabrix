using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("TaskComments")]
    public class TaskComment
    {
        [Key]
        public Guid CommentId { get; set; }

        public Guid? TaskId { get; set; }

        public Guid? UserId { get; set; }

        [Required]
        public required string Content { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
