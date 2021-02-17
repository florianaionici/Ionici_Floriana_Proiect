using System;
using System.ComponentModel.DataAnnotations;
namespace Ionici_Floriana_Proiect.Models.BakeryViewModels
{
    public class OrderGroup
    {
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }
        public int CakeCount { get; set; }

    }
}