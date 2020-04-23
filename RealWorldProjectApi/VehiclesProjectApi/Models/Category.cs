using System.Collections.Generic;

namespace VehiclesProjectApi.Models
{
    /// <summary>
    /// Vehicle categories.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Vehicle ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Vehicle Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Collection of Vehicles.
        /// </summary>
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}