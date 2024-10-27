using HamnApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamnApp.Models
{
    public class HarborContext : DbContext
    {
        public virtual DbSet<Boat> Boats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Boat>().HasKey(b => b.Id);
            
            //modelBuilder.Entity<Boat>()
            //    .HasIndex(b => b.SpotId)
            //    .IsUnique();

            //modelBuilder.Entity<Boat>().HasData(
            //    new RowBoat(),
            //    new MotorBoat(),
            //    new SailBoat(),
            //    new CargoShip()
            //);

        }
    }
}
