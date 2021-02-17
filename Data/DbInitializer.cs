using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ionici_Floriana_Proiect.Models;


namespace Ionici_Floriana_Proiect.Data
{
    public class DbInitializer
    {
        public static void Initialize(BakeryContext context)
        {
            context.Database.EnsureCreated();
            if (context.Cake.Any())
            {
                return;
            }

                var cakes = new Cake[]
            {
 new Cake{Name="Tiramisu",Baker="Simon Matei",Price=Decimal.Parse("25")},
 new Cake{Name="Profiterol",Baker="Maria Pop",Price=Decimal.Parse("50")},
 new Cake{Name="Negresa",Baker="Mihai Constantin",Price=Decimal.Parse("15")},
 new Cake{Name="Amandina",Baker="Celia Popescu",Price=Decimal.Parse("22")},
 new Cake{Name="Musuroi de cartita",Baker="Elena Constantin",Price=Decimal.Parse("40")},
 new Cake{Name="Brownies",Baker="Emilia Calinescu",Price=Decimal.Parse("33")},
            };
            if (!context.Cake.Any())
            {
                foreach (Cake b in cakes)
                {
                    context.Cake.Add(b);
                }
                context.SaveChanges();
            }
            var customers = new Customer[]
            {

 new Customer{Name="Popescu Marcela",BirthDate=DateTime.Parse("1989-04-20")},
 new Customer{Name="Mihailesc Cornel",BirthDate=DateTime.Parse("2000-07-12")},

            };
            if (!context.Customers.Any())
            {
                foreach (Customer c in customers)
                {
                    context.Customers.Add(c);
                }
                context.SaveChanges();
            }
            var orders = new Order[]
            {
 new Order{Cake = context.Cake.Single(c => c.Name == "Tiramisu" ), Customer = context.Customers.Single(c => c.Name == "Popescu Marcela" ),OrderDate=DateTime.Parse("12-2-2020")},
 new Order{Cake = context.Cake.Single(c => c.Name == "Amandina" ), Customer = context.Customers.Single(c => c.Name == "Mihailesc Cornel" ),OrderDate=DateTime.Parse("05-20-2020")},
 };

            if (!context.Orders.Any()) { 
                foreach (Order e in orders)
                {
                    context.Orders.Add(e);
                }
            context.SaveChanges();
        }
            var owners = new Owners[]
            {

 new Owners{OwnerName="Felix Dumitru",Adress="Str. Garii, nr. 5, Targu-Jiu"},
 new Owners{OwnerName="Maria Dragomir",Adress="Str. Plopilor, nr. 12, Sibiu"},
 new Owners{OwnerName="Adrian Cercel",Adress="Str. Soarelui, nr.10, Cluj-Napoca"},
            };
            foreach (Owners p in owners)
            {
                context.Owners.Add(p);
            }
            context.SaveChanges();
            var ownedcake = new OwnedCake[]
            {
 new OwnedCake {CakeID = context.Cake.Single(c => c.Name == "Tiramisu" ).ID,
     OwnerID = context.Owners.Single(i => i.OwnerName =="Felix Dumitru").ID,
 Cake = context.Cake.Single(c => c.Name == "Tiramisu" ),
 Owners = context.Owners.Single(i => i.OwnerName =="Felix Dumitru")
 }
            };
            foreach (OwnedCake pb in ownedcake)
            {
                context.OwnedCake.Add(pb);
            }
            context.SaveChanges();
        }
    }
}
