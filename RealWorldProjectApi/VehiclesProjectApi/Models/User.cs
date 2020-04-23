using System.Collections.Generic;

namespace VehiclesProjectApi.Models
{
    /// <summary>
    /// User info.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// User name: First and Last.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User password.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// User phone number.
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// User image.
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// User's vehicle(s).
        /// </summary>
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}