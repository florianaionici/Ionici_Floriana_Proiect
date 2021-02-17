using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ionici_Floriana_Proiect.Models
{
    public class Owners
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Owner Name")]
        [StringLength(50)]
        public string OwnerName { get; set; }

        [StringLength(70)]
        public string Adress { get; set; }
        public ICollection<OwnedCake> OwnedCake { get; set; }
    }
}
