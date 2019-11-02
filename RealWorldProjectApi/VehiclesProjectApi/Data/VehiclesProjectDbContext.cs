using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehiclesProjectApi.Models;

namespace VehiclesProjectApi.Data
{
    public class VehiclesProjectDbContext : DbContext
    {
        public VehiclesProjectDbContext(DbContextOptions<VehiclesProjectDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ImageModel> Images { get; set; }    
        public DbSet<Category> Categories { get; set; }
    }
}
