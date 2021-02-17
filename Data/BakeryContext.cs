using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ionici_Floriana_Proiect.Models;

namespace Ionici_Floriana_Proiect.Data
{
    public class BakeryContext : DbContext
    {
        public BakeryContext(DbContextOptions<BakeryContext> options) :
       base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cake> Cake { get; set; }
        public DbSet<Owners> Owners { get; set; }
        public DbSet<OwnedCake> OwnedCake { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Cake>().ToTable("Cake");
            modelBuilder.Entity<Owners>().ToTable("Owners");
            modelBuilder.Entity<OwnedCake>().ToTable("OwnedCake");
            modelBuilder.Entity<OwnedCake>()
            .HasKey(c => new { c.CakeID, c.OwnerID });
        }
    }
}
