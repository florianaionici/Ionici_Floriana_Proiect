using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ionici_Floriana_Proiect.Models
{
    public class OwnedCake
    {
        public int OwnerID { get; set; }
        public int CakeID { get; set; }
        public Owners Owners { get; set; }
        public Cake Cake { get; set; }
    }
}