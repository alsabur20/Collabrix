using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("ChatRooms")]
    public class ChatRoom
    {
        [Key]
        public Guid ChatRoomId { get; set; }

        public Guid? ProjectId { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
