using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ionici_Floriana_Proiect.Models.BakeryViewModels
{
    public class OwnersIndexData
    {
        public IEnumerable<Owners> Owners  { get; set; }
        public IEnumerable<Cake> Cakes { get; set; }
        public IEnumerable<Order> Orders { get; set; }

    }
}
