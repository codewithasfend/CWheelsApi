using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VehiclesProjectApi.Models
{
    /// <summary>
    /// Vehicle info.
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Vehicle ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Vehicle Name
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Vehicle Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Vehicle Price
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Vehicle Model
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Vehicle Engine Type
        /// </summary>
        public string Engine { get; set; }
        /// <summary>
        /// Vehicle Color
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// Vehicle Brand
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// Vehicle Year
        /// </summary>
        public DateTime Assembly { get; set; }
        /// <summary>
        /// Date Posted
        /// </summary>
        public DateTime DatePosted { get; set; }
        /// <summary>
        /// Is New?
        /// </summary>
        public bool IsHotAndNew { get; set; }
        /// <summary>
        /// Featured?
        /// </summary>
        public bool IsFeatured { get; set; }
        /// <summary>
        /// Location of Vehicle 
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Vehicle Condition
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// User Selling vehicle
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Vehicle category
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Vehicle Images
        /// </summary>
        public ICollection<ImageModel> Images { get; set; }
    }
}