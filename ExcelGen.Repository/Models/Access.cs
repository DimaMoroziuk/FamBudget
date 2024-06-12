using ExcelGen.Repository.AuthorizationData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelGen.Repository.Models
{
    public class Access
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey("AuthorId")]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        [ForeignKey("AccessRecieverId")]
        public string AccessRecieverId { get; set; }
        public ApplicationUser AccessReciever { get; set; }

        public int AccessType { get; set; }
    }
}
