using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("FileAttachments")]
    public class FileAttachment
    {
        [Key]
        public Guid AttachmentId { get; set; }

        public Guid? TaskId { get; set; }

        public Guid? MessageId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string FilePath { get; set; }

        public Guid? UploadedBy { get; set; }

        public DateTime? UploadedAt { get; set; }
    }
}
