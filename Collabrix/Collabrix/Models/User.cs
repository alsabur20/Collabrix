using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Username { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public required string PasswordHash { get; set; }

        [MaxLength(255)]
        public string? ProfilePicture { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
