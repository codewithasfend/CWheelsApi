using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VehiclesProjectApi.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int VehicleId { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }
    }
}
