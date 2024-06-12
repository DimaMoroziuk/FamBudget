using ExcelGen.Repository.AuthorizationData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ExcelGen.Repository.Models
{
    public class Purchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime AddedDate { get; set; }
        public int Price { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        [ForeignKey("CategoryId")]
        public string CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("AuthorId")]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
    }
}
