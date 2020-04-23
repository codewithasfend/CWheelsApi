using Microsoft.EntityFrameworkCore;
using VehiclesProjectApi.Models;

namespace VehiclesProjectApi.Data
{
    /// <summary>
    /// DBContext for Vehicles Project
    /// </summary>
    public class VehiclesProjectDbContext : DbContext
    {
        /// <summary>
        /// CTOR.
        /// </summary>
        /// <param name="options"></param>
        public VehiclesProjectDbContext(DbContextOptions<VehiclesProjectDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DBSet for Users.
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// DBSet for Vehicles.
        /// </summary>
        public DbSet<Vehicle> Vehicles { get; set; }
        /// <summary>
        /// DBSet for Vehicl Images.
        /// </summary>
        public DbSet<ImageModel> Images { get; set; }
        /// <summary>
        /// DBSet for Categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; }
    }
}