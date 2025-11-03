using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoApp.Models
{
    [Table("todoGroup")]
    public class TodoGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupNo { get; set; } // AUTO_INCREMENT

        [Required, MaxLength(128)]
        public string UserId { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Title { get; set; } = string.Empty;
    }
}
