using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collabrix.Models
{
    [Table("ProjectTaskStage")]
    public class ProjectTaskStage
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? ProjectId { get; set; }

        public Guid? StageId { get; set; }
    }
}
