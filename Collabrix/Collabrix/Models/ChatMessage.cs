using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{

    [Table("ChatMessages")]
    public class ChatMessage
    {
        [Key]
        public Guid MessageId { get; set; }

        public Guid? ChatRoomId { get; set; }

        public Guid? UserId { get; set; }

        [Required]
        public required string MessageContent { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
