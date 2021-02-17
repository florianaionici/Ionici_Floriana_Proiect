using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ionici_Floriana_Proiect.Models
{
    public class Cake
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
            public string Name { get; set; }
            public string Baker { get; set; }
           public decimal Price { get; set; }
            public ICollection<Order> Orders { get; set; }
        public ICollection<OwnedCake> OwnedCake{ get; set; }

    }
}
