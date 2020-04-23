using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using VehiclesProjectApi.Data;
using VehiclesProjectApi.Helpers;
using VehiclesProjectApi.Models;

namespace VehiclesProjectApi.Controllers
{
    /// <summary>
    /// Car images.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly VehiclesProjectDbContext _dbContext;

        /// <summary>
        /// Car Images CTOR
        /// </summary>
        /// <param name="dbContext"></param>
        public ImagesController(VehiclesProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// POST: api/Images
        /// </summary>
        /// <param name="imageModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] ImageModel imageModel)
        {
            var stream = new MemoryStream(imageModel.ImageArray);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var folder = "wwwroot/images";
            var fullPath = $"{folder}/{file}";
            var imageFullPath = fullPath.Remove(0, 7);
            var response = FilesHelper.UploadPhoto(stream, folder, file);
            if (response)
            {
                imageModel.ImageUrl = imageFullPath;
                var image = new ImageModel()
                {
                    ImageUrl = imageModel.ImageUrl,
                    VehicleId = imageModel.VehicleId,
                };
                _dbContext.Images.Add(image);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}