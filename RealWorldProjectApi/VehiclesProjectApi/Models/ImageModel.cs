using System.ComponentModel.DataAnnotations.Schema;

namespace VehiclesProjectApi.Models
{
    /// <summary>
    /// Vehicl Images.
    /// </summary>
    public class ImageModel
    {
        /// <summary>
        /// Image ID.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Image url.
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Vehicle ID.
        /// </summary>
        public int VehicleId { get; set; }

        /// <summary>
        /// Image array.
        /// </summary>
        [NotMapped]
        public byte[] ImageArray { get; set; }
    }
}